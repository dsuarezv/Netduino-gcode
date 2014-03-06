using System;

namespace gcodeparser
{
    public abstract class Device
    {
        public float mCurrentX;
        public float mCurrentY;
        public float mCurrentZ;

        public void ResetAxis()
        {
            mCurrentX = 0;
            mCurrentY = 0;
            mCurrentZ = 0;
        }

        public abstract void MoveAbsoluteLinear(float x, float y, float z);
        public abstract void Print(string s);
        public abstract void MoveRawX(int steps);
        public abstract void MoveRawY(int steps);
        public abstract void MoveRawZ(int steps);
        public abstract void Calibrate(float xStepsPerMm, float yStepsPerMm, float zStepsPerMm);
        public abstract void SetPosition(float x, float y, float z);
        public abstract void SetFeedRate(float mmPerMinute);
    }
}
