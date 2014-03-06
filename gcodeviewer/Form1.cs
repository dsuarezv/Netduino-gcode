using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

using Druid.Viewer;

namespace gcodeparser
{
    public partial class MainForm : Form
    {
        private ViewerDevice mViewDevice;
        private Canvas m2dTargetControl;
        private Viewer3d m3dTargetControl;
        private ConfigurationFile mConfig;
        bool mLoading = false;

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            InitConfig();

            //Setup2dViewer();
            Setup3dViewer();

            ProcessFile(GetConfig().GetString("last.open.file"));
        }

        private void InitConfig()
        {
            try
            {
                string path = Path.GetDirectoryName(Application.ExecutablePath);

                mConfig = new ConfigurationFile(Path.Combine(path, "gcodeviewer.ini"));
                mConfig.Load();
            }
            catch
            {  }
        }

        private ConfigurationSection GetConfig()
        { 
            return mConfig.GetSection("");
        }

        private void Setup2dViewer()
        {
            m2dTargetControl = new Canvas();
            m2dTargetControl.Dock = DockStyle.Fill;

            this.RenderPanel.Controls.Add(m2dTargetControl);

            mViewDevice = new ViewerDevice(m2dTargetControl);
            mViewDevice.Initialize();
            mViewDevice.CodeChanged += new EventHandler(mDevice_CodeChanged);

            DeviceFactory.RegisterDevice(mViewDevice);            
        }

        private void Setup3dViewer()
        {
            m3dTargetControl = new Viewer3d();
            m3dTargetControl.Dock = DockStyle.Fill;
            m3dTargetControl.BackColor = RenderPanel.BackColor;
            this.RenderPanel.Controls.Add(m3dTargetControl);

            mViewDevice = new Viewer3dDevice(m3dTargetControl);
            mViewDevice.Initialize();
            mViewDevice.CodeChanged += new EventHandler(mDevice_CodeChanged);

            DeviceFactory.RegisterDevice(mViewDevice);
        }

        private void SetLoading(bool val)
        {
            mLoading = val;

            if (!mLoading) mDevice_CodeChanged(null, EventArgs.Empty);
        }

        private void ProcessFile(string file)
        {
            if (file == null) return;

            if (!File.Exists(file))
            {
                MessageBox.Show("File not found: " + file);
                return;
            }

            try
            {
                StreamReader reader = new StreamReader(file);

                string line;

                SetLoading(true);

                mViewDevice.Clear();

                while ((line = reader.ReadLine()) != null)
                {
                    GCodeParser.ParseLine(line);
                }

                SetLoading(false);

                reader.Close();

                ZoomTextbox.Text = mViewDevice.VisualScale.ToString();

                SetupJob();

                RenderPanel.Invalidate();

                FileNameLabel.Text = Path.GetFileName(file);

                GetConfig().SetString("last.open.file", file);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupJob()
        { 
            float distance = mViewDevice.GetTotalDistance();
            float mmPerMinute = ViewerDevice.MmPerMinute;

            TotalDistanceTextbox.Text = string.Format("{0:0.0} mm", distance);
            EstimatedTimeTextbox.Text = string.Format("{0:0.0} minutes", distance / mmPerMinute);
        }

        void mDevice_CodeChanged(object sender, EventArgs e)
        {
            if (mLoading) return;

            int total = mViewDevice.CodeLines.Count;

            if (total == 0) total = 1;

            CurrentLineTrackbar.Minimum = 0;
            CurrentLineTrackbar.Maximum = total - 1;
            CurrentLineTrackbar.Value = 0;
            mViewDevice.CurrentCodeLineIndex = 0;
            //agaProgress1.Maximum = (int)mViewDevice.GetTotalDistance();
        }

        private void StepTrackbar_Scroll(object sender, EventArgs e)
        {
            int i = CurrentLineTrackbar.Value;

            mViewDevice.CurrentCodeLineIndex = i;
            
            string currentLineText = string.Format("{0}: {1}", i + 1, mViewDevice.CodeLines[i].GCodeLine);
            
            CurrentLineTextbox.Text = currentLineText;

            agaProgress1.Value = (int)mViewDevice.CurrentDistance;
        }

        private void ZoomTextbox_TextChanged(object sender, EventArgs e)
        {
            float scale;

            if (float.TryParse(ZoomTextbox.Text, out scale))
            {
                if (scale > 0f)
                {
                    mViewDevice.VisualScale = scale;
                }
            }
        }

        private void FileNameLabel_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ProcessFile(openFileDialog1.FileName);
            }
        }        

        private void SpindleDiameterTextbox_TextChanged(object sender, EventArgs e)
        {
            float d;

            if (float.TryParse(SpindleDiameterTextbox.Text, out d))
            {
                if (d > 15) return;

                mViewDevice.ToolDiameter = d;
            }
        }

        private void CloseLabel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CheckMathLabel_Click(object sender, EventArgs e)
        {
            new MathTestForm().Show();
        }


        // __ Device connection _______________________________________________


        private CncDeviceClient mTcpDevice;
        private Queue<CodeLine> mCodeLinesQueue = new Queue<CodeLine>();
        private bool mIsIdle = true;

        private void Connect()
        {
            if (mTcpDevice != null) Disconnect();

            string cncMachine = GetConfig().GetString("cnc.address");
            int cncPort = GetConfig().GetInt("cnc.port", 82);

            Assert.IsNotNull(cncMachine, "cnc.address parameter not defined");

            mTcpDevice = new CncDeviceClient(cncMachine, cncPort, this);
            mTcpDevice.Connected += new CncDeviceDelegate(mTcpDevice_Connected);
            mTcpDevice.Disconnected += new CncDeviceDelegate(mTcpDevice_Disconnected);
            mTcpDevice.DataSent += new CncDeviceDelegate(mTcpDevice_DataSent);
            mTcpDevice.DataReceived += new CncDeviceResponseDelegate(mTcpDevice_DataReceived);
            mTcpDevice.ErrorRaised += new CncDeviceDelegate(mTcpDevice_ErrorRaised);

            ConnectionStatusLabel.Text = "Connecting...";
            mTcpDevice.Connect();
        }

        private void Disconnect()
        {
            if (mTcpDevice == null) return;
            
            mTcpDevice.Send("exit\r\n");
            
            mTcpDevice.Disconnect();
            mTcpDevice = null;
        }

        internal void EnqueueLine(string line, int index)
        {
            if (line == null) return;

            mCodeLinesQueue.Enqueue(new CodeLine(line, index));

            mIsIdle = false;
        }

        internal void SendQueuedLineToDevice()
        {
            if (mCodeLinesQueue.Count == 0)
            {
                mIsIdle = true;
                SetMachineStatus("Stopped", Color.White);
                PlayButton.Enabled = true;
                CancelMachiningButton.Enabled = false;
                return;
            }

            CodeLine currentLine = mCodeLinesQueue.Dequeue();

            CurrentLineTextbox.Text = currentLine.Line;
            mViewDevice.CurrentCodeLineIndex = currentLine.Index;

            mTcpDevice.Send(currentLine.Line + "\n");
        }

        internal bool IsDeviceIdle()
        {
            return mIsIdle;
        }

        private void Cancel()
        {
            mCodeLinesQueue.Clear();

            SetMachineStatus("Cancelling...", Color.Salmon);

            PlayButton.Enabled = true;
            CancelMachiningButton.Enabled = false;
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            Connect();
        }

        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            Disconnect();
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            PlayButton.Enabled = false;
            CancelMachiningButton.Enabled = true;

            SetMachineStatus("Machining", Color.Red);

            // Enqueue any calibration line here (maybe from the config file)
            EnqueueLine(GetConfig().GetString("calibration.command"), -1);

            int i = 0;

            // Fill pending code queue
            foreach (ViewerLine l in mViewDevice.CodeLines)
            {
                EnqueueLine(l.GCodeLine, i++);
            }

            // Send first line to launch the process
            SendQueuedLineToDevice();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Cancel();
        }


        // __ Device events ___________________________________________________


        void mTcpDevice_Connected(ConnectionState state)
        {
            UpdateState(state);
        }

        void mTcpDevice_Disconnected(ConnectionState state)
        {
            UpdateState(state);
            SetMachineStatus("Stopped", Color.White);

            if (mTcpDevice == null) return;

            mTcpDevice.Connected -= new CncDeviceDelegate(mTcpDevice_Connected);
            mTcpDevice.Disconnected -= new CncDeviceDelegate(mTcpDevice_Disconnected);
            mTcpDevice.DataSent -= new CncDeviceDelegate(mTcpDevice_DataSent);
            mTcpDevice.DataReceived -= new CncDeviceResponseDelegate(mTcpDevice_DataReceived);
            mTcpDevice.ErrorRaised -= new CncDeviceDelegate(mTcpDevice_ErrorRaised);

            mTcpDevice = null;
        }       

        void mTcpDevice_DataSent(ConnectionState state)
        {
            //UpdateState(state);
        }

        void mTcpDevice_DataReceived(string receivedString)
        {
            if (receivedString == "+\r\n")   // + is confirmation
            {
                SendQueuedLineToDevice();
            }
        }

        void mTcpDevice_ErrorRaised(ConnectionState state)
        {
            UpdateState(state);

            MessageBox.Show(state.LastError.Message, "Device error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void UpdateState(ConnectionState state)
        {
            if (state.Socket.Connected)
            {
                SetOnlineState();
            }
            else
            {
                SetOfflineState();
            }
        }

        private void SetOnlineState()
        {
            ConnectionStatusLabel.ForeColor = System.Drawing.Color.LightGreen;
            ConnectionStatusLabel.Text = "Online";
            ConnectButton.Enabled = false;
            DisconnectButton.Enabled = true;
            PlayButton.Enabled = true;
            CancelMachiningButton.Enabled = false;
        }

        private void SetOfflineState()
        {
            ConnectionStatusLabel.Text = "Offline";
            ConnectionStatusLabel.ForeColor = ConnectionStatusLabel.Parent.ForeColor;
            ConnectButton.Enabled = true;
            DisconnectButton.Enabled = false;
            PlayButton.Enabled = false;
            CancelMachiningButton.Enabled = false;
        }

        private void SetMachineStatus(string caption, Color color)
        {
            MachineStatusLabel.Text = caption;
            MachineStatusLabel.ForeColor = color;
        }

        private void ManualModeButton_Click(object sender, EventArgs e)
        {
            Cancel();

            ManualModeForm f = new ManualModeForm();
            f.SetCommander(this);
            f.ShowDialog();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mConfig == null) return;

            try
            {
                mConfig.Save();
            }
            catch
            { }
        }
    }



    public class CodeLine
    {
        public string Line;
        public int Index;

        public CodeLine(string line, int index)
        {
            Line = line;
            Index = index;
        }
    }
}
