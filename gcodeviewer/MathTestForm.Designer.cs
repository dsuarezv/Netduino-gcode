namespace gcodeparser
{
    partial class MathTestForm
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
            this.agaLabel1 = new AGAControls.AgaLabel();
            this.ResultTextbox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // agaLabel1
            // 
            this.agaLabel1.Location = new System.Drawing.Point(12, 13);
            this.agaLabel1.Name = "agaLabel1";
            this.agaLabel1.Size = new System.Drawing.Size(268, 50);
            this.agaLabel1.TabIndex = 0;
            this.agaLabel1.Text = "Checks the software math against the .NET framework Math class.";
            this.agaLabel1.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // 
            // ResultTextbox
            // 
            this.ResultTextbox.Location = new System.Drawing.Point(15, 67);
            this.ResultTextbox.Multiline = true;
            this.ResultTextbox.Name = "ResultTextbox";
            this.ResultTextbox.Size = new System.Drawing.Size(265, 339);
            this.ResultTextbox.TabIndex = 1;
            // 
            // MathTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 418);
            this.Controls.Add(this.ResultTextbox);
            this.Controls.Add(this.agaLabel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MathTestForm";
            this.Text = "Math test";
            this.Load += new System.EventHandler(this.MathTestForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AGAControls.AgaLabel agaLabel1;
        private System.Windows.Forms.TextBox ResultTextbox;
    }
}