using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace gcodeparser
{
    public class CncDeviceClient
    {
        private ConnectionState mState;
        private Control mUiControl;

        public event CncDeviceDelegate Connected;
        public event CncDeviceDelegate Disconnected;
        public event CncDeviceDelegate ErrorRaised;
        public event CncDeviceDelegate DataSent;
        public event CncDeviceResponseDelegate DataReceived;

        public CncDeviceClient(string hostName, int port, Control uiControl)
        {
            // El tcp client se conecta directamente. Hay que usar un socket sin más. 

            mUiControl = uiControl;
            mState = new ConnectionState(hostName, port);
            mState.Socket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect()
        {
            mState.Socket.BeginConnect(
                mState.HostName, 
                mState.Port, 
                new AsyncCallback(OnConnect), 
                mState);
        }

        public void Send(string line)
        {
            if (!mState.Socket.Connected) throw new Exception("The device is not connected");

            byte[] buffer = Encoding.UTF8.GetBytes(line);

            mState.Socket.BeginSend(
                buffer, 0, buffer.Length, 
                SocketFlags.None, 
                new AsyncCallback(OnDataSent), mState);
        }

        private void Receive()
        {
            byte[] buffer = new byte[4000];

            mState.Socket.BeginReceive(
                buffer, 0, buffer.Length, 
                SocketFlags.None, new AsyncCallback(OnDataReceived), buffer);
        }

        public void Disconnect()
        {
            if (!mState.Socket.Connected) return;

            mState.Socket.BeginDisconnect(false, new AsyncCallback(OnDisconnect), mState);
        }


        // __ Callbacks _______________________________________________________


        private void InvokeEvent(Delegate d, params object[] args)
        {
            if (mUiControl != null)
            {
                mUiControl.Invoke(d, args);
            }
            else
            {
                d.DynamicInvoke(args);
            }
        }

        private void OnConnect(IAsyncResult ar)
        {
            try
            {
                //mState = (ConnectionState)ar.AsyncState;
                mState.Socket.EndConnect(ar);

                // Start receive call to read welcome message. Is this full duplex capable?
                Receive();

                InvokeEvent(Connected, mState);
            }
            catch (Exception ex)
            {
                NotifyError(ex);
            }
        }

        private void OnDisconnect(IAsyncResult ar)
        {
            try
            {
                mState.Socket.EndDisconnect(ar);

                InvokeEvent(Disconnected, mState);
            }
            catch (Exception ex)
            {
                NotifyError(ex);
            }
        }

        private void OnDataSent(IAsyncResult ar)
        {
            try
            {
                int sentBytes = mState.Socket.EndSend(ar);

                InvokeEvent(DataSent, mState);
            }
            catch (Exception ex)
            {
                NotifyError(ex);
            }
        }

        private void OnDataReceived(IAsyncResult ar)
        {
            try
            {
                int numbytes = mState.Socket.EndReceive(ar);
                byte[] buffer = (byte[])ar.AsyncState;

                string s = Encoding.UTF8.GetString(buffer, 0, numbytes);

                InvokeEvent(DataReceived, s);

                // When disconnecting, a running receive call is finished. 
                // If the socket is then disconnected, we don't want to 
                // receive again, because it starts spinning with 0 bytes 
                // received.
                if (!mState.Socket.Connected) return;

                // Keep receiving
                Receive();
            }
            catch (Exception ex)
            {
                NotifyError(ex);
            }
        }

        private void NotifyError(Exception ex)
        {
            mState.LastError = ex;

            InvokeEvent(ErrorRaised, mState);
        }
    }

    public class ConnectionState
    {
        public string HostName;
        public int Port;
        public Socket Socket;
        public Exception LastError;

        public ConnectionState(string hostName, int port)
        {
            HostName = hostName;
            Port = port;
        }
    }

    public delegate void CncDeviceDelegate(ConnectionState state);

    public delegate void CncDeviceResponseDelegate(string response);
    
}
