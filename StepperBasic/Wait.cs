using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace NetduinoDevice
{
    public class Delay
    {
        private const long TicksPerMicrosecond = TimeSpan.TicksPerMillisecond / 1000;

        public static void Microseconds(uint microSeconds)
        {
            long stopTicks = Utility.GetMachineTime().Ticks +
                (microSeconds * TicksPerMicrosecond);

            while (Utility.GetMachineTime().Ticks < stopTicks) { }
        }



        private static long[] Axes = new long[3];

        /// <summary>
        /// Per-axis delay: don't wait if the last pulse already happened more 
        /// than "delay" ago (most likely moving another axis).
        /// </summary>
        /// <param name="axis">X = 0; Y = 1; Z = 2:</param>
        /// <param name="microSeconds">Maximum microseconds to wait</param>
        public static void Axis(Axis axis, uint microSeconds)
        {
            long increment = microSeconds * TicksPerMicrosecond;
            long targetTicks = Axes[(int)axis] + increment;
            long current;

            while ( (current = Utility.GetMachineTime().Ticks) < targetTicks) { }

            Axes[(int)axis] = current;
        }
    }

    public enum Axis
    { 
        X = 0, 
        Y = 1, 
        Z = 2
    }
}
