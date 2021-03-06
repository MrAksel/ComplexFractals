﻿using System.Drawing;
using System.Numerics;
using System.Threading;
using System.Windows.Forms;

namespace FractalRenderer
{

    public abstract class AbstractRenderer
    {
        public abstract SettingsWindow GetSettingsWindow();

        public abstract bool SupportsZooming();
        public abstract void SetClip(Complex min, Complex max);
        public abstract void GetClip(out Complex min, out Complex max);

        public virtual Complex PointToComplex(Point onPlane, Size planeSize)
        {
            Complex min, max;
            GetClip(out min, out max);
            double cx = onPlane.X / (double)planeSize.Width * (max.Real - min.Real) + min.Real;
            double cy = onPlane.Y / (double)planeSize.Height * (max.Imaginary - min.Imaginary) + min.Imaginary;

            return new Complex(cx, cy);
        }
        public virtual Complex PointToRealPlane(Complex onPlane, Size planeSize)
        {
            Complex min, max;
            GetClip(out min, out max);
            double x = (onPlane.Real - min.Real) / (max.Real - min.Real) * planeSize.Width;
            double y = (onPlane.Imaginary - min.Imaginary) / (max.Imaginary - min.Imaginary) * planeSize.Height;

            return new Complex(x, y);
        }

        public abstract Bitmap DrawPreview(Size size);
        public abstract Bitmap DrawFractal(Size size);

        public virtual int StartRenderAsync(Size size, RenderComplete completeCallback)
        {
            return StartRenderAsync(size, completeCallback, null, null, null);
        }
        public virtual int StartRenderAsync(Size size, RenderComplete completeCallback, RenderProgress progressCallback)
        {
            return StartRenderAsync(size, completeCallback, progressCallback, null, null);
        }
        public virtual int StartRenderAsync(Size size, RenderComplete completeCallback, RenderProgress progressCallback, RenderAborted abortedCallback)
        {
            return StartRenderAsync(size, completeCallback, progressCallback, abortedCallback, null);
        }

        public abstract int StartRenderAsync(Size size, RenderComplete completeCallback, RenderProgress progressCallback, RenderAborted abortedCallback, object user);
        public abstract void AbortRender(int task);


        public delegate void RenderProgress(int task, object user, float percentage);
        public delegate void RenderComplete(int task, object user, Bitmap result);
        public delegate void RenderAborted(int task, object user);


        private int currTaskNum = 1;
        private object tasknumLock = new object();
        protected int GetTaskNumber()
        {
            lock (tasknumLock)
            {
                int val = currTaskNum;

                currTaskNum++;
                if (currTaskNum == 0) // Skip 0
                    currTaskNum++;

                return val;
            }
        }
    }
}
