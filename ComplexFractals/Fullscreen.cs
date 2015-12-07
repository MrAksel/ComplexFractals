using FractalRenderer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComplexFractals
{
    public partial class Fullscreen : Form
    {
        AbstractRenderer fractalRenderer;
        Stack<Tuple<Complex, Complex>> zooms;
        Point pMouseDown;
        Point pMousePos;

        int currentTask;

        public Fullscreen(AbstractRenderer renderer, Stack<Tuple<Complex, Complex>> zoomhistory)
        {
            fractalRenderer = renderer;
            zooms = zoomhistory;
            InitializeComponent();

            Location = Screen.PrimaryScreen.Bounds.Location;
            Size = Screen.PrimaryScreen.Bounds.Size;

            RedrawFractal();
        }

        private void SetStatus(string fmt, params object[] args)
        {

        }

        private void RedrawFractal()
        {
            if (fractalRenderer == null)
            {
                SetStatus("Renderer not selected yet");
                return;
            }

            if (currentTask != 0)
            {
                SetStatus("Aborting current task");
                fractalRenderer.AbortRender(currentTask);
            }

            currentTask = fractalRenderer.StartRenderAsync(pbFractal.Size, renderComplete, renderProgress, renderAborted);
            if (currentTask != 0)
            {
                SetStatus("Started rendering fractal (task {0})", currentTask);
            }
            else
            {
                SetStatus("Task not started");
            }
        }

        private void renderProgress(int task, object user, float percentage)
        {
            if (currentTask == task)
            {
                Invoke(new Action(() =>
                {
                    SetStatus("Rendering... {0:P1}", percentage);
                }));
            }
        }

        private void renderComplete(int task, object user, Bitmap result)
        {
            if (currentTask == task)
            {
                Invoke(new Action(() =>
                {
                    pbFractal.Image = result;
                    SetStatus("Completed render");
                }));
            }
        }

        private void renderAborted(int task, object user)
        {
            if (currentTask == task)
            {
                currentTask = 0;
                Invoke(new Action(() =>
                {
                    SetStatus("Aborted task");
                }));
            }
        }


        private void pbFractal_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && fractalRenderer != null && fractalRenderer.SupportsZooming())
            {
                pMouseDown = pbFractal.PointToClient(MousePosition);
            }
        }

        private void pbFractal_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && pMouseDown.X > 0 && fractalRenderer != null && fractalRenderer.SupportsZooming())
            {
                pMousePos = pbFractal.PointToClient(MousePosition);
                pbFractal.Invalidate();
            }
        }

        private void pbFractal_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && pMouseDown.X > 0 && fractalRenderer != null && fractalRenderer.SupportsZooming())
            {
                pMousePos = pbFractal.PointToClient(MousePosition);
                int l = Math.Min(pMouseDown.X, pMousePos.X);
                int t = Math.Min(pMouseDown.Y, pMousePos.Y);
                int r = Math.Max(pMouseDown.X, pMousePos.X);
                int b = Math.Max(pMouseDown.Y, pMousePos.Y);

                Complex min = fractalRenderer.PointToComplex(new Point(l, t), pbFractal.Size);
                Complex max = fractalRenderer.PointToComplex(new Point(r, b), pbFractal.Size);

                pMouseDown = pMousePos = new Point(-1, -1);

                zooms.Push(new Tuple<Complex, Complex>(min, max));
                fractalRenderer.SetClip(min, max);
                RedrawFractal();
            }
            else if (e.Button == MouseButtons.Right && fractalRenderer != null && fractalRenderer.SupportsZooming())
            {
                if (zooms.Count > 1)
                {
                    zooms.Pop();
                    Tuple<Complex, Complex> zoom = zooms.Peek();
                    fractalRenderer.SetClip(zoom.Item1, zoom.Item2);
                    RedrawFractal();
                }
            }
        }

        private void pbFractal_Paint(object sender, PaintEventArgs e)
        {
            if (pMouseDown.X >= 0)
            {
                int l = Math.Min(pMouseDown.X, pMousePos.X);
                int t = Math.Min(pMouseDown.Y, pMousePos.Y);
                int r = Math.Max(pMouseDown.X, pMousePos.X);
                int b = Math.Max(pMouseDown.Y, pMousePos.Y);
                Rectangle zoomed = Rectangle.FromLTRB(l, t, r, b);

                e.Graphics.DrawRectangle(Pens.Gold, zoomed);
            }
        }
    }
}
