using System;
using System.Threading;

using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace NetduinoDevice
{
    public class NetduinoAxis
    {
        
        // __ 
        

        private Thread mWorker;
        private bool mInProgress = false;
        private AxisConfiguration mAxisConfig;
        

        // __ Location __________________________________________________________________


        public double GlobalPosition = 0;


        // __ Movement __________________________________________________________________

        private int mTargetSteps = 0;
        private int mTargetSpeed = 10;
        private bool mTargetDirection = false;
        
        public int CurrentSteps = 0;
        public int RemainingTime = 0;
        

        // __ Hardware __________________________________________________________________


        private OutputPort mDirectionPort;
        private OutputPort mStepPort;


        // __ Public API ________________________________________________________________

        /// <summary>
        /// Creates a new linear axis controller. The axis runs in it's own thread
        /// so it can be moved in parallel to other axes. 
        /// DAVE: This is a linear interpolator. If you need sin or cos, this approach is not enough. 
        /// </summary>
        /// <param name="axisConfig"></param>
        public NetduinoAxis(AxisConfiguration axisConfig)
        {
            mAxisConfig = axisConfig;

            SetupWorker();

            SetupHardware();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="millimeters"></param>
        /// <param name="speed">Number of steps per second. Software limit is around 50000 (20us per cycle), 
        ///                     but the motor will have a much lower limit.</param>
        /// <returns></returns>
        public bool MoveAxisLinear(bool direction, double millimeters, int speed)
        {
            if (mInProgress)
            {
                Logger.Log("Axis " + mAxisConfig.Name + " is already moving, wait till it finishes");
                return false;
            }

            if (!mWorker.IsAlive)
            {
                Logger.Log("Axis " + mAxisConfig.Name + " worker was dead. Relaunch needed");
                SetupWorker();
            }

            int steps = (int)(mAxisConfig.StepsPerMillimeter * millimeters);

            mTargetDirection = direction;
            mTargetSpeed = speed;
            mTargetSteps = steps;

            return true;
        }

        /// <summary>
        /// Moves the axis synchronously (blocks till the op finishes)
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="millimeters"></param>
        /// <param name="speed">Number of steps per second. Software limit is around 50000 (20us per cycle), 
        ///                     but the motor will have a much lower limit.</param>
        /// <returns></returns>
        public bool MoveAxisLinearSync(bool direction, double millimeters, int speed)
        {
            if (mInProgress)
            {
                Logger.Log("Axis " + mAxisConfig.Name + " is already moving, wait till it finishes");
                return false;
            }

            int steps = (int)(mAxisConfig.StepsPerMillimeter * millimeters);

            Step(direction, steps, speed);

            return true;
        }

        /// <summary>
        /// Aborts the current linear movement.
        /// </summary>
        public void Abort()
        {
            if (mWorker.IsAlive) mWorker.Abort();
        }


        // __ Internal Impl _____________________________________________________________


        private void SetupHardware()
        {
            mDirectionPort = new OutputPort(mAxisConfig.DirectionPin, false);
            mStepPort = new OutputPort(mAxisConfig.StepPin, false);
        }

        private void SetupWorker()
        {
            mWorker = new Thread(new ThreadStart(WorkerProc));
            mWorker.Start();
        }

        private void WorkerProc()
        {
            try
            {
                while (true)
                {
                    if (mTargetSteps != 0)
                    {
                        Step(mTargetDirection, mTargetSteps, mTargetSpeed);
                    }
                    Thread.Sleep(1);
                }
            }
            catch (ThreadAbortException)
            {
                Debug.Print(mAxisConfig.Name + " axis aborted");
            }
        }

        private void Step(bool direction, int steps, int stepsPerSecond)
        {
            mInProgress = true;
            
            CurrentSteps = 0;
            
            int delay = 500000 / stepsPerSecond;      // Microseconds resolution
            int delay2 = delay * 2;

            // Set direction

            mDirectionPort.Write(direction);
            NetduinoDevice.Delay.Microseconds(50000);

            // Send number of steps as pulses. 

            for (int i = 0; i < steps; ++i)
            {
                mStepPort.Write(true);
                NetduinoDevice.Delay.Microseconds(delay);
                mStepPort.Write(false);
                NetduinoDevice.Delay.Microseconds(delay);
                
                // Update stats
                CurrentSteps++;
                RemainingTime = delay2 * (steps - i);
            }

            mTargetSteps = 0;
            mInProgress = false;
        }

    }
}
