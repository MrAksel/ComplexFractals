using FractalRenderer;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using LocalRenderers.Newton;

namespace LocalRenderers
{
    public partial class LocalNewtonRendererSettingsControl : SettingsWindow
    {
        public bool update;
        private Jace.CalculationEngine parser;
        private Dictionary<TextBox, string> texts;

        public LocalNewtonRendererSettingsControl()
        {
            InitializeComponent();

            parser = new Jace.CalculationEngine(System.Globalization.CultureInfo.InvariantCulture, Jace.Execution.ExecutionMode.Compiled, true, true);

            nmrIter.Maximum = int.MaxValue;
            nmrUpdates.Maximum = int.MaxValue;

            nmrTolerance.DecimalPlaces = 20;
            nmrTolerance.Minimum = (decimal)0.00001;
            nmrTolerance.Maximum = int.MaxValue;
            nmrTolerance.Increment = nmrTolerance.Minimum;

            cbColoring.Items.AddRange(Enum.GetNames(typeof(NewtonColoringAlgorithm)));

            cbColoring.SelectedIndex = 0;

            texts = new Dictionary<TextBox, string>();
            texts.Add(textBox1, "");
            texts.Add(textBox2, "");
            texts.Add(textBox3, "");
            texts.Add(textBox4, "");
            texts.Add(tbssW, "");
            texts.Add(tbssH, "");

            textBox1.Text = (-2.5).ToString();
            textBox2.Text = (-2.0).ToString();
            textBox3.Text = (+2.5).ToString();
            textBox4.Text = (+2.0).ToString();

            tbssW.Text = "1";
            tbssH.Text = "1";

            tbMulReal.Text = "1";
            tbMulImag.Text = "0";

            tbFunc.Text = "z^3-1";
            tbDFunc.Text = "3*z^2";

            update = true;
        }


        public int Iterations
        {
            get
            {
                return (int)nmrIter.Value;
            }
        }

        public int Updates
        {
            get
            {
                return (int)nmrUpdates.Value;
            }
        }

        public NewtonColoringAlgorithm Coloring
        {
            get
            {
                NewtonColoringAlgorithm res;
                if (Enum.TryParse(cbColoring.SelectedItem.ToString(), out res))
                    return res;
                else
                    return NewtonColoringAlgorithm.Smooth;
            }
        }

        public Size FullSizeAAScaler
        {
            get
            {
                return new Size(int.Parse(tbssW.Text), int.Parse(tbssH.Text));
            }
        }

        public Complex Min
        {
            get
            {
                return new Complex(double.Parse(textBox1.Text), double.Parse(textBox2.Text));
            }
            set
            {
                textBox1.Text = value.Real.ToString();
                textBox2.Text = value.Imaginary.ToString();
            }
        }

        public Complex Max
        {
            get
            {
                return new Complex(double.Parse(textBox3.Text), double.Parse(textBox4.Text));
            }
            set
            {
                textBox3.Text = value.Real.ToString();
                textBox4.Text = value.Imaginary.ToString();
            }
        }

        public bool Multithreaded
        {
            get
            {
                return cbThreads.Checked;
            }
        }

        public Func<Complex, Complex, Complex> Function
        {
            get
            {
                try
                {
                    Complex i = Complex.ImaginaryOne;
                    Func<Dictionary<string, Complex>, Complex> func = parser.Build(tbFunc.Text);
                    return (z, c) =>
                    {
                        Dictionary<string, Complex> vars = new Dictionary<string, Complex>();
                        vars.Add("z", z);
                        vars.Add("c", c);
                        return func(vars);
                    };
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid function for f(z). " + ex.Message);
                    return (z, c) => z * z;
                }
            }
        }
        public Func<Complex, Complex, Complex> Derivative
        {
            get
            {
                Complex i = Complex.ImaginaryOne;
                try
                {
                    Func<Dictionary<string, Complex>, Complex> func = parser.Build(tbDFunc.Text);
                    return (z, c) =>
                    {
                        Dictionary<string, Complex> vars = new Dictionary<string, Complex>();
                        vars.Add("z", z);
                        vars.Add("c", c);
                        return func(vars);
                    };
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid function for derivative. " + ex.Message);
                    return (z, c) => 2 * z;
                }
            }
        }

        public double Tolerance
        {
            get
            {
                return (double)nmrTolerance.Value;
            }
        }
        public Complex Multiplier
        {
            get
            {
                double r = 1.0;
                double i = 0.0;
                double.TryParse(tbMulReal.Text, out r);
                double.TryParse(tbMulImag.Text, out i);
                return new Complex(r, i);
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
                OnSettingsChanged(update);
            }
        }

        private void other_changed(object sender, EventArgs e)
        {
            OnSettingsChanged(update);
        }


        private void tbssH_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb == null)
                return;

            uint val;
            string text = tb.Text;
            bool paresable = uint.TryParse(text, out val);

            if (!paresable)
            {
                tb.Text = texts[tb]; // Reset text
            }
            else
            {
                texts[tb] = tb.Text; // Update cache
                OnSettingsChanged(update);
            }
        }
    }
}
