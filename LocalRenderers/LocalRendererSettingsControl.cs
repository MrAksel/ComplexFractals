using FractalRenderer;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;
using System.Windows.Forms;
using System.Drawing;

namespace LocalRenderers
{
    public partial class LocalRendererSettingsControl : SettingsWindow
    {
        private Dictionary<TextBox, string> texts;

        public LocalRendererSettingsControl()
        {
            InitializeComponent();

            nmrIter.Maximum = int.MaxValue;

            cbColoring.Items.AddRange(Enum.GetNames(typeof(ColoringAlgorithm)));

            cbSampling.Items.Add(SuperSampling.OneByOne);
            cbSampling.Items.Add(SuperSampling.ThreeByThree);
            cbSampling.Items.Add(SuperSampling.ThreeByThreeCross);
            cbSampling.Items.Add(SuperSampling.FiveByFive);
            cbSampling.Items.Add(SuperSampling.FiveByFiveCross);

            cbColoring.SelectedIndex = 0;
            cbSampling.SelectedIndex = 0;

            texts = new Dictionary<TextBox, string>();
            texts.Add(textBox1, "");
            texts.Add(textBox2, "");
            texts.Add(textBox3, "");
            texts.Add(textBox4, "");

            textBox1.Text = (-2.5).ToString();
            textBox2.Text = (-1.2).ToString();
            textBox3.Text = (+1.5).ToString();
            textBox4.Text = (+1.2).ToString();

            LoadPalette();
        }

        public int Iterations
        {
            get
            {
                return (int)nmrIter.Value;
            }
        }

        public ColoringAlgorithm Coloring
        {
            get
            {
                ColoringAlgorithm res;
                if (Enum.TryParse(cbColoring.SelectedItem.ToString(), out res))
                    return res;
                else
                    return ColoringAlgorithm.FastIterPalette;
            }
        }

        public SuperSampling Sampling
        {
            get
            {
                switch (cbSampling.SelectedItem.ToString())
                {
                    case "One by one": return SuperSampling.OneByOne;
                    case "Three by three": return SuperSampling.ThreeByThree;
                    case "Three by three cross": return SuperSampling.ThreeByThreeCross;
                    case "Five by five": return SuperSampling.FiveByFive;
                    case "Five by five cross": return SuperSampling.FiveByFiveCross;
                    default: return SuperSampling.OneByOne;
                }
            }
        }

        public Complex Min
        {
            get
            {
                return new Complex(double.Parse(textBox1.Text), double.Parse(textBox2.Text));
            }
        }

        public Complex Max
        {
            get
            {
                return new Complex(double.Parse(textBox3.Text), double.Parse(textBox4.Text));
            }
        }

        public Color[] Palette
        {
            get
            {
                List<Color> colors = new List<Color>();
                foreach (ColorPicker cp in flpPalette.Controls)
                {
                    colors.Add(cp.BackColor);
                }
                return colors.ToArray();
            }
        }

        public bool Multithreaded
        {
            get
            {
                return cbThreads.Checked;
            }
        }

        private void LoadPalette()
        {
            Color[] colors = new Color[] { Color.Blue, Color.BlueViolet, Color.Indigo, Color.MediumVioletRed, Color.Red, Color.Orange, Color.Gold, Color.Yellow, Color.Green, Color.DarkSeaGreen };
            foreach (Color c in colors)
            {
                ColorPicker np = new ColorPicker() { BackColor = c};
                np.OnRemoval += Np_OnRemoval;
                np.BackColorChanged += PaletteChange;
                flpPalette.Controls.Add(np);
            }
        }

        private void tb_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb == null)
                return;

            double val;
            string text = tb.Text;
            bool paresable = double.TryParse(text, out val);

            if (!paresable)
            {
                tb.Text = texts[tb]; // Reset text
            }
            else
            {
                texts[tb] = tb.Text; // Update cache
                OnSettingsChanged(true);
            }
        }

        private void other_changed(object sender, EventArgs e)
        {
            OnSettingsChanged(true);
        }


        private void flpPalette_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ColorPicker np = new ColorPicker();
            np.OnRemoval += Np_OnRemoval;
            np.BackColorChanged += PaletteChange;
            flpPalette.Controls.Add(np);

            if (Coloring == ColoringAlgorithm.FastIterPalette || Coloring == ColoringAlgorithm.SmoothIterPalette)
            {
                OnSettingsChanged(true);
            }
        }

        private void PaletteChange(object sender, EventArgs e)
        {
            if (Coloring == ColoringAlgorithm.FastIterPalette || Coloring == ColoringAlgorithm.SmoothIterPalette)
            {
                OnSettingsChanged(true);
            }
        }

        private void Np_OnRemoval(ColorPicker picker)
        {
            flpPalette.Controls.Remove(picker);
            picker.Dispose();
        }
    }
}
