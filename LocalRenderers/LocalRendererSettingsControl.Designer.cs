namespace LocalRenderers
{
    partial class LocalRendererSettingsControl
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
            SavePalette();
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
            this.cbSampling = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.flpPalette = new System.Windows.Forms.FlowLayoutPanel();
            this.cbThreads = new System.Windows.Forms.CheckBox();
            this.cbBulbs = new System.Windows.Forms.CheckBox();
            this.nmrUpdates = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nmrIter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrUpdates)).BeginInit();
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
            // cbSampling
            // 
            this.cbSampling.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbSampling.FormattingEnabled = true;
            this.cbSampling.Location = new System.Drawing.Point(82, 82);
            this.cbSampling.Name = "cbSampling";
            this.cbSampling.Size = new System.Drawing.Size(92, 21);
            this.cbSampling.TabIndex = 5;
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
            // flpPalette
            // 
            this.flpPalette.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpPalette.AutoScroll = true;
            this.flpPalette.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpPalette.Location = new System.Drawing.Point(3, 259);
            this.flpPalette.Name = "flpPalette";
            this.flpPalette.Size = new System.Drawing.Size(171, 167);
            this.flpPalette.TabIndex = 12;
            this.flpPalette.SizeChanged += new System.EventHandler(this.flpPalette_SizeChanged);
            this.flpPalette.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.flpPalette_MouseDoubleClick);
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
            // cbBulbs
            // 
            this.cbBulbs.AutoSize = true;
            this.cbBulbs.Checked = true;
            this.cbBulbs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbBulbs.Location = new System.Drawing.Point(6, 236);
            this.cbBulbs.Name = "cbBulbs";
            this.cbBulbs.Size = new System.Drawing.Size(121, 17);
            this.cbBulbs.TabIndex = 13;
            this.cbBulbs.Text = "Check primary bulbs";
            this.cbBulbs.UseVisualStyleBackColor = true;
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
            // LocalRendererSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.nmrUpdates);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cbBulbs);
            this.Controls.Add(this.cbThreads);
            this.Controls.Add(this.flpPalette);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbSampling);
            this.Controls.Add(this.cbColoring);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nmrIter);
            this.Controls.Add(this.label1);
            this.Name = "LocalRendererSettingsControl";
            this.Size = new System.Drawing.Size(180, 429);
            ((System.ComponentModel.ISupportInitialize)(this.nmrIter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrUpdates)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nmrIter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbColoring;
        private System.Windows.Forms.ComboBox cbSampling;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.FlowLayoutPanel flpPalette;
        private System.Windows.Forms.CheckBox cbThreads;
        private System.Windows.Forms.CheckBox cbBulbs;
        private System.Windows.Forms.NumericUpDown nmrUpdates;
        private System.Windows.Forms.Label label6;
    }
}
