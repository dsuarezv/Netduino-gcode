using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using System.Net.Sockets;
using System.Net;
using System.Text;


namespace NetduinoSocketServer
{
    public class LineServer
    {
        public LineServer()
        {

        }

        public void Listen(int port)
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                try
                {
                    socket.Bind(new IPEndPoint(IPAddress.Any, port));
                    socket.Listen(1);

                    Debug.Print("Netduino is listening on port " + port);

                    while (true)
                    {
                        using (Socket commSocket = socket.Accept())
                        {
                            OnConnect(commSocket);

                            while (true)
                            {
                                Thread.Sleep(1);

                                if (commSocket.Poll(-1, SelectMode.SelectRead))
                                {
                                    int available = commSocket.Available;

                                    if (available == 0) break;   // Disconnect from the other side

                                    byte[] bytes = new byte[available];
                                    int count = commSocket.Receive(bytes);

                                    bool result = Process(bytes, commSocket);

                                    if (!result) break;
                                }
                            }

                            commSocket.Close();
                        }
                    }

                    socket.Close();

                    Debug.Print("Netduino server stopped");
                }
                catch (Exception ex)
                {
                    Debug.Print("ERROR: " + ex.Message + ex.StackTrace);
                }
            }
        }


        private const int MaxBufLen = 255;

        private char[] mRecvBuf = new char[MaxBufLen];
        private int mRecvBufPos = 0;

        private bool Process(byte[] buffer, Socket socket)
        {
            char[] chars = Encoding.UTF8.GetChars(buffer);

            bool result = true;

            foreach (char c in chars)
            {
                switch (c)
                {
                    case '\r':
                    case '\n':
                        if (mRecvBufPos > 0)
                        {
                            // ProcessLine
                            string line = new string(mRecvBuf, 0, mRecvBufPos);
                            result = ProcessLine(line, socket);
                            mRecvBufPos = 0;
                        }
                        break;
                    default:
                        mRecvBuf[mRecvBufPos++] = c;
                        break;
                }
            }

            return result;
        }

        protected virtual void OnConnect(Socket socket)
        {
            
        }

        protected virtual bool ProcessLine(string line, Socket socket)
        {
            //Debug.Print("LINE: " + line);

            if (line.ToLower() == "exit") return false;

            return true;
        }

        protected void SendString(string s, Socket socket)
        {
            byte[] buf = Encoding.UTF8.GetBytes(s);

            int sent = 0;

            while (sent < buf.Length)
            {
                sent += socket.Send(buf, sent, buf.Length - sent, SocketFlags.None);
            }
        }

    }
}
