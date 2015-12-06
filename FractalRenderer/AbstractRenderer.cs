using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace FractalRenderer
{

    public abstract class AbstractRenderer
    {
        public abstract SettingsWindow GetSettingsWindow();

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
        public delegate void RenderComplete(int task, int returnCode, object user, Bitmap result);
        public delegate void RenderAborted(int task, int returnCode, object user);


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
