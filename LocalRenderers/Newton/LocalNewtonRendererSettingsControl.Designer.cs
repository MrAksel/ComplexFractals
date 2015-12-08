namespace LocalRenderers
{
    partial class LocalNewtonRendererSettingsControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.nmrIter = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbColoring = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.cbThreads = new System.Windows.Forms.CheckBox();
            this.nmrUpdates = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.tbssW = new System.Windows.Forms.TextBox();
            this.tbssH = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tbFunc = new System.Windows.Forms.TextBox();
            this.tbDFunc = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.nmrTolerance = new System.Windows.Forms.NumericUpDown();
            this.tbMulReal = new System.Windows.Forms.TextBox();
            this.tbMulImag = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nmrIter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrUpdates)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrTolerance)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Iterations:";
            // 
            // nmrIter
            // 
            this.nmrIter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nmrIter.Location = new System.Drawing.Point(62, 3);
            this.nmrIter.Name = "nmrIter";
            this.nmrIter.Size = new System.Drawing.Size(112, 20);
            this.nmrIter.TabIndex = 1;
            this.nmrIter.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nmrIter.ValueChanged += new System.EventHandler(this.other_changed);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Coloring:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Supersampling:";
            // 
            // cbColoring
            // 
            this.cbColoring.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbColoring.FormattingEnabled = true;
            this.cbColoring.Location = new System.Drawing.Point(62, 55);
            this.cbColoring.Name = "cbColoring";
            this.cbColoring.Size = new System.Drawing.Size(112, 21);
            this.cbColoring.TabIndex = 4;
            this.cbColoring.SelectedIndexChanged += new System.EventHandler(this.other_changed);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Bottom left:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 164);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Top right:";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(74, 109);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 8;
            this.textBox1.TextChanged += new System.EventHandler(this.tb_TextChanged);
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.Location = new System.Drawing.Point(74, 135);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 9;
            this.textBox2.TextChanged += new System.EventHandler(this.tb_TextChanged);
            // 
            // textBox3
            // 
            this.textBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox3.Location = new System.Drawing.Point(74, 161);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 20);
            this.textBox3.TabIndex = 11;
            this.textBox3.TextChanged += new System.EventHandler(this.tb_TextChanged);
            // 
            // textBox4
            // 
            this.textBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox4.Location = new System.Drawing.Point(74, 187);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(100, 20);
            this.textBox4.TabIndex = 10;
            this.textBox4.TextChanged += new System.EventHandler(this.tb_TextChanged);
            // 
            // cbThreads
            // 
            this.cbThreads.AutoSize = true;
            this.cbThreads.Checked = true;
            this.cbThreads.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbThreads.Location = new System.Drawing.Point(6, 213);
            this.cbThreads.Name = "cbThreads";
            this.cbThreads.Size = new System.Drawing.Size(100, 17);
            this.cbThreads.TabIndex = 0;
            this.cbThreads.Text = "Multiple threads";
            this.cbThreads.UseVisualStyleBackColor = true;
            // 
            // nmrUpdates
            // 
            this.nmrUpdates.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nmrUpdates.Location = new System.Drawing.Point(104, 29);
            this.nmrUpdates.Name = "nmrUpdates";
            this.nmrUpdates.Size = new System.Drawing.Size(70, 20);
            this.nmrUpdates.TabIndex = 15;
            this.nmrUpdates.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Update frequency:";
            // 
            // tbssW
            // 
            this.tbssW.Location = new System.Drawing.Point(83, 82);
            this.tbssW.Name = "tbssW";
            this.tbssW.Size = new System.Drawing.Size(33, 20);
            this.tbssW.TabIndex = 16;
            this.tbssW.TextChanged += new System.EventHandler(this.tbssH_TextChanged);
            // 
            // tbssH
            // 
            this.tbssH.Location = new System.Drawing.Point(140, 82);
            this.tbssH.Name = "tbssH";
            this.tbssH.Size = new System.Drawing.Size(33, 20);
            this.tbssH.TabIndex = 17;
            this.tbssH.TextChanged += new System.EventHandler(this.tbssH_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(122, 85);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(12, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "x";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 290);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(24, 13);
            this.label8.TabIndex = 19;
            this.label8.Text = "F(z)";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 348);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(26, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "F\'(z)";
            // 
            // tbFunc
            // 
            this.tbFunc.Location = new System.Drawing.Point(7, 306);
            this.tbFunc.Multiline = true;
            this.tbFunc.Name = "tbFunc";
            this.tbFunc.Size = new System.Drawing.Size(167, 39);
            this.tbFunc.TabIndex = 21;
            // 
            // tbDFunc
            // 
            this.tbDFunc.Location = new System.Drawing.Point(7, 364);
            this.tbDFunc.Multiline = true;
            this.tbDFunc.Name = "tbDFunc";
            this.tbDFunc.Size = new System.Drawing.Size(167, 39);
            this.tbDFunc.TabIndex = 22;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 268);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 13);
            this.label10.TabIndex = 23;
            this.label10.Text = "Tolerance:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 242);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(51, 13);
            this.label11.TabIndex = 24;
            this.label11.Text = "Multiplier:";
            // 
            // nmrTolerance
            // 
            this.nmrTolerance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nmrTolerance.Location = new System.Drawing.Point(73, 266);
            this.nmrTolerance.Name = "nmrTolerance";
            this.nmrTolerance.Size = new System.Drawing.Size(100, 20);
            this.nmrTolerance.TabIndex = 25;
            // 
            // tbMulReal
            // 
            this.tbMulReal.Location = new System.Drawing.Point(73, 239);
            this.tbMulReal.Name = "tbMulReal";
            this.tbMulReal.Size = new System.Drawing.Size(42, 20);
            this.tbMulReal.TabIndex = 26;
            // 
            // tbMulImag
            // 
            this.tbMulImag.Location = new System.Drawing.Point(128, 239);
            this.tbMulImag.Name = "tbMulImag";
            this.tbMulImag.Size = new System.Drawing.Size(37, 20);
            this.tbMulImag.TabIndex = 27;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(115, 242);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(13, 13);
            this.label12.TabIndex = 28;
            this.label12.Text = "+";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(165, 242);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(9, 13);
            this.label13.TabIndex = 29;
            this.label13.Text = "i";
            // 
            // LocalNewtonRendererSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.tbMulImag);
            this.Controls.Add(this.tbMulReal);
            this.Controls.Add(this.nmrTolerance);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.tbDFunc);
            this.Controls.Add(this.tbFunc);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbssH);
            this.Controls.Add(this.tbssW);
            this.Controls.Add(this.nmrUpdates);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cbThreads);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbColoring);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nmrIter);
            this.Controls.Add(this.label1);
            this.Name = "LocalNewtonRendererSettingsControl";
            this.Size = new System.Drawing.Size(180, 411);
            ((System.ComponentModel.ISupportInitialize)(this.nmrIter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrUpdates)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrTolerance)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nmrIter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbColoring;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.CheckBox cbThreads;
        private System.Windows.Forms.NumericUpDown nmrUpdates;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbssW;
        private System.Windows.Forms.TextBox tbssH;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbFunc;
        private System.Windows.Forms.TextBox tbDFunc;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown nmrTolerance;
        private System.Windows.Forms.TextBox tbMulReal;
        private System.Windows.Forms.TextBox tbMulImag;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
    }
}
