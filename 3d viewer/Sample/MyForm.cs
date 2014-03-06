using System;
using System.Drawing;
using System.Windows.Forms;

using Tao.OpenGl;


namespace Sample
{
    public partial class MyForm : Form
    {
        public MyForm()
        {
            InitializeComponent();
        }

        private void objectViewer1_InitializeGlScene(object sender, EventArgs e)
        {
            
        }

        private void objectViewer1_DrawGlScene(object sender, EventArgs e)
        {
            Gl.glBegin(Gl.GL_LINES);

            Gl.glColor3f(1f, 0f, 0f);

            Gl.glVertex3f(0f, 0f, 0f);
            Gl.glVertex3f(20f, 20f, 20f);

            Gl.glColor3f(0f, 1f, 0f);
            Gl.glVertex3f(40f, 20f, 20f);
            Gl.glVertex3f(20f, 40f, 20f);

            Gl.glEnd();

            UpdateStatus();
        }

        private void UpdateStatus()
        {
            label1.Text = string.Format("Zoom: {0}   Translate: {1}", objectViewer1.Zoom, objectViewer1.Translation);
        }
    }
}
