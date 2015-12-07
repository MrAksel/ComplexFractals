namespace ComplexFractals
{
    partial class Fullscreen
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
            this.pbFractal = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbFractal)).BeginInit();
            this.SuspendLayout();
            // 
            // pbFractal
            // 
            this.pbFractal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbFractal.Location = new System.Drawing.Point(0, 0);
            this.pbFractal.Name = "pbFractal";
            this.pbFractal.Size = new System.Drawing.Size(284, 261);
            this.pbFractal.TabIndex = 0;
            this.pbFractal.TabStop = false;
            this.pbFractal.Paint += new System.Windows.Forms.PaintEventHandler(this.pbFractal_Paint);
            this.pbFractal.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbFractal_MouseDown);
            this.pbFractal.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbFractal_MouseMove);
            this.pbFractal.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbFractal_MouseUp);
            // 
            // Fullscreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.ControlBox = false;
            this.Controls.Add(this.pbFractal);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Fullscreen";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Fractals fullscreen";
            ((System.ComponentModel.ISupportInitialize)(this.pbFractal)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbFractal;
    }
}