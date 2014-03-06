namespace gcodeparser
{
    partial class ManualModeForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.SetZeroLabel = new System.Windows.Forms.Label();
            this.SetFeedRate = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(29, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(228, 92);
            this.label1.TabIndex = 0;
            this.label1.Text = "Use arrow keys for X and Y axis. Page up and Page down for Z. Press Shift for fin" +
                "e movement";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SetZeroLabel
            // 
            this.SetZeroLabel.BackColor = System.Drawing.Color.GreenYellow;
            this.SetZeroLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SetZeroLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SetZeroLabel.ForeColor = System.Drawing.Color.DarkGreen;
            this.SetZeroLabel.Location = new System.Drawing.Point(13, 154);
            this.SetZeroLabel.Name = "SetZeroLabel";
            this.SetZeroLabel.Size = new System.Drawing.Size(259, 92);
            this.SetZeroLabel.TabIndex = 1;
            this.SetZeroLabel.Text = "Set current location as 0, 0, 0";
            this.SetZeroLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.SetZeroLabel.Click += new System.EventHandler(this.SetZeroLabel_Click);
            // 
            // SetFeedRate
            // 
            this.SetFeedRate.BackColor = System.Drawing.Color.GreenYellow;
            this.SetFeedRate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SetFeedRate.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SetFeedRate.ForeColor = System.Drawing.Color.DarkGreen;
            this.SetFeedRate.Location = new System.Drawing.Point(13, 257);
            this.SetFeedRate.Name = "SetFeedRate";
            this.SetFeedRate.Size = new System.Drawing.Size(259, 33);
            this.SetFeedRate.TabIndex = 2;
            this.SetFeedRate.Text = "Set Feed Rate";
            this.SetFeedRate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.SetFeedRate.Click += new System.EventHandler(this.SetFeedRate_Click);
            // 
            // ManualModeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(284, 303);
            this.Controls.Add(this.SetFeedRate);
            this.Controls.Add(this.SetZeroLabel);
            this.Controls.Add(this.label1);
            this.Name = "ManualModeForm";
            this.Text = "ManualModeForm";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ManualModeForm_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label SetZeroLabel;
        private System.Windows.Forms.Label SetFeedRate;
    }
}