using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace gcodeparser
{
    public partial class ManualModeForm : Form
    {
        public ManualModeForm()
        {
            InitializeComponent();
        }

        private MainForm mCommander = null;

        public void SetCommander(MainForm command)
        {
            mCommander = command;
        }

        private void ManualModeForm_KeyDown(object sender, KeyEventArgs e)
        {
            string command = "D10{0}{1}\r\n";
            int steps = e.Shift ? 16 : 160;
            char axis = ' ';

            switch (e.KeyCode)
            {
                case Keys.Up: axis = 'X'; steps = -steps; break;
                case Keys.Down: axis = 'X'; break;
                case Keys.Left: axis = 'Y'; steps = -steps; break;
                case Keys.Right: axis = 'Y';  break;
                case Keys.PageDown: axis = 'Z'; steps = -steps; break;
                case Keys.PageUp: axis = 'Z'; break;
                default: return;
            }

            string finalCommand = string.Format(command, axis, steps);

            if (mCommander.IsDeviceIdle())
            {
                mCommander.EnqueueLine(finalCommand, -1);
                mCommander.SendQueuedLineToDevice();
            }
        }

        private void SetZeroLabel_Click(object sender, EventArgs e)
        {
            mCommander.EnqueueLine("D1", -1);
            mCommander.SendQueuedLineToDevice();

            MessageBox.Show("Machine current location reset to 0, 0, 0");
        }

        private void SetFeedRate_Click(object sender, EventArgs e)
        {
            string result = InputBox.Show("New feed rate (1-230)", "Set feed rate");

            if (result == null) return;

            int feedRate;

            if (!Int32.TryParse(result, out feedRate)) return;

            mCommander.EnqueueLine("F" + feedRate, -1);
            mCommander.SendQueuedLineToDevice();

            SetFeedRate.Text = string.Format("Feed rate set to {0}mm/min", feedRate);
        }
    }
}
