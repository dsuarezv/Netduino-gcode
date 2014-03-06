using System;

namespace gcodeparser
{
    public struct CPointF
    {
        public float X;
        public float Y;

        public CPointF(float x, float y)
        {
            X = x;
            Y = y;
        }
    }

    public class ArcInterpolation
    {
        internal CPointF Origin, Center, End;
        internal float Alpha, Beta, Gamma;
        internal float Distance;
        internal bool Clockwise;

        public ArcInterpolation(CPointF origin, CPointF end, float radius, bool clockwise)
        {
            // PhlatScript uses the radius format, so we need to support it. 

            Origin = origin;
            End = end;
            Distance = radius;
            Clockwise = !clockwise;

            // Calculate center. Best explanation found here: 
            // http://mathforum.org/library/drmath/view/53027.html

            float x1 = Origin.X;
            float y1 = Origin.Y;
            float x2 = End.X;
            float y2 = End.Y;
            float r = Distance;

            // Distance between start and end
            float q = Math2.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));

            // middle ploint between both points
            float x3 = (x1 + x2) / 2;
            float y3 = (y1 + y2) / 2;

            if (!Clockwise)
            {
                Center = new CPointF(
                    x3 - Math2.Sqrt(r * r - (q / 2) * (q / 2)) * (y1 - y2) / q,
                    y3 - Math2.Sqrt(r * r - (q / 2) * (q / 2)) * (x2 - x1) / q);
            }
            else
            {
                Center = new CPointF(
                    x3 + Math2.Sqrt(r * r - (q / 2) * (q / 2)) * (y1 - y2) / q,
                    y3 + Math2.Sqrt(r * r - (q / 2) * (q / 2)) * (x2 - x1) / q);
            }

            const string E_NO_ARC_CENTER = "Could not find a suitable center for arc";

            if (Center.X == float.MinValue) Machine.Error(E_NO_ARC_CENTER);
            if (Center.Y == float.MinValue) Machine.Error(E_NO_ARC_CENTER);

            Initialize();
        }

        public ArcInterpolation(CPointF origin, CPointF center, CPointF end, bool clockwise)
        {
            Origin = origin;
            Center = center;
            End = end;
            Clockwise = clockwise;

            Initialize();
        }

        private void Initialize()
        {
            // Distance from start to center. 

            const float twoPi = 2 * 3.141592654f;

            float oox = Origin.X - Center.X;
            float ooy = Origin.Y - Center.Y;

            float eex = End.X - Center.X;
            float eey = End.Y - Center.Y;

            Distance = (float)Math2.Sqrt(oox * oox + ooy * ooy);

            // Alpha angle: start with X axis

            Alpha = (float)Math2.Atan2(ooy, oox);

            // Beta angle: end with X axis

            Beta = (float)Math2.Atan2(eey, eex);

            // Gamma angle is arc angle (beta - alpha) 

            if (Alpha < 0 && Beta > 0)
            {
                Gamma = Beta - (Alpha + twoPi);
            }
            else if (Alpha > 0 && Beta < 0)
            {
                Gamma = (Beta + twoPi) - Alpha;
            }
            else
            {
                Gamma = Beta - Alpha;
            }

            if (Math2.Abs(Gamma) > 3.141592654f)
            {
                Gamma = Beta - Alpha;
            }
        }


        public CPointF GetArcPoint(float t)
        {
            float Delta = Gamma * t + Alpha;

            float x = Distance * (float)Math2.Cos(Delta);
            float y = Distance * (float)Math2.Sin(Delta);

            CPointF result = new CPointF(Center.X + x, Center.Y + y);

            return result;
        }
    }
}
