using System;

namespace gcodeparser
{
    public enum MachineState
    {
        Stopped,
        G0_RapidMove, 
        G1_LinearMove,
        G2_ArcMove, 
        G3_ArcMoveCCW,
        G92_SetPosition,
        D21_ManualMode,
        D92_Calibration
    }

    public enum MachinePlane { XY, XZ, YZ }
    public enum DistanceMode { Absolute, Relative }

    public class Machine
    {

        private const float ArcStep = 0.1f;  // Make sure it's a "multiple" of 1.0f.

        private static MachineState mState = MachineState.Stopped;
        private static MachinePlane mPlane = MachinePlane.XY;
        private static DistanceMode mDistanceMode = DistanceMode.Absolute;

        private static Device mDev;

        public static void SetState(MachineState state)
        {
            mState = state;

            if (state == MachineState.D21_ManualMode)
            {
                ProcessManualMode();
            }
        }

        public static void SetPlane(MachinePlane plane)
        {
            mPlane = plane;
        }

        public static void SetMode(DistanceMode mode)
        {
            mDistanceMode = mode;
        }

        public static void Move(float x, float y, float z, float i, float j, float k, float r)
        {
            Initialize();

            switch (mState)
            { 
                case MachineState.G0_RapidMove:
                case MachineState.G1_LinearMove:
                    LinearMove(x, y, z);
                    break;
                case MachineState.G2_ArcMove:
                    ArcMove(x, y, z, i, j, k, r, true);
                    break;
                case MachineState.G3_ArcMoveCCW:
                    ArcMove(x, y, z, i, j, k, r, false);
                    break;
                case MachineState.G92_SetPosition:
                    mDev.SetPosition(x, y, z);
                    break;
                case MachineState.D92_Calibration:
                    mDev.Calibrate(x, y, z);
                    break;
                default:
                    Logger.Error("Axis coords received, but machine is not in the right state");
                    break;
            }
        }

        private static void LinearMove(float x, float y, float z)
        {
            if (mDistanceMode == DistanceMode.Absolute)
            {
                if (x == float.MinValue) x = mDev.mCurrentX;
                if (y == float.MinValue) y = mDev.mCurrentY;
                if (z == float.MinValue) z = mDev.mCurrentZ;

                mDev.MoveAbsoluteLinear(x, y, z);
            }
            else
            {
                float absX = mDev.mCurrentX;
                float absY = mDev.mCurrentY;
                float absZ = mDev.mCurrentZ;

                if (x != float.MinValue) absX += x;
                if (y != float.MinValue) absY += y;
                if (z != float.MinValue) absZ += z;

                mDev.MoveAbsoluteLinear(absX, absY, absZ);
            }
        }

        private static void ArcMove(float x, float y, float z, float i, float j, float k, float radius, bool clockwise)
        {
            // Only XY plane supported for now.

            CPointF start = new CPointF(mDev.mCurrentX, mDev.mCurrentY), end;
            ArcInterpolation arc = null;

            if (radius == float.MinValue)
            {   
                // Center format arc.

                if (i == float.MinValue && j == float.MinValue) Error("G2/3: I and J are missing");

                if (i == float.MinValue) i = 0;
                if (j == float.MinValue) j = 0;
                if (x == float.MinValue) x = mDev.mCurrentX;
                if (y == float.MinValue) y = mDev.mCurrentY;
                if (z == float.MinValue) z = mDev.mCurrentZ;

                CPointF center = new CPointF(mDev.mCurrentX + i, mDev.mCurrentY + j);
                end = new CPointF(x, y);

                arc = new ArcInterpolation(start, center, end, clockwise);
            }
            else
            {
                // Radius format arc
                // XYZ are the endpoint. R is the radius. 
                if (x == float.MinValue && y == float.MinValue) Error("G2/3: X and Y are missing");

                if (x == float.MinValue) x = mDev.mCurrentX;
                if (y == float.MinValue) y = mDev.mCurrentY;

                if (mDistanceMode == DistanceMode.Absolute)
                {
                    end = new CPointF(x, y);
                }
                else
                {
                    end = new CPointF(mDev.mCurrentX + x, mDev.mCurrentY + y);
                }

                arc = new ArcInterpolation(start, end, radius, clockwise);
            }


            if (arc == null) Error("G2/3: could not find an arc solution");

            for (float t = ArcStep; t <= 1.0 + (ArcStep / 2); t += ArcStep)
            {
                CPointF target = arc.GetArcPoint(t);

                // Only XY supported
                mDev.MoveAbsoluteLinear(target.X, target.Y, mDev.mCurrentZ);
            }
        }


        // __ Manual Mode _____________________________________________________


        private static void ProcessManualMode()
        {
            
        }


        // __ Helpers _________________________________________________________


        private static void Initialize()
        {
            if (mDev != null) return;

            // Lazy initialization, to allow device registration.

            mDev = DeviceFactory.Get();

            if (mDev == null)
            {
                Logger.Error("Device is not initialized. Can't continue.");
            }
        }

        public static void Error(string msg, params object[] args)
        {
            Logger.Error("On line: " + GCodeParser.Line);
            Logger.Error(msg, args);
#if DESKTOP
            throw new Exception(string.Format("On line: {0}\nError: {1}", 
                GCodeParser.Line, string.Format(msg, args)));
#else
            throw new Exception();
#endif

        }
    }
}
