using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace NetduinoDevice
{
    public class Configuration
    {
        
    }

    public class AxisConfiguration
    {
        public string Name;

        public int StepsPerMillimeter;

        public Cpu.Pin StepPin;

        public Cpu.Pin DirectionPin;
    }
}
