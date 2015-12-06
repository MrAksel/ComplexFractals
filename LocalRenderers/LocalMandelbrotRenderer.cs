using FractalRenderer;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

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

            // TODO Supersampling, bulb checking, fixing smooth iter count, threading
            int rowsDone = 0;
            Action<Tuple<int, int, int, int>> worker = new Action<Tuple<int, int, int, int>>((offset) =>
            {
                int xoff = offset.Item1;
                int xjmp = offset.Item2;
                int yoff = offset.Item3;
                int yjmp = offset.Item4;

                int bailout = short.MaxValue;
                int bailsqr = bailout * bailout;
                double lnbail = Math.Log(bailout);

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

                        int iter = 0;
                        while (iter < options.Iterations && (rr + ii) < bailsqr)
                        {
                            i = r * i;
                            i = i + i + ci; // 2 * r * i + ci
                            r = rr - ii + cr;
                            //tmp = rr - ii + cr;
                            //i = 2 * r * i + ci;
                            //r = tpr;

                            rr = r * r;
                            ii = i * i;
                            iter++;
                        }
                        byte red, grn, blu;
                        if (iter < options.Iterations)
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
                                        double smooth = iter + 1 + Math.Log(lnbail / Math.Log(Math.Sqrt(rr + ii))) / ln2;
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
                                        double smooth = iter + 1 + Math.Log(lnbail / Math.Log(Math.Sqrt(rr + ii))) / ln2;
                                        double p2 = smooth % 1;
                                        double p1 = 1 - p2;
                                        int min = (int)smooth % options.Palette.Length + options.Palette.Length;
                                        int max = (min + 1) % options.Palette.Length + options.Palette.Length;
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

                    int rows = Interlocked.Increment(ref rowsDone);
                    if (options.TaskProgress != null)
                        options.TaskProgress(tasknum, options.User, rows / (float)bd.Height);
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
            opt.Palette = settingsControl.Palette;
            opt.Coloring = settingsControl.Coloring;
            opt.Iterations = settingsControl.Iterations;
            opt.SuperSampling = settingsControl.Sampling;
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

            string msg = string.Join("\n", tuples.Select(t => string.Format("{0}+{1} {2}+{3}", t.Item1, t.Item2, t.Item3, t.Item4)).ToArray());
            System.Diagnostics.Debug.WriteLine(msg);
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
    }
}
