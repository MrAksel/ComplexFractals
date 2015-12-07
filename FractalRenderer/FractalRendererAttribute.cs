using System;

namespace FractalRenderer
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class FractalRendererAttribute : Attribute
    {
        readonly bool enabled;
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
        public bool Enabled
        {
            get { return enabled; }
        }

        public FractalRendererAttribute(string fractalName, string rendererName)
            :this(fractalName, rendererName, true)
        {
        }

        public FractalRendererAttribute(string fractalName, string rendererName, bool enabled)
        {
            this.enabled = enabled;
            this.fractal = fractalName;
            this.renderer = rendererName;
        }
    }
}
