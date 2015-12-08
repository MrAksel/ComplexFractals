using FractalRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Numerics;
using System.Threading;
using System.Collections.Concurrent;
using System.Drawing.Imaging;

namespace LocalRenderers.Newton
{
    [FractalRenderer("Newton", "Local renderer", true)]
    public class LocalNewtonRenderer : AbstractRenderer
    {
        private const double ln2 = 0.693147180559945309417232121458176568075500134360255254120680; // Didn't bother with truncating the digits..

        private ConcurrentDictionary<int, CancellationTokenSource> activeTasks;
        private LocalNewtonRendererSettingsControl settingsControl;

        public LocalNewtonRenderer()
        {
            activeTasks = new ConcurrentDictionary<int, CancellationTokenSource>();
            settingsControl = new LocalNewtonRendererSettingsControl();
        }


        public override SettingsWindow GetSettingsWindow()
        {
            return settingsControl;
        }

        public override void AbortRender(int task)
        {
            if (activeTasks.ContainsKey(task))
            {
                activeTasks[task].Cancel();
            }
        }

        public override Bitmap DrawPreview(Size size)
        {
            NewtonTaskOptions opt = CreatePreviewOptions(size);
            return DrawFractal(opt);
        }

        public override Bitmap DrawFractal(Size size)
        {
            NewtonTaskOptions opt = CreateRenderOptions(size);
            return DrawFractal(opt);
        }

        public Bitmap DrawFractal(NewtonTaskOptions opt)
        {
            bool success = false;
            Bitmap bmref = null;
            ManualResetEvent set = new ManualResetEvent(false);

            RenderComplete cb = new RenderComplete((tnum, usr, bm) =>
            {
                success = true;
                bmref = bm; // TODO Should maybe check whether tnum == task
                set.Set();
            });

            RenderAborted ab = new RenderAborted((tnum, usr) =>
            {
                success = false;
                set.Set();
            });

            opt.TaskComplete = opt.TaskComplete + cb; // TODO cb or users delegate first?

            int task = StartRenderAsync(opt);
            set.WaitOne();

            opt.TaskComplete = opt.TaskComplete - cb;

            if (success)
                return bmref;
            else
                return null;
        }

        public override int StartRenderAsync(Size size, RenderComplete completeCallback, RenderProgress progressCallback, RenderAborted abortedCallback, object user)
        {
            if (size.Width < 1 || size.Height < 1 || completeCallback == null) // No point in starting if the area is zero or noone gets notified when its completed
                return 0; // Zero means not started

            NewtonTaskOptions options = CreateRenderOptions(size);
            options.TaskComplete = completeCallback;
            options.TaskProgress = progressCallback;
            options.TaskAborted = abortedCallback;
            options.User = user;


            return StartRenderAsync(options);
        }

        public int StartRenderAsync(NewtonTaskOptions options)
        {
            if (options.Area == 0)
                return 0;

            CancellationTokenSource source = new CancellationTokenSource();

            int tasknum = GetTaskNumber();
            while (!activeTasks.TryAdd(tasknum, source))
                tasknum = GetTaskNumber();

            NewtonTaskOptions taskopt = options.Clone();
            WaitCallback task = new WaitCallback(delegate
            {
                RenderTaskAsync(taskopt, tasknum, source.Token);
            });
            ThreadPool.QueueUserWorkItem(task);

            return tasknum;
        }


        private unsafe void RenderTaskAsync(NewtonTaskOptions options, int tasknum, CancellationToken cancelToken)
        {
            bool canceled = false;
            Bitmap res = new Bitmap(options.ActualRenderSize.Width, options.ActualRenderSize.Height, PixelFormat.Format24bppRgb);

            if (options.Iterations == 0)
                goto finished;

            BitmapData bd = res.LockBits(new Rectangle(Point.Empty, options.ActualRenderSize), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            byte* start = (byte*)bd.Scan0.ToPointer();
            int pxsize = 3;

            double rinc = (options.RealMax - options.RealMin) / bd.Width;
            double iinc = (options.ImagMax - options.ImagMin) / bd.Height;

            // TODO Supersampling. How about you rather buy some more RAM and render at a higher resolution? Heh
            bool progress = options.TaskProgress != null && options.Updates > 0;
            int pxDone = 0;
            float pxTot = bd.Width * bd.Height;
            int updateinc = (progress ? (int)(pxTot / options.Updates) : int.MaxValue);
            int nextupdate = updateinc;

            bool smooth = (options.Coloring == NewtonColoringAlgorithm.Smooth);

            Func<Complex, Complex, Complex> F = options.Function;
            Func<Complex, Complex, Complex> Fdz = options.Derivative;

            Action<Tuple<int, int, int, int>> worker = new Action<Tuple<int, int, int, int>>((offset) =>
            {
                int xoff = offset.Item1;
                int xjmp = offset.Item2;
                int yoff = offset.Item3;
                int yjmp = offset.Item4;

                for (int y = yoff; y < bd.Height && !cancelToken.IsCancellationRequested; y += yjmp)
                {
                    double mappedi = (double)y / bd.Height * (options.ImagMax - options.ImagMin) + options.ImagMin;
                    double ci = mappedi;

                    byte* row = start + bd.Stride * y;
                    byte* px = row + pxsize * xoff;
                    for (int x = xoff; x < bd.Width; x += xjmp)
                    {
                        double mappedr = (double)x / bd.Width * (options.RealMax - options.RealMin) + options.RealMin;
                        double cr = mappedr;

                        double r = cr;
                        double i = ci;
                        double rr = r * r;
                        double ii = i * i;

                        double distsqr = rr + ii;

                        // TODO Loop on r and i
                        Complex c = new Complex(r, i);
                        Complex z = new Complex(r, i);
                        Complex dz;
                        Complex e = Complex.Zero;
                        double sm = 0.0;
                        int iter = 0;
                        while (iter < options.Iterations)
                        {
                            dz = Fdz(z, c);
                            if (dz.Magnitude < options.Tolerance)
                            {
                                //z = z + 1;
                                //continue;
                            }
                            e = -(F(z, c) / dz);
                            z += e;

                            if (smooth)
                                sm = sm + Math.Pow(Math.E, -Complex.Abs(Complex.Divide(1.0, e)));
                            iter++;
                            if (e.Magnitude < options.Tolerance)
                            {
                                break;
                            }
                        }

                        // TODO Colorize based on root and convergence rate
                        Color root = options.DivergeColor;
                        if (iter < options.Iterations)
                        {
                            // root = options.Palette[iter % (options.Palette.Length - 1) + 1];
                            double hue = z.Phase * 180 / Math.PI;
                            while (hue < 0)
                                hue += 360;
                            double sat = 1.0 / (1 + Math.Exp(-z.Magnitude)); // Squash magnitude to (0, 1)
                            root = ColorFromHSV(hue, sat, 1.0);
                        }
                        byte red, grn, blu;
                        red = grn = blu = 0;
                        switch (options.Coloring)
                        {
                            case NewtonColoringAlgorithm.Smooth:
                                {
                                    double p1 = 1 - sm % 1;
                                    red = (byte)(root.R * p1);
                                    grn = (byte)(root.G * p1);
                                    blu = (byte)(root.B * p1);
                                    break;
                                }
                        }

                        px[0] = blu;
                        px[1] = grn;
                        px[2] = red;
                        px += pxsize * xjmp;
                    }

                    if (progress)
                    {
                        int numpx = Interlocked.Add(ref pxDone, bd.Width / xjmp);
                        int updat = nextupdate;
                        if (numpx > updat)
                        {
                            options.TaskProgress(tasknum, options.User, numpx / pxTot);
                            Interlocked.CompareExchange(ref nextupdate, numpx + updateinc, updat); // If its not the same another thread has already updated it
                        }
                    }
                }
                if (cancelToken.IsCancellationRequested)
                {
                    canceled = true;
                }
            });

            if (options.MultiThreaded)
            {
                IEnumerable<Tuple<int, int, int, int>> offsets = CreateOffsets();
                Parallel.ForEach(offsets, worker); // Does actual work
            }
            else
            {
                worker(new Tuple<int, int, int, int>(0, 1, 0, 1)); // Work on a single thread
            }

            res.UnlockBits(bd);

            finished:

            if (!canceled)
            {
                res = new Bitmap(res, options.Size);

                if (options.TaskProgress != null)
                    options.TaskProgress(tasknum, options.User, 1f);

                if (options.TaskComplete != null)
                    options.TaskComplete(tasknum, options.User, res);
            }
            else
            {
                if (options.TaskAborted != null)
                {
                    options.TaskAborted(tasknum, options.User);
                }
            }
        }


        private NewtonTaskOptions CreatePreviewOptions(Size size)
        {
            NewtonTaskOptions opt = CreateRenderOptions(size); // Creates options based on the settings control

            opt.AntiAliasingScale = new Size(1, 1);
            
            return opt;
        }

        private NewtonTaskOptions CreateRenderOptions(Size size)
        {
            NewtonTaskOptions opt = new NewtonTaskOptions();
            opt.Size = size;
            opt.Min = settingsControl.Min;
            opt.Max = settingsControl.Max;
            opt.Updates = settingsControl.Updates;
            opt.Coloring = settingsControl.Coloring;
            opt.Function = settingsControl.Function;
            opt.Tolerance = settingsControl.Tolerance;
            opt.Derivative = settingsControl.Derivative;
            opt.Multiplier = settingsControl.Multiplier;
            opt.Iterations = settingsControl.Iterations;
            opt.MultiThreaded = settingsControl.Multithreaded;
            opt.AntiAliasingScale = settingsControl.FullSizeAAScaler;
            return opt;
        }

        private List<Tuple<int, int, int, int>> CreateOffsets()
        {
            int logicalCores = Environment.ProcessorCount;
            int x, y;
            SolveRectangle(logicalCores * 2, out x, out y);

            List<Tuple<int, int, int, int>> tuples = new List<Tuple<int, int, int, int>>();
            for (int a = 0; a < x; a++)
            {
                for (int b = 0; b < y; b++)
                {
                    Tuple<int, int, int, int> t = new Tuple<int, int, int, int>
                        (a, x, b, y);
                    tuples.Add(t);
                }
            }

            return tuples;
        }

        private void SolveRectangle(int area, out int s_a, out int s_b)
        {
            int maxDiv = (int)Math.Sqrt(area) + 1;
            s_a = 1;
            s_b = area;
            for (int a = 1; a < maxDiv; a++)
            {
                int b = area / a;
                if (a * b == area && Math.Abs(a - b) < Math.Abs(s_a - s_b))
                {
                    s_a = a;
                    s_b = b;
                }
            }
        }


        public override bool SupportsZooming()
        {
            return true;
        }

        public override void SetClip(Complex min, Complex max)
        {
            settingsControl.update = false;
            settingsControl.Min = min;
            settingsControl.Max = max;
            settingsControl.update = true;
        }

        public override void GetClip(out Complex min, out Complex max)
        {
            min = settingsControl.Min;
            max = settingsControl.Max;
        }


        private static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }
    }
}
