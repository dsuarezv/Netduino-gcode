using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace NetduinoDevice
{
    public class Delay
    {
        private const long TicksPerMicrosecond = TimeSpan.TicksPerMillisecond / 1000;

        public static void Microseconds(int microSeconds)
        {
            long stopTicks = Utility.GetMachineTime().Ticks +
                (microSeconds * TicksPerMicrosecond);

            while (Utility.GetMachineTime().Ticks < stopTicks) { }
        }
    }
}
