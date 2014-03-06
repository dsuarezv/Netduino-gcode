using System;
using System.Collections.Generic;

using Druid.Viewer;
using Tao.OpenGl;

namespace gcodeparser
{
    public class Viewer3dDevice : ViewerDevice
    {
        private Viewer3d mViewControl;
        private GlPen GlCutPen = new GlPen(new ColorRgb(0f, 0.6f, 0f), 3f);
        private GlPen GlCutPendingPen = new GlPen(new ColorRgb(1f, 0f, 0f), 3f);
        private GlPen GlMovePen = new GlPen(new ColorRgb(0.7f, 0.7f, 0.7f), 1f);
        private GlPen GlGridPen = new GlPen(new ColorRgb(0.5f, 0.5f, 0.5f), 1f);
        private GlPen GlSmallGridPen = new GlPen(new ColorRgb(0.4f, 0.4f, 0.4f), 0.5f);


        public Viewer3dDevice(Viewer3d target) : base(target)
        {
            mViewControl = target;
        }

        public override void Initialize()
        {
            mViewControl.InitializeGlScene += new EventHandler(mViewControl_InitializeGlScene);
            mViewControl.DrawGlScene += new EventHandler(mViewControl_DrawGlScene);
        }

        protected override void SetToolPensDiameter(float diameter)
        {
            GlCutPen.Width = diameter;
            GlMovePen.Width = 1f;
        }

        void mViewControl_InitializeGlScene(object sender, EventArgs e)
        {
            Gl.glEnable(Gl.GL_LINE_SMOOTH);

            mViewControl.DrawAxis = false;
        }

        void mViewControl_DrawGlScene(object sender, EventArgs e)
        {
            SetupBoundingBox();

            DrawGrid();

            mCurrentDistance = 0f;

            //ViewerStep head = null;
            //for (int i = 0; i < mSteps.Count; ++i)
            //{
            //    DrawStep(mSteps[i], ref head);
            //}

            for (int i = 0; i < mCodeLines.Count; ++i)
            {
                DrawCodeLine(i);
            }
        }

        private void DrawCodeLine(int index)
        {
            ViewerStep head = null;

            foreach (ViewerStep step in mCodeLines[index].Steps)
            {
                DrawStep(step, ref head, (mCurrentLineIndex > index) );
            }
        }

        private int mLastOperationCount = 0;
        private BoundingBox mLimits = new BoundingBox(new Vect3f(0, 0, 0), new Vect3f(0, 0, 0));

        private void SetupBoundingBox()
        {
            if (mSteps.Count == mLastOperationCount) return;

            mLimits = new BoundingBox(new Vect3f(0, 0, 0), new Vect3f(0, 0, 0));

            foreach (ViewerStep op in mSteps)
            {
                CheckOpBoundingBox(op);
            }

            mLastOperationCount = mSteps.Count;

            // Center model

            Vect3f midPoint = new Vect3f(
                (mLimits.MinPoint.X - mLimits.MaxPoint.X) / 2,
                (mLimits.MinPoint.Y - mLimits.MaxPoint.Y) / 2,
                0f);

            mViewControl.Translation = midPoint;
        }

        private void CheckOpBoundingBox(ViewerStep op)
        {
            CheckPointBoundingBox(op.Start);
            CheckPointBoundingBox(op.End);
        }

        private void CheckPointBoundingBox(Vect3f p)
        {
            if (p.X > mLimits.MaxPoint.X) mLimits.MaxPoint.X = p.X;
            if (p.Y > mLimits.MaxPoint.Y) mLimits.MaxPoint.Y = p.Y;
            if (p.Z > mLimits.MaxPoint.Z) mLimits.MaxPoint.Z = p.Z;

            if (p.X < mLimits.MinPoint.X) mLimits.MinPoint.X = p.X;
            if (p.Y < mLimits.MinPoint.Y) mLimits.MinPoint.Y = p.Y;
            if (p.Z < mLimits.MinPoint.Z) mLimits.MinPoint.Z = p.Z;
        }

        private void DrawStep(ViewerStep op, ref ViewerStep head, bool alreadyCut)
        {
            GlPen pen = (op.IsCuttingOp) ? GlCutPen : GlMovePen;

            if (pen == GlCutPen && !alreadyCut) pen = GlCutPendingPen;

            PaintStep(op, pen);

            mCurrentDistance += op.Distance;
            head = op;
        }

        private void SetPen(GlPen pen)
        {
            Gl.glLineWidth(pen.Width);
            Gl.glColor3f(pen.Color.R, pen.Color.G, pen.Color.B);
        }

        private void PaintStep(ViewerStep op, GlPen pen)
        {
            SetPen(pen);

            Gl.glBegin(Gl.GL_LINES);            
            Gl.glVertex3f(op.Start.X, op.Start.Y, op.Start.Z);
            Gl.glVertex3f(op.End.X, op.End.Y, op.End.Z);
            Gl.glEnd();
        }

        private void DrawGrid()
        {
            int marginSteps = 0;

            float step = 10;
            float min = Math.Min(mLimits.MinPoint.X, mLimits.MinPoint.Y);
            float max = Math.Max(mLimits.MaxPoint.X, mLimits.MaxPoint.Y);

            min = (float)(Math.Ceiling(min / step) - marginSteps) * step;
            max = (float)(Math.Ceiling(max / step) + marginSteps) * step;

            DrawGrid(step, min, max);
        }

        private void DrawGrid(float step, float start, float end)
        {
            SetPen(GlSmallGridPen);

            Gl.glBegin(Gl.GL_LINES);

            //for (float x = start; x <= end; x += step / 10)
            //{
            //    Gl.glVertex3f(x, start, 0);
            //    Gl.glVertex3f(x, end, 0);

            //    Gl.glVertex3f(start, x, 0);
            //    Gl.glVertex3f(end, x, 0);
            //}

            SetPen(GlGridPen);

            for (float x = start; x <= end; x += step)
            {
                Gl.glVertex3f(x, start, 0);
                Gl.glVertex3f(x, end, 0);

                Gl.glVertex3f(start, x, 0);
                Gl.glVertex3f(end, x, 0);
            }

            Gl.glEnd();
        }
    }


    internal class GlPen
    {
        public float Width;
        public ColorRgb Color;

        public GlPen(ColorRgb color, float width)
        {
            Width = width;
            Color = color;
        }
    }


    public class ViewerStep
    {
        public static float ToolDiameter = 3.2f;

        public Vect3f Start;
        public Vect3f End;
        internal float Distance;

        internal string GCodeLine;

        public bool IsCuttingOp
        {
            get { return (End.Z <= ViewerDevice.ZCutLevel) && (Start.Z <= ViewerDevice.ZCutLevel); }
        }

        public ViewerStep(float fromX, float fromY, float fromZ, float toX, float toY, float toZ)
        {
            Start = new Vect3f(fromX, fromY, fromZ);

            if (toX == float.MinValue) toX = fromX;
            if (toY == float.MinValue) toY = fromY;
            if (toZ == float.MinValue) toZ = fromZ;

            End = new Vect3f(toX, toY, toZ);

            Distance = (float)Math.Sqrt(
                Math.Pow(toX - fromX, 2) +
                Math.Pow(toY - fromY, 2) +
                Math.Pow(toZ - fromZ, 2));
        }
    }
}
