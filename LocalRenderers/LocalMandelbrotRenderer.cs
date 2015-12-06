using FractalRenderer;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Numerics;

namespace LocalRenderers
{
    [FractalRenderer("Mandelbrot", "Local renderer")]
    public class LocalMandelbrotRenderer : AbstractRenderer
    {
        private const double ln2 = 0.693147180559945309417232121458176568075500134360255254120680; // Didn't bother with truncating the digits..

        private SynchronizedCollection<int> activeTasks;
        private LocalRendererSettingsControl settingsControl;


        public LocalMandelbrotRenderer()
        {
            activeTasks = new SynchronizedCollection<int>();
            settingsControl = new LocalRendererSettingsControl();
        }


        public override SettingsWindow GetSettingsWindow()
        {
            return settingsControl;
        }

        public override void AbortRender(int task)
        {
            activeTasks.Remove(task);
        }

        public override Bitmap DrawPreview(Size size)
        {
            TaskOptions opt = CreatePreviewOptions(size);
            return DrawFractal(opt);
        }

        public override Bitmap DrawFractal(Size size)
        {
            TaskOptions opt = CreateRenderOptions(size);
            return DrawFractal(opt);
        }

        public Bitmap DrawFractal(TaskOptions opt)
        {
            int retcode = 0;
            Bitmap bmref = null;
            ManualResetEvent set = new ManualResetEvent(false);

            RenderComplete cb = new RenderComplete((tnum, ret, usr, bm) =>
            {
                retcode = ret;
                bmref = bm; // TODO Should maybe check whether tnum == task
                set.Set();
            });

            opt.TaskComplete = opt.TaskComplete + cb; // TODO cb or users delegate first?

            int task = StartRenderAsync(opt);
            set.WaitOne();

            // TODO Handle retcode

            return bmref;
        }

        public override int StartRenderAsync(Size size, RenderComplete completeCallback, RenderProgress progressCallback, RenderAborted abortedCallback, object user)
        {
            if (size.Width < 1 || size.Height < 1 || completeCallback == null) // No point in starting if the area is zero or noone gets notified when its completed
                return 0; // Zero means not started

            TaskOptions options = CreateRenderOptions(size);
            options.TaskComplete = completeCallback;
            options.TaskProgress = progressCallback;
            options.TaskAborted = abortedCallback;
            options.User = user;


            return StartRenderAsync(options);
        }

        public int StartRenderAsync(TaskOptions options)
        {
            if (options.Area == 0)
                return 0;

            int tasknum = GetTaskNumber();
            activeTasks.Add(tasknum);

            TaskOptions taskopt = options.Clone();
            WaitCallback task = new WaitCallback(delegate
            {
                RenderTaskAsync(taskopt, tasknum);
            });
            ThreadPool.QueueUserWorkItem(task);

            return tasknum;
        }


        private unsafe void RenderTaskAsync(TaskOptions options, int tasknum)
        {
            int retcode = 0;
            Bitmap res = new Bitmap(options.Size.Width, options.Size.Height, PixelFormat.Format24bppRgb);

            if (options.Iterations == 0)
                goto finished;

            BitmapData bd = res.LockBits(new Rectangle(Point.Empty, options.Size), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            byte* start = (byte*)bd.Scan0.ToPointer();
            int pxsize = 3;

            double rinc = (options.RealMax - options.RealMin) / bd.Width;
            double iinc = (options.ImagMax - options.ImagMin) / bd.Height;

            // TODO Supersampling, bulb checking
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

                int bailout = short.MaxValue;
                int bailsqr = bailout * bailout;
                double ln2lnbailoverln2 = 1 + Math.Log(Math.Log(bailout)) / ln2; // ln (2 * ln(bailout)) / ln2 = (ln2 + ln(ln(bailout))) / ln2 = 1 + ln(ln(bailout)) / ln2

                for (int y = yoff; y < bd.Height; y += yjmp)
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
                                case ColoringAlgorithm.FastIterGray:
                                    {
                                        byte val = (byte)(iter * 255 / options.Iterations);
                                        red = grn = blu = val;
                                        break;
                                    }
                                case ColoringAlgorithm.SmoothIterGray:
                                    {
                                        double smooth = iter + 1 + ln2lnbailoverln2 - Math.Log(Math.Log(distsqr)) / ln2;
                                        double p2 = smooth % 1;
                                        double p1 = 1 - p2;
                                        byte val = (byte)((iter * 255 * p1 + (iter + 1) * 255 * p2) / options.Iterations);
                                        red = grn = blu = val;
                                        break;
                                    }
                                case ColoringAlgorithm.FastIterPalette:
                                    {
                                        int col = iter % options.Palette.Length;
                                        Color color = options.Palette[col];
                                        red = color.R;
                                        grn = color.G;
                                        blu = color.B;
                                        break;
                                    }
                                case ColoringAlgorithm.SmoothIterPalette:
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

            if (options.TaskProgress != null)
                options.TaskProgress(tasknum, options.User, 1f);

            if (options.TaskComplete != null)
                options.TaskComplete(tasknum, retcode, options.User, res);
        }

        private TaskOptions CreatePreviewOptions(Size size)
        {
            TaskOptions opt = CreateRenderOptions(size); // Creates options based on the settings control

            if (opt.Coloring == ColoringAlgorithm.SmoothIterPalette)
                opt.Coloring = ColoringAlgorithm.FastIterPalette;
            if (opt.Coloring == ColoringAlgorithm.SmoothIterGray)
                opt.Coloring = ColoringAlgorithm.FastIterGray;

            opt.SuperSampling = SuperSampling.OneByOne;
            opt.Iterations = (int)Math.Pow(settingsControl.Iterations, 2.0 / 3.0);
            opt.MultiThreaded = false;

            return opt;
        }

        private TaskOptions CreateRenderOptions(Size size)
        {
            TaskOptions opt = new TaskOptions();
            opt.Size = size;
            opt.Min = settingsControl.Min;
            opt.Max = settingsControl.Max;
            opt.Updates = settingsControl.Updates;
            opt.Palette = settingsControl.Palette;
            opt.Coloring = settingsControl.Coloring;
            opt.Iterations = settingsControl.Iterations;
            opt.SuperSampling = settingsControl.Sampling;
            opt.BulbChecking = settingsControl.BulbChecking;
            opt.MultiThreaded = settingsControl.Multithreaded;
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

        public override Complex PointToComplex(Point onPlane, Size planeSize)
        {
            double cx = onPlane.X / (double)planeSize.Width * (settingsControl.Max.Real - settingsControl.Min.Real) + settingsControl.Min.Real;
            double cy = onPlane.Y / (double)planeSize.Height * (settingsControl.Max.Imaginary - settingsControl.Min.Imaginary) + settingsControl.Min.Imaginary;

            return new Complex(cx, cy);
        }

        public override Complex PointToRealPlane(Complex onPlane, Size planeSize)
        {
            double x = (onPlane.Real - settingsControl.Min.Real) / (settingsControl.Max.Real - settingsControl.Min.Real) * planeSize.Width;
            double y = (onPlane.Imaginary - settingsControl.Min.Imaginary) / (settingsControl.Max.Imaginary - settingsControl.Min.Imaginary) * planeSize.Height;

            return new Complex(x, y);
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
