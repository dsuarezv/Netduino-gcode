namespace Sample
{
    partial class MyForm
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
            this.objectViewer1 = new Druid.Viewer.Viewer3d();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // objectViewer1
            // 
            this.objectViewer1.AccumBits = ((byte)(0));
            this.objectViewer1.AutoCheckErrors = false;
            this.objectViewer1.AutoFinish = false;
            this.objectViewer1.AutoMakeCurrent = true;
            this.objectViewer1.AutoSwapBuffers = true;
            this.objectViewer1.BackColor = System.Drawing.Color.White;
            this.objectViewer1.ColorBits = ((byte)(32));
            this.objectViewer1.DepthBits = ((byte)(16));
            this.objectViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectViewer1.Location = new System.Drawing.Point(0, 33);
            this.objectViewer1.Name = "objectViewer1";
            this.objectViewer1.Size = new System.Drawing.Size(284, 289);
            this.objectViewer1.StencilBits = ((byte)(0));
            this.objectViewer1.TabIndex = 0;
            this.objectViewer1.InitializeGlScene += new System.EventHandler(this.objectViewer1_InitializeGlScene);
            this.objectViewer1.DrawGlScene += new System.EventHandler(this.objectViewer1_DrawGlScene);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(284, 33);
            this.panel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "info";
            // 
            // MyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 322);
            this.Controls.Add(this.objectViewer1);
            this.Controls.Add(this.panel1);
            this.Name = "MyForm";
            this.Text = "MyForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Druid.Viewer.Viewer3d objectViewer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
    }
}