using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace StepperBasic
{
    public class Program2
    {
        static OutputPort mp1 = new OutputPort(Pins.GPIO_PIN_D4, false);
        static OutputPort mp2 = new OutputPort(Pins.GPIO_PIN_D3, false);

        public static void Main_()
        {

            int interval = 5;

            while (true)
            {
                Thread.Sleep(interval);
                SetPorts(true);
                Thread.Sleep(interval);
                SetPorts(false);
            }
        }

        private static void SetPorts(bool value)
        {
            mp1.Write(value);
            mp2.Write(value);
        }

    }
}
