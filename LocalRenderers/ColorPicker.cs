using System;
using System.Drawing;
using System.Windows.Forms;

namespace LocalRenderers
{
    public partial class ColorPicker : UserControl
    {
        public ColorPicker()
        {
            InitializeComponent();
        }

        private void ColorPicker_BackColorChanged(object sender, EventArgs e)
        {
            if (BackColor.GetBrightness() < 0.5f)
                btnClear.ForeColor = Color.White;
            else
                btnClear.ForeColor = Color.Black;

            if (OnColorChanged != null)
                OnColorChanged(this);
        }


        public event ColorPickerEvent OnRemoval;
        public event ColorPickerEvent OnColorChanged;
        public delegate void ColorPickerEvent(ColorPicker picker);

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (OnRemoval != null)
                OnRemoval(this);
        }

        private void ColorPicker_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (cd1.ShowDialog() == DialogResult.OK)
            {
                BackColor = cd1.Color;
            }
        }
    }
}
