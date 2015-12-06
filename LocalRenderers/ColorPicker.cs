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
        }

        public event OnRemovalEvent OnRemoval;
        public delegate void OnRemovalEvent(ColorPicker picker);

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (OnRemoval != null)
                OnRemoval(this);
        }
    }
}
