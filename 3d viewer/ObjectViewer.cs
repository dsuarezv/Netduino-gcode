using System;
using System.Drawing;
using System.Windows.Forms;

using Tao.OpenGl;
using Tao.Platform.Windows;

namespace Druid.Viewer
{
    public class Viewer3d : SimpleOpenGlControl
    {
        private MouseButtons mMouseButtons = MouseButtons.None;
        private Keys mModifierKeys = Keys.None;
        private int mBeginX, mBeginY;
        
        private float mZoomFactor = 50.0f;
        private float mElevation = 0; //(float)(10 * Math.PI / 180);
        private float mRotation = 0; //(float)(30 * Math.PI / 180);
        
        private Vect3f mTranslation = new Vect3f(0, 0, 0);
        private Vect3f mCurrentPoint = new Vect3f(0, 0, 0);
        private Vect3f mEye = new Vect3f(0, 0, 0);

        private bool mDrawAxis = true;


        // __ Events __________________________________________________________


        public event EventHandler InitializeGlScene;
        public event EventHandler DrawGlScene;


        // __ Public properties _______________________________________________


        public float Zoom
        {
            get
            {
                return mZoomFactor;
            }
            set
            {
                mZoomFactor = value;

                Invalidate();
            }
        }

        public Vect3f Translation
        {
            get
            {
                return mTranslation;
            }
            set
            {
                mTranslation = value;
                Invalidate();
            }
        }

        public bool DrawAxis
        {
            get { return mDrawAxis; }
            set { mDrawAxis = value; Invalidate(); }
        }


        // __ Control _________________________________________________________


        public Viewer3d()
        {
            InitOpenGl();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }


        // __ Initialization __________________________________________________


        private void InitOpenGl()
        {
            // Control

            AccumBits = ((System.Byte)(0));
            AutoCheckErrors = false;
            AutoFinish = false;
            AutoMakeCurrent = true;
            AutoSwapBuffers = true;
            ColorBits = ((System.Byte)(32));
            DepthBits = ((System.Byte)(16));
            StencilBits = ((System.Byte)(0));

            InitializeContexts();

            // OpenGl

            Gl.glShadeModel(Gl.GL_SMOOTH);
            Gl.glClearDepth(1);
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glDepthFunc(Gl.GL_LEQUAL);
            Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);         // Really Nice Perspective Calculations
            Gl.glEnable(Gl.GL_CULL_FACE);

            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
            Gl.glHint(Gl.GL_POINT_SMOOTH_HINT, Gl.GL_NICEST);
            Gl.glHint(Gl.GL_LINE_SMOOTH_HINT, Gl.GL_NICEST);

            // for cube mapping

            InitLights();

            Gl.glLoadIdentity();
            Gl.glColorMaterial(Gl.GL_FRONT, Gl.GL_DIFFUSE);
            Gl.glEnable(Gl.GL_COLOR_MATERIAL);
            Gl.glFrontFace(Gl.GL_CCW);

            if (InitializeGlScene != null) InitializeGlScene(this, EventArgs.Empty);
        }

        private void InitLights()
        {

            float[] matEspecular = new float[] { 0.0f, 1.0f, 1.0f, 1.0f };
            float[] matBright = new float[] { 100.0f };
            float[] matDiffuse = new float[] { 1.0f, 1.0f, 1.0f, 0.0f };
            float[] lightPosition = new float[] { 0.0f, 10000.0f, 10000.0f, 0.0f };

            //Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_AMBIENT, matEspecular);
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_SPECULAR, matEspecular);
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_SHININESS, matBright);
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_DIFFUSE, matDiffuse);

            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, lightPosition);
        }

        
        // __ Control events __________________________________________________


        protected override void OnPaint(PaintEventArgs e)
        {
            DrawScene();

            base.OnPaint(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            ResizeGL(Width, Height);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            mModifierKeys = e.Modifiers;

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            mModifierKeys = e.Modifiers;

            base.OnKeyUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            
            Motion(e.X, e.Y);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            mMouseButtons = e.Button;
            mBeginX = e.X;
            mBeginY = e.Y;

            if (e.Button == MouseButtons.Left)
            {
                // Select object
                Vect3f p = Get3DPoint(e.X, e.Y);
                mCurrentPoint = p;
                mCurrentPoint.Y = -mCurrentPoint.Y;

                mEye = CalcEye(mElevation, mRotation);
                mCurrentPoint = mCurrentPoint - mEye;
                mCurrentPoint.X *= 1000;
                mCurrentPoint.Y *= 1000;
                mCurrentPoint.Z *= 1000;

                // DAVE: launch event for object selection here. 
                // Should be used together with Object3D

                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            mMouseButtons &= ~e.Button;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            int steps = e.Delta / 120;  // 120 is the magic delta value for one single wheel step. .NET reference

            MotionZoom((int)(steps * 5));

            Invalidate();
        }

        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;

                float r = (float)value.R / 255;
                float g = (float)value.G / 255;
                float b = (float)value.B / 255;

                Gl.glClearColor(r, g, b, 0f);
            }
        }

        // __ Draw ____________________________________________________________


        private bool DrawScene()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);        // Clear The Screen And The Depth Buffer

            Gl.glLoadIdentity();

            Vect3f eye = CalcEye(mElevation, mRotation);

            double[] projBefore = GetProjection();

            Glu.gluLookAt(eye.X, eye.Y, eye.Z, 0, 0, 0, 0, 1, 0);

            double[] projAfter = GetProjection();

            Gl.glPushMatrix();
            Gl.glTranslatef(mTranslation.X, mTranslation.Y, mTranslation.Z);
            
            if (DrawGlScene != null) DrawGlScene(this, EventArgs.Empty);
            if (mDrawAxis) DoDrawAxis();

            Gl.glPopMatrix();

            return true;
        }


        // __ Camera __________________________________________________________


        private Vect3f CalcEye(float Elevation, float Rotation)
        {
            float x, y, z;
            y = mZoomFactor * (float)Math.Sin(Elevation);

            float rproj = mZoomFactor * (float)Math.Cos(Elevation);

            z = rproj * (float)Math.Cos(Rotation);
            x = rproj * (float)Math.Sin(Rotation);

            return new Vect3f(x, y, z);
        }

        private void MotionTranslate(int x, int y)
        {
            Vect3f origin = Get3DPoint(mBeginX, mBeginY);
            Vect3f destination = Get3DPoint(x, y);            
            mTranslation.X += (destination.X - origin.X);
            mTranslation.Y -= (destination.Y - origin.Y);
            mTranslation.Z += (destination.Z - origin.Z);
        }

        private void MotionRotate(int x, int y)
        {
            int h = Height;
            int w = Width;

            mRotation += ((mBeginX - x) / (float)w) * 2.0f * (float)Math.PI;
            mElevation += ((y - mBeginY) / (float)h) * 2.0f * (float)Math.PI;
            Constraint(ref mElevation, (float)Math.PI / 2 - 0.0005f);
        }

        private void MotionZoom(int steps)
        {
            mZoomFactor = mZoomFactor - steps;
        }

        private void Motion(int x, int y)
        {
            if (mMouseButtons == MouseButtons.None)
                return;

            if (mMouseButtons == MouseButtons.Left)
            {
                if( (mModifierKeys & Keys.Shift) == Keys.Shift )
                {
                    MotionTranslate(x, y);
                }
                else if ((mModifierKeys & Keys.Control) == Keys.Control)
                {
                    int steps = (mBeginY - y) / 3;

                    MotionZoom(steps);
                }
                else
                {
                    MotionRotate(x, y);
                }
            }
            else if (mMouseButtons == MouseButtons.Middle)
            {
                MotionTranslate(x, y);
            }

            mBeginX = x;
            mBeginY = y;

            Invalidate();
        }


        // __ Helper methods __________________________________________________


        private void ResizeGL(int width, int height)
        {
            if (height == 0) height = 1;

            Gl.glViewport(0, 0, width, height);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Glu.gluPerspective(45, width / (double)height, 0.1, 50000);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
        }

        private void DoDrawAxis()
        {
            Gl.glLineWidth(1f);
            Gl.glColor4f(0.8f, 0f, 0f, 1f);

            float axislen = 1f;
            
            Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex3f(-axislen, 0, 0);
            Gl.glVertex3f(axislen, 0, 0);
            Gl.glVertex3f(0, -axislen, 0);
            Gl.glVertex3f(0, axislen, 0);

            Gl.glColor4f(0.8f, 0.8f, 0.8f, 1f);
            
            Gl.glVertex3f(0, 0, -axislen);
            Gl.glVertex3f(0, 0, axislen);

            /*
            // Origin grid

            // divisions in x
            for (int i = 0; i < 10; ++i)
            {
                Gl.glVertex3f(i, 0, -axislen);
                Gl.glVertex3f(i, 0, axislen);
                Gl.glVertex3f(-i, 0, -axislen);
                Gl.glVertex3f(-i, 0, axislen);
            }

            for (int i = 0; i < 10; ++i)
            {
                Gl.glVertex3f(-axislen, 0, i);
                Gl.glVertex3f(axislen, 0, i);
                Gl.glVertex3f(axislen, 0, -i);
                Gl.glVertex3f(-axislen, 0, -i);
            }
            */

            Gl.glEnd();
        }

        private double[] GetProjection()
        {
            double[] result = new double[16];

            Gl.glGetDoublev(Gl.GL_MODELVIEW_MATRIX, result);

            return result;
        }

        public static float RadiansToDegrees(float radians)
        {
            return radians * (180f / (float)Math.PI);
        }

        public static float AngleBetweenVectors(Vect3f v1, Vect3f v2)
        {
            return RadiansToDegrees(
                (float)Math.Acos((v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z) /
                (v1.Length() * v2.Length())));
        }

        private static void Constraint(ref float val, float limit)
        {
            if (val < -limit) val = -limit;
            if (val > limit)  val = limit;
        }

        private Vect3f Get3DPoint(int x, int y)
        {
            double dX, dY, dZ;
            double[] modelview = new double[16];
            double[] projmatrix = new double[16];
            int[] viewport = new int[4];

            Gl.glLoadIdentity();
            Gl.glGetIntegerv(Gl.GL_VIEWPORT, viewport);
            Gl.glGetDoublev(Gl.GL_PROJECTION_MATRIX, projmatrix);

            Vect3f eye = CalcEye(-mElevation, mRotation);
            Glu.gluLookAt(eye.X, eye.Y, eye.Z, 0, 0, 0, 0, 1, 0);

            // get the current modelview matrix
            Gl.glGetDoublev(Gl.GL_MODELVIEW_MATRIX, modelview);

            Glu.gluUnProject((double)x, (double)y, 0.996, modelview, projmatrix, viewport,
                out dX, out dY, out dZ);

            return new Vect3f((float)dX, (float)(dY), (float)(dZ));
        }
    }
    



    // == Vector3 =============================================================




    public class Vect3f
    {
        public float Z;
        public float Y;
        public float X;

        public Vect3f(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Vect3f(Vect3f copy)
        {
            this.X = copy.X;
            this.Y = copy.Y;
            this.Z = copy.Z;
        }

        public void crossProd(Vect3f v1, Vect3f v2, Vect3f v3)
        {
            float x1, y1, z1, x2, y2, z2;
            x1 = v3.X - v2.X;
            y1 = v3.Y - v2.Y;
            z1 = v3.Z - v2.Z;

            x2 = v1.X - v2.X;
            y2 = v1.Y - v2.Y;
            z2 = v1.Z - v2.Z;

            X = y1 * z2 - y2 * z1;
            Y = x2 * z1 - x1 * z2;
            Z = x1 * y2 - x2 * y1;
            Normalize();
        }

        public void crossProd(Vect3f v1, Vect3f v2)
        {
            X = v1.Y * v2.Z - v1.Z * v2.Y;
            Y = v1.Z * v2.X - v1.X * v2.Z;
            Z = v1.X * v2.Y - v1.Y * v2.X;
            Normalize();
        }

        public void Normalize()
        {
            float length = Length();
            X /= length;
            Y /= length;
            Z /= length;
        }

        public float Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public Vect3f Minus(Vect3f v1)
        {
            Vect3f result = new Vect3f(X, Y, Z);
            result.X -= v1.X;
            result.Y -= v1.Y;
            result.Z -= v1.Z;
            return result;
        }

        public void Divide(float fact)
        {
            X = X / fact;
            Y = Y / fact;
            Z = Z / fact;
        }

        public static Vect3f operator +(Vect3f a, Vect3f b)
        {
            return new Vect3f(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vect3f operator -(Vect3f a, Vect3f b)
        {
            return new Vect3f(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public override string ToString()
        {
            return string.Format("({0:.4}, {1:.4}, {2:.4})", X, Y, Z);
        }

    }

    internal class Quaternion
    {
        private float[] q = new float[4];


        public Quaternion()
        {
        }

        public Quaternion(float a, float b, float c, float d)
        {
            q[0] = a;
            q[1] = b;
            q[2] = c;
            q[3] = d;
        }

        public Quaternion(Vect3f axis, float angle)
        {
            SetAxisAngle(axis, angle);
        }

        public float this[int index]
        {
            get { return q[index]; }
        }


        private void SetAxisAngle(Vect3f axis, float angle)
        {
            axis.Normalize();
            float norm = axis.Length();

            if (norm < 1E-8)
            {
                q[0] = 0.0f;
                q[1] = 0.0f;
                q[2] = 0.0f;
                q[3] = 1.0f;
            }
            else
            {
                float sin_half_angle = (float)Math.Sin(angle / 2.0);
                q[0] = sin_half_angle * axis.X / norm;
                q[1] = sin_half_angle * axis.Y / norm;
                q[2] = sin_half_angle * axis.Z / norm;
                q[3] = (float)Math.Cos(angle / 2.0);
            }
        }

        public static Quaternion operator *(Quaternion a, Quaternion b)
        {
            return new Quaternion(
                a.q[3] * b.q[0] + b.q[3] * a.q[0] + a.q[1] * b.q[2] - a.q[2] * b.q[1],
                a.q[3] * b.q[1] + b.q[3] * a.q[1] + a.q[2] * b.q[0] - a.q[0] * b.q[2],
                a.q[3] * b.q[2] + b.q[3] * a.q[2] + a.q[0] * b.q[1] - a.q[1] * b.q[0],
                a.q[3] * b.q[3] - b.q[0] * a.q[0] - a.q[1] * b.q[1] - a.q[2] * b.q[2]);
        }
    }

}
