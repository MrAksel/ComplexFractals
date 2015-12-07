namespace ComplexFractals
{
    partial class Fractals
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbSettings = new System.Windows.Forms.GroupBox();
            this.btnRedraw = new System.Windows.Forms.Button();
            this.pbPreview = new System.Windows.Forms.PictureBox();
            this.cbFractal = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbRenderer = new System.Windows.Forms.ComboBox();
            this.pbFractal = new System.Windows.Forms.PictureBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.fsWatch = new System.IO.FileSystemWatcher();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFractal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fsWatch)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbSettings
            // 
            this.gbSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gbSettings.Location = new System.Drawing.Point(13, 231);
            this.gbSettings.Name = "gbSettings";
            this.gbSettings.Size = new System.Drawing.Size(265, 481);
            this.gbSettings.TabIndex = 0;
            this.gbSettings.TabStop = false;
            this.gbSettings.Text = "Settings";
            // 
            // btnRedraw
            // 
            this.btnRedraw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRedraw.Location = new System.Drawing.Point(748, 13);
            this.btnRedraw.Name = "btnRedraw";
            this.btnRedraw.Size = new System.Drawing.Size(75, 23);
            this.btnRedraw.TabIndex = 1;
            this.btnRedraw.Text = "Render";
            this.btnRedraw.UseVisualStyleBackColor = true;
            this.btnRedraw.Click += new System.EventHandler(this.btnRedraw_Click);
            // 
            // pbPreview
            // 
            this.pbPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbPreview.Location = new System.Drawing.Point(13, 42);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new System.Drawing.Size(265, 183);
            this.pbPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbPreview.TabIndex = 2;
            this.pbPreview.TabStop = false;
            // 
            // cbFractal
            // 
            this.cbFractal.FormattingEnabled = true;
            this.cbFractal.Location = new System.Drawing.Point(329, 14);
            this.cbFractal.Name = "cbFractal";
            this.cbFractal.Size = new System.Drawing.Size(121, 21);
            this.cbFractal.TabIndex = 3;
            this.cbFractal.SelectedIndexChanged += new System.EventHandler(this.cbFractal_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(284, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Fractal";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(456, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Renderer";
            // 
            // cbRenderer
            // 
            this.cbRenderer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbRenderer.FormattingEnabled = true;
            this.cbRenderer.Location = new System.Drawing.Point(513, 14);
            this.cbRenderer.Name = "cbRenderer";
            this.cbRenderer.Size = new System.Drawing.Size(229, 21);
            this.cbRenderer.TabIndex = 6;
            this.cbRenderer.SelectedIndexChanged += new System.EventHandler(this.cbRenderer_SelectedIndexChanged);
            // 
            // pbFractal
            // 
            this.pbFractal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbFractal.Location = new System.Drawing.Point(284, 42);
            this.pbFractal.Name = "pbFractal";
            this.pbFractal.Size = new System.Drawing.Size(805, 670);
            this.pbFractal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbFractal.TabIndex = 7;
            this.pbFractal.TabStop = false;
            this.pbFractal.Paint += new System.Windows.Forms.PaintEventHandler(this.pbFractal_Paint);
            this.pbFractal.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbFractal_MouseDown);
            this.pbFractal.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbFractal_MouseMove);
            this.pbFractal.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbFractal_MouseUp);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(829, 13);
            this.progressBar1.Maximum = 1000;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(260, 23);
            this.progressBar1.TabIndex = 8;
            // 
            // fsWatch
            // 
            this.fsWatch.EnableRaisingEvents = true;
            this.fsWatch.IncludeSubdirectories = true;
            this.fsWatch.NotifyFilter = ((System.IO.NotifyFilters)((((((System.IO.NotifyFilters.FileName | System.IO.NotifyFilters.DirectoryName) 
            | System.IO.NotifyFilters.Attributes) 
            | System.IO.NotifyFilters.Size) 
            | System.IO.NotifyFilters.LastWrite) 
            | System.IO.NotifyFilters.Security)));
            this.fsWatch.SynchronizingObject = this;
            this.fsWatch.Changed += new System.IO.FileSystemEventHandler(this.fsWatch_Changed);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 715);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1101, 22);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(39, 17);
            this.lblStatus.Text = "Ready";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Fullscreen";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Fractals
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1101, 737);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.pbFractal);
            this.Controls.Add(this.cbRenderer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbFractal);
            this.Controls.Add(this.pbPreview);
            this.Controls.Add(this.btnRedraw);
            this.Controls.Add(this.gbSettings);
            this.Name = "Fractals";
            this.Text = "Complex fractals";
            this.Shown += new System.EventHandler(this.Fractals_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFractal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fsWatch)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSettings;
        private System.Windows.Forms.Button btnRedraw;
        private System.Windows.Forms.PictureBox pbPreview;
        private System.Windows.Forms.ComboBox cbFractal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbRenderer;
        private System.Windows.Forms.PictureBox pbFractal;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.IO.FileSystemWatcher fsWatch;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.Button button1;
    }
}

