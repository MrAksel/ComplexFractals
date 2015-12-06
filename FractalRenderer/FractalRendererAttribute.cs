using System;

namespace FractalRenderer
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class FractalRendererAttribute : Attribute
    {
        readonly string fractal;
        readonly string renderer;

        public string FractalName
        {
            get { return fractal; }
        }
        public string RendererName
        {
            get { return renderer; }
        }

        public FractalRendererAttribute(string fractalName, string rendererName)
        {
            this.fractal = fractalName;
            this.renderer = rendererName;
        }
    }
}
