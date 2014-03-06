using System;
using System.Drawing;
using System.Windows.Forms;


namespace AGAControls
{
    public class AgaProgress: Control
    {
        private const float mPenWidth = 7f;
        private const int mPadding = 10;

        private Pen mBackPen;
        private Pen mProgressPen;
        private Color mBackColor;
        private Color mProgressColor;
        private int mMinimum = 0;
        private int mMaximum = 100;
        private int mValue = 20;


        public int Maximum
        {
            get
            {
                return mMaximum;
            }
            set
            {
                mMaximum = value;
                Invalidate();
            }
        }

        public int Minimum
        {
            get { return mMinimum; }
            set
            {
                mMinimum = value;
                Invalidate();
            }
        }

        public int Value
        {
            get
            {
                return mValue;
            }

            set
            {
                mValue = value;
                Invalidate();
            }
        }



        public Color ProgressBackColor
        {
            get
            {
                return mBackColor;
            }
            set
            {
                mBackColor = value;
                if (mBackPen != null) mBackPen.Dispose();
                mBackPen = GetPenForColor(mBackColor);
            }
        }

        public Color ProgressColor
        {
            get
            {
                return mProgressColor;
            }
            set
            {
                mProgressColor = value;
                if (mProgressPen != null) mProgressPen.Dispose();
                mProgressPen = GetPenForColor(mProgressColor);
            }
        }


        public AgaProgress()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint
                | ControlStyles.UserPaint
                | ControlStyles.OptimizedDoubleBuffer
                , true);

            ProgressBackColor = Color.White;
            ProgressColor = Color.FromArgb(255, 170, 0);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int total = mMaximum - mMinimum;
            total = (total == 0) ? 1 : total;
            int width = this.Width - mPadding * 2;
            float scale = (float)width / (float)total;
            int xValue = (int)(scale * mValue);
            int x = mPadding;
            int y = this.Height / 2;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.DrawLine(mBackPen, x, y, this.Width - x, y);
            e.Graphics.DrawLine(mProgressPen, x, y, xValue + x, y);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            { 
                
            }

            base.Dispose(disposing);
        }


        private Pen GetPenForColor(Color color)
        {
            Pen result = new Pen(color, mPenWidth);

            result.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            result.StartCap = System.Drawing.Drawing2D.LineCap.Round;

            return result;
        }
        
    }
}
