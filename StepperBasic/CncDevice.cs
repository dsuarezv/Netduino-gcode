using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

using gcodeparser;
using NetduinoDevice;

namespace StepperBasic
{

    public class CncDevice: Device
    {
        private const int MaximumXYFeedRate = 670;  // Safe at default driver current setting
        private const int MaximumZFeedRate = 230;  // Safe at default driver current setting


        // __ Port setup ______________________________________________________


        static OutputPort MicroStep1Port = new OutputPort(Pins.GPIO_PIN_D0, false);
        static OutputPort MicroStep2Port = new OutputPort(Pins.GPIO_PIN_D1, false);

        static OutputPort XDirectionPort = new OutputPort(Pins.GPIO_PIN_D10, false);
        static OutputPort YDirectionPort = new OutputPort(Pins.GPIO_PIN_D9, false);
        static OutputPort ZDirectionPort = new OutputPort(Pins.GPIO_PIN_D8, false);

        static OutputPort XStepPort = new OutputPort(Pins.GPIO_PIN_D6, false);
        static OutputPort YStepPort = new OutputPort(Pins.GPIO_PIN_D5, false);
        static OutputPort ZStepPort = new OutputPort(Pins.GPIO_PIN_D4, false);


        // __ Absolute position _______________________________________________


        private int AbsoluteXSteps = 0;
        private int AbsoluteYSteps = 0;
        private int AbsoluteZSteps = 0;
        private uint XYStepInterval = 1500;
        private uint ZStepInterval = 1500;
        private float FeedRate = 125;       // mm per minute

        private float StepsPerMmX = 160.2564f * 2;
        private float StepsPerMmY = 160.3849f * 2;
        private float StepsPerMmZ = 160.97924f * 2;

        //private int XBacklashCompensationSteps = 0;
        //private int YBacklashCompensationSteps = 0;
        //private int ZBacklashCompensationSteps = 0;

        private const bool PositiveAxisDirection = true;
        private const bool NegativeAxisDirection = !PositiveAxisDirection;


        // __ Commands ________________________________________________________


        /// <summary>
        /// Sets the feed rate (cutting head advance speed).
        /// </summary>
        /// <param name="feedRate">Feed rate in milimetes per minute.</param>
        /// <remarks>At 19V and max current, the vexta steppers can handle 670mmperminute in with the current screws.
        /// </remarks>
        public override void SetFeedRate(float mmPerMinute)
        {
            if (mmPerMinute > MaximumXYFeedRate) mmPerMinute = MaximumXYFeedRate;

            FeedRate = mmPerMinute;
            
            XYStepInterval = GetIntervalForFeedRate(mmPerMinute);

            if (mmPerMinute > MaximumZFeedRate) mmPerMinute = MaximumZFeedRate;

            ZStepInterval = GetIntervalForFeedRate(mmPerMinute);
        }

        private uint GetIntervalForFeedRate(float mmPerMinute)
        {
            const uint microsecondsPerMinute = 60000000;

            float stepsPerMinute = mmPerMinute * StepsPerMmX;
            return (uint)(microsecondsPerMinute / stepsPerMinute);
        }

        public void SetZero()
        {
            AbsoluteXSteps = 0;
            AbsoluteYSteps = 0;
            AbsoluteZSteps = 0;
        }

        public string GetLocation()
        {
            return "LOCATION"
                + " X: " + mCurrentX
                + " Y: " + mCurrentY
                + " Z: " + mCurrentZ
                + "\r\n";
        }

        public string GetStatus()
        {
            string s = "STAT"
                + " X:" + AbsoluteXSteps
                + " Y:" + AbsoluteYSteps
                + " Z:" + AbsoluteZSteps
                + " DirX:" + (XDirectionPort.Read() ? '1' : '0')
                + " DirY:" + (YDirectionPort.Read() ? '1' : '0')
                + " MS1:" + (MicroStep1Port.Read() ? '1' : '0')
                + " MS2:" + (MicroStep2Port.Read() ? '1' : '0')
                + " FeedRate: " + XYStepInterval + "ms/step"
                + "\r\n";

            return s;
        }

        public string GetCalibration()
        {
            return "CALIBRATION"
                + " X:" + StepsPerMmX
                + " Y:" + StepsPerMmY
                + " Z:" + StepsPerMmZ;
        }

        public int GetAbsoluteSteps(int steps, OutputPort directionPort)
        {
            bool ms1 = MicroStep1Port.Read();
            bool ms2 = MicroStep2Port.Read();
            bool dir = directionPort.Read();

            int multiplier = 1;

            if (ms1 && ms2) multiplier = 1;
            if (!ms1 && ms2) multiplier = 2;
            if (ms1 && !ms2) multiplier = 4;
            if (!ms1 & !ms2) multiplier = 8;

            if (dir == NegativeAxisDirection) multiplier = -multiplier;

            return steps * multiplier;
        }

        public void SetDirection(bool xDir, bool yDir, bool zDir)
        {
            XDirectionPort.Write(xDir);
            YDirectionPort.Write(yDir);
            ZDirectionPort.Write(!zDir);
        }

        public void SetMicroStepping(bool ms1, bool ms2)
        {
            MicroStep1Port.Write(ms1);
            MicroStep2Port.Write(ms2);
            NetduinoDevice.Delay.Microseconds(1000);
        }

        private void MoveSingleAxis(OutputPort port, OutputPort directionPort, int steps, uint delay)
        {
            SetMovementDirection(ref steps, port, directionPort);

            MoveSingleAxis(port, steps, delay);
        }

        private void MoveSingleAxis(OutputPort port, int steps, uint delay)
        {
            if (port == ZStepPort && delay == 0) delay = ZStepInterval;

            if (delay == 0) delay = XYStepInterval;
            
            for (int i = 0; i < steps; ++i)
            {
                port.Write(true);
                NetduinoDevice.Delay.Microseconds(delay);
                port.Write(false);
                NetduinoDevice.Delay.Microseconds(delay);
            }
        }

        private bool XLastState = false;
        private bool YLastState = false;
        private bool ZLastState = false;

        /*
        public void MoveAxes(int xSteps, int ySteps)
        {
            int max = (xSteps > ySteps) ? xSteps : ySteps;
            uint time = (uint)max * FeedRate;

            MoveAxes(xSteps, ySteps, time);
        }

        public void MoveAxes(int xSteps, int ySteps, uint timeInUs)
        {
            if (xSteps == 0 && ySteps == 0) return;

            // Single axis move

            if (xSteps == 0)
            {
                MoveSingleAxis(YStepPort, ySteps, (uint)FeedRate);
                return;
            }

            if (ySteps == 0)
            {
                MoveSingleAxis(XStepPort, xSteps, (uint)FeedRate);
                return;
            }
            
            // Coordinated move

            uint xstep = timeInUs / (uint)xSteps / 2;
            uint ystep = timeInUs / (uint)ySteps / 2;

            // Proc2
            // This method waits the time needed for the next closest state change. It's
            // pretty accurate and independent of the numbers.

            uint uxstep = (uint)xstep;
            uint uystep = (uint)ystep;

            uint xtime = uxstep;
            uint ytime = uystep;

            uint ellapsed = 0;

            while (ellapsed < timeInUs)
            {
                if (xtime > ytime)
                {
                    NetduinoDevice.Delay.Microseconds(ytime);
                    ellapsed += ytime;
                    xtime -= ytime;
                    YLastState = !YLastState;
                    YStepPort.Write(YLastState);
                    ytime = uystep;
                }
                else
                {
                    NetduinoDevice.Delay.Microseconds(xtime);
                    ellapsed += xtime;
                    ytime -= xtime;
                    XLastState = !XLastState;
                    XStepPort.Write(XLastState);
                    xtime = uxstep;
                }
            }

            AbsoluteXSteps += GetAbsoluteSteps(xSteps, XDirectionPort);
            AbsoluteYSteps += GetAbsoluteSteps(ySteps, YDirectionPort);
        }
         */

        /*
        public void MoveAxes3(int xSteps, int ySteps, int zSteps)
        {
            // 3D line Bresenham

            int dx = xSteps;
            int dy = ySteps;
            int dz = zSteps;

            int x_inc = (dx < 0) ? -1 : 1;
            int y_inc = (dy < 0) ? -1 : 1;
            int z_inc = (dz < 0) ? -1 : 1;

            int Adx = Abs(dx);
            int Ady = Abs(dy);
            int Adz = Abs(dz);

            int dx2 = Adx * 2;
            int dy2 = Ady * 2;
            int dz2 = Adz * 2;

            if ((Adx >= Ady) && (Adx >= Adz))
            {
                int err_1 = dy2 - Adx;
                int err_2 = dz2 - Adx;

                for (int c = 0; c < Adx; ++c)
                {
                    if (err_1 > 0)
                    {
                        YLastState = !YLastState;
                        YStepPort.Write(YLastState);
                        NetduinoDevice.Delay.Microseconds(StepInterval);
                        err_1 -= dx2;
                    }

                    if (err_2 > 0)
                    {
                        ZLastState = !ZLastState;
                        ZStepPort.Write(ZLastState);
                        NetduinoDevice.Delay.Microseconds(StepInterval);
                        err_2 -= dx2;
                    }

                    err_1 += dy2;
                    err_2 += dz2;
                    XLastState = !XLastState;
                    XStepPort.Write(XLastState);
                    NetduinoDevice.Delay.Microseconds(StepInterval);
                }
            }

            if ((Ady > Adx) && (Ady >= Adz))
            {
                int err_1 = dx2 - Ady;
                int err_2 = dz2 - Ady;

                for (int c = 0; c < Ady; ++c)
                {
                    if (err_1 > 0)
                    {
                        XLastState = !XLastState;
                        XStepPort.Write(XLastState);
                        NetduinoDevice.Delay.Microseconds(StepInterval);
                        err_1 -= dy2;
                    }

                    if (err_2 > 0)
                    {
                        ZLastState = !ZLastState;
                        ZStepPort.Write(ZLastState);
                        NetduinoDevice.Delay.Microseconds(StepInterval);
                        err_2 -= dy2;
                    }

                    err_1 += dx2;
                    err_2 += dz2;
                    YLastState = !YLastState;
                    YStepPort.Write(YLastState);
                    NetduinoDevice.Delay.Microseconds(StepInterval);
                }
            }

            if ((Adz > Adx) && (Adz > Ady))
            {
                int err_1 = dy2 - Adz;
                int err_2 = dx2 - Adz;

                for (int c = 0; c < Adz; ++c)
                {
                    if (err_1 > 0)
                    {
                        YLastState = !YLastState;
                        YStepPort.Write(YLastState);
                        NetduinoDevice.Delay.Microseconds(StepInterval);
                        err_1 -= dz2;
                    }

                    if (err_2 > 0)
                    {
                        XLastState = !XLastState;
                        XStepPort.Write(XLastState);
                        NetduinoDevice.Delay.Microseconds(StepInterval);
                        err_2 -= dz2;
                    }

                    err_1 += dy2;
                    err_2 += dx2;
                    ZLastState = !ZLastState;
                    ZStepPort.Write(ZLastState);
                    NetduinoDevice.Delay.Microseconds(StepInterval);
                }
            }
        }
        */

        public void MoveAxes4(int xSteps, int ySteps, int zSteps)
        {
            // 3D line Bresenham

            int dx = xSteps;
            int dy = ySteps;
            int dz = zSteps;

            int x_inc = (dx < 0) ? -1 : 1;
            int y_inc = (dy < 0) ? -1 : 1;
            int z_inc = (dz < 0) ? -1 : 1;

            int Adx = Abs(dx);
            int Ady = Abs(dy);
            int Adz = Abs(dz);

            int dx2 = Adx * 2;
            int dy2 = Ady * 2;
            int dz2 = Adz * 2;

            if ((Adx >= Ady) && (Adx >= Adz))
            {
                int err_1 = dy2 - Adx;
                int err_2 = dz2 - Adx;

                for (int c = 0; c < Adx; ++c)
                {
                    if (err_1 > 0)
                    {
                        YLastState = !YLastState;
                        YStepPort.Write(YLastState);
                        Delay.Axis(Axis.Y, XYStepInterval);
                        err_1 -= dx2;
                    }

                    if (err_2 > 0)
                    {
                        ZLastState = !ZLastState;
                        ZStepPort.Write(ZLastState);
                        Delay.Axis(Axis.Z, ZStepInterval);
                        err_2 -= dx2;
                    }

                    err_1 += dy2;
                    err_2 += dz2;
                    XLastState = !XLastState;
                    XStepPort.Write(XLastState);
                    Delay.Axis(Axis.X, XYStepInterval);
                }
            }

            if ((Ady > Adx) && (Ady >= Adz))
            {
                int err_1 = dx2 - Ady;
                int err_2 = dz2 - Ady;

                for (int c = 0; c < Ady; ++c)
                {
                    if (err_1 > 0)
                    {
                        XLastState = !XLastState;
                        XStepPort.Write(XLastState);
                        Delay.Axis(Axis.X, XYStepInterval);
                        err_1 -= dy2;
                    }

                    if (err_2 > 0)
                    {
                        ZLastState = !ZLastState;
                        ZStepPort.Write(ZLastState);
                        Delay.Axis(Axis.Z, ZStepInterval);
                        err_2 -= dy2;
                    }

                    err_1 += dx2;
                    err_2 += dz2;
                    YLastState = !YLastState;
                    YStepPort.Write(YLastState);
                    Delay.Axis(Axis.Y, XYStepInterval);
                }
            }

            if ((Adz > Adx) && (Adz > Ady))
            {
                int err_1 = dy2 - Adz;
                int err_2 = dx2 - Adz;

                for (int c = 0; c < Adz; ++c)
                {
                    if (err_1 > 0)
                    {
                        YLastState = !YLastState;
                        YStepPort.Write(YLastState);
                        Delay.Axis(Axis.Y, XYStepInterval);
                        err_1 -= dz2;
                    }

                    if (err_2 > 0)
                    {
                        XLastState = !XLastState;
                        XStepPort.Write(XLastState);
                        Delay.Axis(Axis.X, XYStepInterval);
                        err_2 -= dz2;
                    }

                    err_1 += dy2;
                    err_2 += dx2;
                    ZLastState = !ZLastState;
                    ZStepPort.Write(ZLastState);
                    Delay.Axis(Axis.Z, ZStepInterval);
                }
            }
        }

        private int Abs(int val)
        {
            return (val < 0) ? -val : val;
        }


        // __ Device implementation ___________________________________________


        public override void MoveRawX(int steps)
        {
            MoveSingleAxis(XStepPort, XDirectionPort, steps, (uint)XYStepInterval);
        }

        public override void MoveRawY(int steps)
        {
            MoveSingleAxis(YStepPort, YDirectionPort, steps, (uint)XYStepInterval);
        }

        public override void MoveRawZ(int steps)
        {
            MoveSingleAxis(ZStepPort, ZDirectionPort, steps, (uint)XYStepInterval);
        }

        public override void Calibrate(float x, float y, float z)
        {
            if (x > float.MinValue) StepsPerMmX = x;
            if (y > float.MinValue) StepsPerMmY = y;
            if (z > float.MinValue) StepsPerMmZ = z;

            Print(GetCalibration());
        }

        public override void SetPosition(float x, float y, float z)
        {
            mCurrentX = x;
            mCurrentY = y;
            mCurrentZ = z;
        }

        public override void MoveAbsoluteLinear(float x, float y, float z)
        {
            float xRelative = x - mCurrentX;
            float yRelative = y - mCurrentY;
            float zRelative = z - mCurrentZ;

            int xRelativeSteps = (int)(xRelative * StepsPerMmX);
            int yRelativeSteps = (int)(yRelative * StepsPerMmY);
            int zRelativeSteps = (int)(zRelative * StepsPerMmZ);

            SetMovementDirection(ref xRelativeSteps, XStepPort, XDirectionPort);
            SetMovementDirection(ref yRelativeSteps, YStepPort, YDirectionPort);
            SetMovementDirection(ref zRelativeSteps, ZStepPort, ZDirectionPort);

            MoveAxes4(xRelativeSteps, yRelativeSteps, zRelativeSteps);

            mCurrentX = x;
            mCurrentY = y;
            mCurrentZ = z;
        }

        private void SetMovementDirection(ref int steps, OutputPort stepPort, OutputPort directionPort)
        {
            if (steps == 0) return;

            //bool previousDirection = directionPort.Read();

            if (steps < 0)
            {
                steps = -steps;
                SetMovementDirection(directionPort, NegativeAxisDirection);
            }
            else
            {
                SetMovementDirection(directionPort, PositiveAxisDirection);
            }

            // NetduinoDevice.Delay.Microseconds(1000);

            // Backlash compensation

            //bool newDirection = directionPort.Read();

            //if (previousDirection != newDirection)
            //{
            //    MoveSingleAxis(stepPort, XBacklashCompensationSteps, 0);
            //}
        }

        private void SetMovementDirection(OutputPort port, bool direction)
        {
            if (port == ZDirectionPort) direction = !direction;
            if (port == XDirectionPort) direction = !direction;

            port.Write(direction);
        }

        public override void Print(string s)
        {
            Debug.Print(s);
        }

    }
}
