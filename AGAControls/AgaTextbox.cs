using System;
using System.Drawing;
using System.Windows.Forms;

namespace AGAControls
{
    public class AgaTextbox: TextBox
    {
        private bool mInited = false;

        public AgaTextbox()
        {
            BorderStyle = System.Windows.Forms.BorderStyle.None;
        }

        protected override void OnParentChanged(EventArgs e)
        {
            if (!mInited)
            {
                //ForeColor = Parent.ForeColor;
                BackColor = Parent.BackColor;
                mInited = true;
            }

            base.OnParentChanged(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {

        }
    }
}
