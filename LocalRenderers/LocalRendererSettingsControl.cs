using FractalRenderer;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Diagnostics;

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

            cbColoring.SelectedIndex = 3;
            cbSampling.SelectedIndex = 0;

            texts = new Dictionary<TextBox, string>();
            texts.Add(textBox1, "");
            texts.Add(textBox2, "");
            texts.Add(textBox3, "");
            texts.Add(textBox4, "");

            // 13.5 sec
            textBox1.Text = (-2.5).ToString();
            textBox2.Text = (-1.2).ToString();
            textBox3.Text = (+1.5).ToString();
            textBox4.Text = (+1.2).ToString();
            
            /*
            textBox1.Text = (-5).ToString();
            textBox2.Text = (-5).ToString();
            textBox3.Text = (+5).ToString();
            textBox4.Text = (+5).ToString();
            */

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

        public bool BulbChecking
        {
            get
            {
                return cbBulbs.Checked;
            }
        }


        private void LoadPalette()
        {
            List<Color> colors = new List<Color>();
            try
            {
                using (FileStream fs = new FileStream("palette.txt", FileMode.Open))
                using (StreamReader sr = new StreamReader(fs))
                {
                    string line = "";
                    while (!sr.EndOfStream && (line = sr.ReadLine()) != "")
                    {
                        string[] cols = line.Split(' ');
                        if (cols.Length != 3)
                            continue;
                        byte r, g, b;
                        if (!byte.TryParse(cols[0], out r)) continue;
                        if (!byte.TryParse(cols[1], out g)) continue;
                        if (!byte.TryParse(cols[2], out b)) continue;

                        colors.Add(Color.FromArgb(r, g, b));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to load palette file. " + ex.Message);
            }
            if (colors.Count == 0)
            {
                colors.Add(Color.DarkBlue);
                colors.Add(Color.Blue);
                colors.Add(Color.LightBlue);
                colors.Add(Color.White);
                colors.Add(Color.Gold);
                colors.Add(Color.SaddleBrown);
                colors.Add(Color.Black);
            }
            foreach (Color c in colors)
            {
                AddControl(c);
            }
        }

        private void SavePalette()
        {
            try
            {
                using (FileStream fs = new FileStream("palette.txt", FileMode.Create))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    foreach (Color c in Palette)
                    {
                        sw.WriteLine("{0} {1} {2}", c.R, c.G, c.B);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save palette. " + ex.Message);
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
            AddControl(Color.White);
            if (Coloring == ColoringAlgorithm.FastIterPalette || Coloring == ColoringAlgorithm.SmoothIterPalette)
            {
                OnSettingsChanged(true);
            }
        }

        private void AddControl(Color c)
        {
            ColorPicker np = new ColorPicker();
            np.OnRemoval += Np_OnRemoval;
            np.OnColorChanged += PaletteChange;
            np.Width = flpPalette.Width - 8 - SystemInformation.VerticalScrollBarWidth;
            np.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            np.BackColor = c;
            flpPalette.Controls.Add(np);
        }

        private void PaletteChange(ColorPicker picker)
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

        private void flpPalette_SizeChanged(object sender, EventArgs e)
        {
            foreach (Control cp in flpPalette.Controls)
            {
                cp.Width = flpPalette.Width - 8 - SystemInformation.VerticalScrollBarWidth;
            }
        }
    }
}
