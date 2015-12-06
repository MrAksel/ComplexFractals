using System.Drawing;
using FractalRenderer;
using System.Numerics;
using System;

namespace LocalRenderers
{
    public class TaskOptions
    {
        public int Updates { get; set; }
        public int Iterations { get; set; }
        public Size Size { get; set; }
        public ColoringAlgorithm Coloring { get; set; }
        public SuperSampling SuperSampling { get; set; }
        public AbstractRenderer.RenderAborted TaskAborted { get; set; }
        public AbstractRenderer.RenderComplete TaskComplete { get; set; }
        public AbstractRenderer.RenderProgress TaskProgress { get; set; }
        public object User { get; set; }
        public bool MultiThreaded { get; set; }
        public bool BulbChecking { get; set; }

        public Color[] Palette { get; set; }

        public Complex Min { get; set; }
        public Complex Max { get; set; }
        public double RealMin { get { return Min.Real; } }
        public double RealMax { get { return Max.Real; } }
        public double ImagMin { get { return Min.Imaginary; } }
        public double ImagMax { get { return Max.Imaginary; } }

        public int Area
        {
            get
            {
                return Size.Width * Size.Height;
            }
        }

        public TaskOptions Clone()
        {
            TaskOptions opt = new TaskOptions();
            opt.Iterations = Iterations;
            opt.Updates = Updates;
            opt.Size = Size;
            opt.Coloring = Coloring;
            opt.SuperSampling = SuperSampling;
            opt.TaskAborted = TaskAborted;
            opt.TaskComplete = TaskComplete;
            opt.TaskProgress = TaskProgress;
            opt.BulbChecking = BulbChecking;
            opt.MultiThreaded = MultiThreaded;
            opt.User = User;

            if (Palette != null)
            {
                opt.Palette = new Color[Palette.Length];
                Array.Copy(Palette, opt.Palette, Palette.Length);
            }

            opt.Min = Min;
            opt.Max = Max;

            return opt;
        }
    }
}