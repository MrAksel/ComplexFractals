using FractalRenderer;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Threading;

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

            WaitCallback task = new WaitCallback(delegate
            {
                TaskOptions taskopt = options.Clone();
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

            // TODO Supersampling, bulb checking, fixing smooth iter count
            for (int y = 0; y < bd.Height; y++)
            {
                if (options.TaskProgress != null)
                    options.TaskProgress(tasknum, options.User, y / (float)bd.Height);

                double mappedi = (double)y / bd.Height * (options.ImagMax - options.ImagMin) + options.ImagMin;
                double ci = mappedi;

                byte* row = start + bd.Stride * y;
                byte* px = row;
                for (int x = 0; x < bd.Width; x++)
                {
                    double mappedr = (double)x / bd.Width * (options.RealMax - options.RealMin) + options.RealMin;

                    double cr = mappedr;
                    double tpr = 0; // Temporary r to store result in loop
                    double r = cr;
                    double i = ci;
                    double rr = r * r;
                    double ii = i * i;

                    int iter = 0;
                    while (iter < options.Iterations && (rr + ii) < 4)
                    {
                        tpr = rr - ii + cr;
                        i = 2 * r * i + ci;
                        r = tpr;

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
                                    double p1 = Math.Log(Math.Log(rr + ii)) / ln2; // Smooth iteration algorithm from wikipedia, simplified by WolframAlpha
                                    double p2 = 1 - p1;
                                    int min = iter;
                                    int max = iter + 1;
                                    byte val = (byte)(iter * 255 * p1 + max * 255 * p2);
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
                                    double p1 = Math.Log(Math.Log(rr + ii)) / ln2; // Smooth iteration algorithm from wikipedia, simplified by WolframAlpha
                                    double p2 = 1 - p1;
                                    int min = iter;
                                    int max = iter + 1;
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
                    px += pxsize;
                }
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

            return opt;
        }

        private TaskOptions CreateRenderOptions(Size size)
        {
            TaskOptions opt = new TaskOptions();
            opt.Coloring = settingsControl.Coloring;
            opt.Iterations = settingsControl.Iterations;
            opt.SuperSampling = settingsControl.Sampling;
            opt.Palette = settingsControl.Palette;
            opt.Min = settingsControl.Min;
            opt.Max = settingsControl.Max;
            opt.Size = size;
            return opt;
        }

    }
}
