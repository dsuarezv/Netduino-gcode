using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace NetduinoDevice
{
    public class Program
    {
        private static OutputPort LED = new OutputPort(Pins.ONBOARD_LED, false);
        private static NetduinoAxis XAxis;
        private static NetduinoAxis YAxis;

        public static void Main()
        {
            Initialize();

            XAxis.MoveAxisLinear(true, 1, 100);
            YAxis.MoveAxisLinear(true, 1, 100);

            Thread.Sleep(20000);

            Cleanup();
        }

        private static void Initialize()
        {
            AxisConfiguration XAxisConfig = new AxisConfiguration();
            AxisConfiguration YAxisConfig = new AxisConfiguration();

            XAxisConfig.Name = "X";
            XAxisConfig.DirectionPin = Pins.GPIO_PIN_D10;
            XAxisConfig.StepPin = Pins.GPIO_PIN_D6;
            XAxisConfig.StepsPerMillimeter = 200;

            YAxisConfig.Name = "Y";
            YAxisConfig.DirectionPin = Pins.GPIO_PIN_D9;
            YAxisConfig.StepPin = Pins.GPIO_PIN_D5;
            YAxisConfig.StepsPerMillimeter = 400;

            XAxis = new NetduinoAxis(XAxisConfig);
            YAxis = new NetduinoAxis(YAxisConfig);

            LED.Write(false);
        }

        private static void Cleanup()
        {
            XAxis.Abort();
            YAxis.Abort();

            LED.Write(true);
        }

    }
}
