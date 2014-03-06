using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Diagnostics;



namespace gcodeparser
{
    public class ViewerDevice: Device
    {
        // Machine simulation

        public static float MmPerMinute = 119.8f;
        public static float ZCutLevel = 0;
        public float CutAreaWidth = 200f;
        public float CutAreaHeight = 100f;
        public float GridStep = 10f;
        protected float mToolDiameter = 0.1f;
        protected int mCurrentStep = 0;
        protected ViewerLine mCurrentLine;
        protected int mCurrentLineIndex = 0;
        protected string mLastCodeLine;

        protected Control mParentControl;
        protected List<ViewerStep> mSteps = new List<ViewerStep>();
        protected List<ViewerLine> mCodeLines = new List<ViewerLine>();
        protected float mCurrentDistance = 0f;


        public event EventHandler CodeChanged;


        // 2D Implementation 

        private float mDeviceScale = 1.0f;
        private float mVisualScale = 10f;
        private Pen mGridPen = new Pen(Color.FromArgb(15, Color.White), 0.1f);
        private Pen mToolPen;

        internal static Pen CutPen;
        internal static Pen DonePen;
        private static Pen MovePen = new Pen(Color.LightGray, 0.1f);

        public float VisualScale
        {
            get
            {
                return mVisualScale;
            }
            set 
            {
                mVisualScale = value;
                mParentControl.Invalidate();
            }
        }

        public float DeviceScale
        {
            get
            {
                return mDeviceScale;
            }
            set
            {
                mDeviceScale = value;
                mParentControl.Invalidate();
            }
        }

        public float ToolDiameter
        {
            get
            {
                return mToolDiameter;
            }
            set
            {
                mToolDiameter = value;
                ViewerStep.ToolDiameter = mToolDiameter;
                SetToolPensDiameter(mToolDiameter);

                if (mToolPen != null) mToolPen.Dispose();

                mToolPen = new Pen(Color.LightSalmon, mToolDiameter / 2);

                mParentControl.Invalidate();
            }
        }


        internal ViewerStep CurrentStep
        {
            get
            {
                return mSteps[mCurrentStep];
            }
        }

        public float CurrentDistance
        {
            get
            {
                return mCurrentDistance;
            }
        }

        public int CurrentStepIndex
        {
            get 
            {
                return mCurrentStep;
            }
            set
            {
                mCurrentStep = value;
                mParentControl.Invalidate();
            }
        }

        public int TotalSteps
        {
            get
            {
                return mSteps.Count;
            }
        }

        public List<ViewerLine> CodeLines
        {
            get
            {
                return mCodeLines;
            }
        }

        public int CurrentCodeLineIndex
        {
            get
            {
                return mCurrentLineIndex;
            }
            set
            {
                mCurrentLineIndex = value;
                mParentControl.Invalidate();
            }
        }


        public ViewerDevice(Control parentControl)
        {
            mParentControl = parentControl;
        }

        public virtual void Initialize()
        {
            mParentControl.Paint += new PaintEventHandler(this.Paint);

            if (CutPen == null) SetToolPensDiameter(3.2f);
        }


        // __ Device Impl _______________________________________________________________


        public override void Print(string s)
        {
            
        }

        public override void SetFeedRate(float mmPerMinute)
        { 
            // Do nothing
        }

        public override void SetPosition(float x, float y, float z)
        {
            SetCurrent(x, y, z);
        }

        public override void MoveAbsoluteLinear(float x, float y, float z)
        {
            AddStep(mCurrentX, mCurrentY, mCurrentZ, x, y, z);

            SetCurrent(x, y, z);
        }

        public override void MoveRawX(int steps)
        {
            throw new NotImplementedException();
        }

        public override void MoveRawY(int steps)
        {
            throw new NotImplementedException();
        }

        public override void MoveRawZ(int steps)
        {
            throw new NotImplementedException();
        }

        public override void Calibrate(float xStepsPerMm, float yStepsPerMm, float zStepsPerMm)
        {
            // Do nothing
        }

        public float GetTotalDistance()
        {
            float result = 0f;

            foreach (ViewerStep op in mSteps)
            {
                result += op.Distance;
            }

            return result;
        }


        private void AddStep(float fromX, float fromY, float fromZ, float toX, float toY, float toZ)
        {
            ViewerStep op = new ViewerStep(
                fromX * mDeviceScale, 
                fromY * mDeviceScale, 
                fromZ,
                toX * mDeviceScale,
                toY * mDeviceScale,
                toZ);

            string line = GCodeParser.Line;

            op.GCodeLine = line;

            if (mLastCodeLine != line)
            {
                mCurrentLine = new ViewerLine(line);
                mLastCodeLine = line;
                mCodeLines.Add(mCurrentLine);
            }

            mCurrentLine.Steps.Add(op);
            mSteps.Add(op);

            if (CodeChanged != null) CodeChanged(this, EventArgs.Empty);
        }

        private void SetCurrent(float x, float y, float z)
        {
            if (x > float.MinValue) mCurrentX = x;
            if (y > float.MinValue) mCurrentY = y;
            if (z > float.MinValue) mCurrentZ = z;
        }


        // __ Paint handling ____________________________________________________________

        
        public void Clear()
        {
            mCodeLines.Clear();
            mSteps.Clear();
            mCurrentStep = 0;
            mCurrentX = 0;
            mCurrentY = 0;
            mCurrentZ = 0;
        }

        internal void Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.Clear(mParentControl.BackColor);

            g.TranslateTransform(0, mParentControl.Height - 40);
            g.ScaleTransform(mVisualScale, -mVisualScale);
            g.TranslateTransform(5, 5);

            g.SmoothingMode = SmoothingMode.AntiAlias;

            DrawGrid(g);

            ViewerStep op = null;
            ViewerStep head = null;
            mCurrentDistance = 0f;

            for (int i = 0; i < mSteps.Count; ++i)
            {
                op = mSteps[i];

                if (i <= mCurrentStep)
                {
                    PaintStep(op, g, DonePen);

                    mCurrentDistance += op.Distance;
                    head = op;
                }
                else
                {
                    PaintStep(op, g);
                }
            }

            if (head != null)
            {
                if (mToolPen == null) ToolDiameter = ToolDiameter;

                g.DrawEllipse(mToolPen, GetRect(head.End.X, head.End.Y, mToolDiameter / 4));
                //g.DrawString(
                    ////string.Format("{0}, {1}", op.End.X, op.End.Y),
                    //string.Format("{0}", mCurrentStep),
                    //mParentControl.Font, Brushes.Red, op.End);
            }
        }

        private void PaintStep(ViewerStep op, Graphics g)
        {
            Pen p = op.IsCuttingOp ? CutPen : MovePen;

            PaintStep(op, g, p);
        }

        private void PaintStep(ViewerStep op, Graphics g, Pen p)
        {
            float xx = Math.Abs(op.Start.X - op.End.X);
            float yy = Math.Abs(op.Start.Y - op.End.Y);

            if (xx < 0.00001 && yy < 0.00001) return;

            p = op.IsCuttingOp ? p : MovePen;

            g.DrawLine(p, op.Start.X, op.Start.Y, op.End.X, op.End.Y);
        }

        protected virtual void SetToolPensDiameter(float diameter)
        {
            if (CutPen != null) CutPen.Dispose();
            if (DonePen != null) DonePen.Dispose();

            CutPen = GetRoundedPen(Color.White, diameter);
            DonePen = GetRoundedPen(Color.Red, diameter);
        }

        private static Pen GetRoundedPen(Color col, float diameter)
        {
            Pen result = new Pen(col, diameter);

            result.StartCap = LineCap.Round;
            result.EndCap = LineCap.Round;

            return result;
        }

        private void DrawGrid(Graphics g)
        {
            int w = (int)CutAreaWidth; 
            int h = (int)CutAreaHeight; 

            for (int x = 0; x <= w; x += (int)GridStep) g.DrawLine(mGridPen, x, 0, x, h);
            for (int y = 0; y <= h; y += (int)GridStep) g.DrawLine(mGridPen, 0, y, w, y);

            string s = string.Format("Each square = {0:0.0}mm", GridStep);

            GraphicsState state = g.Save();
            g.ResetTransform();
            g.DrawString(s, mParentControl.Font, Brushes.White, 20, mParentControl.Height - 40);
            g.Restore(state);

        }

        private static RectangleF GetRect(float x, float y)
        {
            return GetRect(x, y, 4);
        }

        private static RectangleF GetRect(float x, float y, float r)
        {
            float r2 = r * 2;

            return new RectangleF(x - r, y - r, r2, r2);
        }

    }

    public class ViewerLine
    {
        public string GCodeLine;

        public List<ViewerStep> Steps = new List<ViewerStep>();

        public ViewerLine(string gCodeLine)
        {
            GCodeLine = gCodeLine;
        }
    }
}
