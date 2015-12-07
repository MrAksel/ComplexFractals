using System.Drawing;
using FractalRenderer;
using System.Numerics;
using System;

namespace LocalRenderers.Newton
{
    public class NewtonTaskOptions
    {
        public Size Size { get; set; }
        public int Updates { get; set; }
        public object User { get; set; }
        public int Iterations { get; set; }
        public bool MultiThreaded { get; set; }
        public Size AntiAliasingScale { get; set; }
        public ColoringAlgorithm Coloring { get; set; }
        public AbstractRenderer.RenderAborted TaskAborted { get; set; }
        public AbstractRenderer.RenderComplete TaskComplete { get; set; }
        public AbstractRenderer.RenderProgress TaskProgress { get; set; }

        public Color[] Palette { get; set; }

        public string Function { get; set; }
        public string Derivative { get; set; }
        public double Tolerance { get; set; }
        public Complex Multiplier { get; set; }

        public Complex Min { get; set; }
        public Complex Max { get; set; }
        public double RealMin { get { return Min.Real; } }
        public double RealMax { get { return Max.Real; } }
        public double ImagMin { get { return Min.Imaginary; } }
        public double ImagMax { get { return Max.Imaginary; } }

        public Size ActualRenderSize
        {
            get
            {
                return new Size(Size.Width * AntiAliasingScale.Width,
                                Size.Height * AntiAliasingScale.Height);
            }
        }

        public int Area
        {
            get
            {
                return Size.Width * Size.Height;
            }
        }

        public NewtonTaskOptions Clone()
        {
            NewtonTaskOptions opt = new NewtonTaskOptions();
            opt.Iterations = Iterations;
            opt.Updates = Updates;
            opt.Size = Size;
            opt.Coloring = Coloring;
            opt.Function = Function;
            opt.Tolerance = Tolerance;
            opt.Derivative = Derivative;
            opt.Multiplier = Multiplier;
            opt.TaskAborted = TaskAborted;
            opt.TaskComplete = TaskComplete;
            opt.TaskProgress = TaskProgress;
            opt.MultiThreaded = MultiThreaded;
            opt.AntiAliasingScale = AntiAliasingScale;
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