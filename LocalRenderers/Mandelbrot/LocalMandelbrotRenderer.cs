using FractalRenderer;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Numerics;
using System.Collections.Concurrent;

namespace LocalRenderers.Mandelbrot
{
    [FractalRenderer("Mandelbrot", "Local renderer")]
    public class LocalMandelbrotRenderer : AbstractRenderer
    {
        private const double ln2 = 0.693147180559945309417232121458176568075500134360255254120680; // Didn't bother with truncating the digits..
        
        private ConcurrentDictionary<int, CancellationTokenSource> activeTasks;
        private LocalMandelbrotRendererSettingsControl settingsControl;

        public LocalMandelbrotRenderer()
        {
            activeTasks = new ConcurrentDictionary<int, CancellationTokenSource>();
            settingsControl = new LocalMandelbrotRendererSettingsControl();
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
            MandelbrotTaskOptions opt = CreatePreviewOptions(size);
            return DrawFractal(opt);
        }

        public override Bitmap DrawFractal(Size size)
        {
            MandelbrotTaskOptions opt = CreateRenderOptions(size);
            return DrawFractal(opt);
        }

        public Bitmap DrawFractal(MandelbrotTaskOptions opt)
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

            MandelbrotTaskOptions options = CreateRenderOptions(size);
            options.TaskComplete = completeCallback;
            options.TaskProgress = progressCallback;
            options.TaskAborted = abortedCallback;
            options.User = user;


            return StartRenderAsync(options);
        }

        public int StartRenderAsync(MandelbrotTaskOptions options)
        {
            if (options.Area == 0)
                return 0;

            CancellationTokenSource source = new CancellationTokenSource();

            int tasknum = GetTaskNumber();
            while (!activeTasks.TryAdd(tasknum, source))
                tasknum = GetTaskNumber();

            MandelbrotTaskOptions taskopt = options.Clone();
            WaitCallback task = new WaitCallback(delegate
            {
                RenderTaskAsync(taskopt, tasknum, source.Token);
            });
            ThreadPool.QueueUserWorkItem(task);

            return tasknum;
        }


        private unsafe void RenderTaskAsync(MandelbrotTaskOptions options, int tasknum, CancellationToken cancelToken)
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
            Action<Tuple<int, int, int, int>> worker = new Action<Tuple<int, int, int, int>>((offset) =>
            {
                int xoff = offset.Item1;
                int xjmp = offset.Item2;
                int yoff = offset.Item3;
                int yjmp = offset.Item4;

                int bailout = short.MaxValue; //short.MaxValue;
                int bailsqr = bailout * bailout;
                double ln2lnbailoverln2 = 1 + Math.Log(Math.Log(bailout)) / ln2; // ln (2 * ln(bailout)) / ln2 = (ln2 + ln(ln(bailout))) / ln2 = 1 + ln(ln(bailout)) / ln2

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
                        bool inset = false;
                        if (options.BulbChecking)
                        {
                            // q = (r - 0.25)^2 + ii = rr - r - 0.0625 + ii = distsqr - r - 0.0625
                            double q = distsqr - r / 2 + 0.0625;
                            inset = (q * (q + (r - 0.25)) < ii / 4.0 ||
                                     distsqr + 2 * r < -0.9375);
                        }

                        int iter = 0;
                        if (!inset)
                        {
                            while (iter < options.Iterations && distsqr < bailsqr)
                            {
                                i = r * i;
                                i = i + i + ci;
                                r = rr - ii + cr;

                                rr = r * r;
                                ii = i * i;
                                distsqr = rr + ii;
                                iter++;
                            }
                        }
                        byte red, grn, blu;
                        if (!inset && iter < options.Iterations)
                        {
                            switch (options.Coloring) // Branch prediction will guess correctly after a few tries, right?   
                            {
                                default:
                                case MandelbrotColoringAlgorithm.FastIterGray:
                                    {
                                        byte val = (byte)(iter * 255 / options.Iterations);
                                        red = grn = blu = val;
                                        break;
                                    }
                                case MandelbrotColoringAlgorithm.FastIterPalette:
                                    {
                                        int col = iter % options.Palette.Length;
                                        Color color = options.Palette[col];
                                        red = color.R;
                                        grn = color.G;
                                        blu = color.B;
                                        break;
                                    }
                                case MandelbrotColoringAlgorithm.SmoothIterPalette:
                                    {
                                        double smooth = iter + 1 + ln2lnbailoverln2 - Math.Log(Math.Log(distsqr)) / ln2;
                                        double p2 = smooth % 1;
                                        double p1 = 1 - p2;
                                        int min = (int)smooth % options.Palette.Length + options.Palette.Length; // Make sure it's positive
                                        int max = min + 1;
                                        Color c1 = options.Palette[min % options.Palette.Length]; // Linear interpolation
                                        Color c2 = options.Palette[max % options.Palette.Length];
                                        red = (byte)(c1.R * p1 + c2.R * p2);
                                        grn = (byte)(c1.G * p1 + c2.G * p2);
                                        blu = (byte)(c1.B * p1 + c2.B * p2);
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            red = grn = blu = 0;
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


        private MandelbrotTaskOptions CreatePreviewOptions(Size size)
        {
            MandelbrotTaskOptions opt = CreateRenderOptions(size); // Creates options based on the settings control

            if (opt.Coloring == MandelbrotColoringAlgorithm.SmoothIterPalette)
                opt.Coloring = MandelbrotColoringAlgorithm.FastIterPalette;

            opt.AntiAliasingScale = new Size(1, 1);
            opt.Iterations = (int)Math.Pow(settingsControl.Iterations, 2.0 / 3.0);
            opt.MultiThreaded = false;

            return opt;
        }

        private MandelbrotTaskOptions CreateRenderOptions(Size size)
        {
            MandelbrotTaskOptions opt = new MandelbrotTaskOptions();
            opt.Size = size;
            opt.Min = settingsControl.Min;
            opt.Max = settingsControl.Max;
            opt.Updates = settingsControl.Updates;
            opt.Palette = settingsControl.Palette;
            opt.Coloring = settingsControl.Coloring;
            opt.Iterations = settingsControl.Iterations;
            opt.BulbChecking = settingsControl.BulbChecking;
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
            settingsControl.Min = min;
            settingsControl.Max = max;
        }

        public override void GetClip(out Complex min, out Complex max)
        {
            min = settingsControl.Min;
            max = settingsControl.Max;
        }
    }
}
