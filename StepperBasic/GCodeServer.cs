using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using System.Net.Sockets;

using NetduinoSocketServer;
using gcodeparser;

namespace StepperBasic
{
    public class GCodeServer: LineServer
    {
        // __ Internals _______________________________________________________

        
        static CncDevice mDevice = new CncDevice();

        
        // __ Port setup ______________________________________________________


        static OutputPort LED = new OutputPort(Pins.ONBOARD_LED, true);


        // __ Server impl _____________________________________________________

        
        public static void Main()
        {
            DeviceFactory.RegisterDevice(mDevice);

            new GCodeServer().Listen(82);
        }

        protected override void OnConnect(Socket socket)
        {
            SendString("Dave's CNC interface 0.70 with G-Code\r\n", socket);
        }

        protected override bool ProcessLine(string line, Socket socket)
        {
            string l = line.ToLower().Trim();

            if (l == "exit") return false;

            LED.Write(false);

            try
            {
                GCodeParser.ParseLine(l);

                SendString("+\r\n", socket);
            }
            catch (Exception ex)
            {
                SendString("ERROR: " + ex.Message + "\r\n", socket);
            }
            finally
            {
                LED.Write(true);
            }

            return true;
        }
    }
}
