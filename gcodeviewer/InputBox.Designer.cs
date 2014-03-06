namespace gcodeparser
{
    partial class InputBox
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
            this.TextLabel = new AGAControls.AgaLabel();
            this.InputTextBox = new AGAControls.AgaTextbox();
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelButt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TextLabel
            // 
            this.TextLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TextLabel.Location = new System.Drawing.Point(13, 13);
            this.TextLabel.Name = "TextLabel";
            this.TextLabel.Size = new System.Drawing.Size(255, 89);
            this.TextLabel.TabIndex = 0;
            this.TextLabel.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // 
            // InputTextBox
            // 
            this.InputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.InputTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.InputTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InputTextBox.Location = new System.Drawing.Point(12, 105);
            this.InputTextBox.Name = "InputTextBox";
            this.InputTextBox.Size = new System.Drawing.Size(256, 20);
            this.InputTextBox.TabIndex = 1;
            // 
            // OkButton
            // 
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Location = new System.Drawing.Point(113, 131);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 2;
            this.OkButton.Text = "Ok";
            this.OkButton.UseVisualStyleBackColor = true;
            // 
            // CancelButt
            // 
            this.CancelButt.Location = new System.Drawing.Point(195, 131);
            this.CancelButt.Name = "CancelButt";
            this.CancelButt.Size = new System.Drawing.Size(75, 23);
            this.CancelButt.TabIndex = 3;
            this.CancelButt.Text = "Cancel";
            this.CancelButt.UseVisualStyleBackColor = true;
            // 
            // InputBox
            // 
            this.AcceptButton = this.OkButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelButt;
            this.ClientSize = new System.Drawing.Size(284, 162);
            this.Controls.Add(this.CancelButt);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.InputTextBox);
            this.Controls.Add(this.TextLabel);
            this.Name = "InputBox";
            this.Text = "InputBox";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AGAControls.AgaLabel TextLabel;
        private AGAControls.AgaTextbox InputTextBox;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelButt;
    }
}