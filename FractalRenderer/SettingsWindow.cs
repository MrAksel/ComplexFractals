using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FractalRenderer
{
    public class SettingsWindow : UserControl
    {
        public delegate void SettingsChangedEventHandler(SettingsWindow sender, bool redraw);
        public event SettingsChangedEventHandler SettingsChanged;

        protected void OnSettingsChanged(bool redraw)
        {
            if (SettingsChanged != null)
                SettingsChanged(this, redraw);
        }
    }
}
