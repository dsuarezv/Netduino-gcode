using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace AGAControls
{
    public class AgaLabel : Label
    {
        private TextRenderingHint mTextRenderingHint = TextRenderingHint.ClearTypeGridFit;


        public TextRenderingHint TextRenderingHint
        {
            get
            {
                return mTextRenderingHint;
            }
            set
            {
                mTextRenderingHint = value;

                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = mTextRenderingHint;

            base.OnPaint(e);
        }
    }
}
