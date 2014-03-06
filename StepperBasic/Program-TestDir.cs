using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace StepperBasic
{
    public class Program
    {
        static OutputPort mstep1 = new OutputPort(Pins.GPIO_PIN_D6, false);
        static OutputPort mdir1 = new OutputPort(Pins.GPIO_PIN_D10, false);
        
        static OutputPort mstep2 = new OutputPort(Pins.GPIO_PIN_D5, false);
        static OutputPort mdir2 = new OutputPort(Pins.GPIO_PIN_D9, false);

        public static void Main_()
        {
            while (true)
            {
                
            }
        }



    }

    public class Axis
    {
        public string Name;
        public OutputPort StepPort;
        public OutputPort DirPort;

        public Axis(string name, Cpu.Pin stepPin, Cpu.Pin dirPin)
        {
            Name = name;
            StepPort = new OutputPort(stepPin, false);
            DirPort = new OutputPort(dirPin, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="steps">Number of steps to perform</param>
        /// <param name="speed">Steps per second</param>
        /// <param name="dir">forward: true, backwards: false</param>
        public void Move(int steps, int speed, bool dir)
        {
            DirPort.Write(dir);

            
        }
        
    }
}
