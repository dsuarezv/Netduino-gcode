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
    public partial class InputBox : Form
    {
        public InputBox()
        {
            InitializeComponent();
        }

        public static string Show(string Text, string caption)
        {
            using (InputBox b = new InputBox())
            {
                b.TextLabel.Text = Text;
                b.Text = caption;

                if (b.ShowDialog() == DialogResult.OK)
                {
                    return b.InputTextBox.Text;
                }
            }

            return null;
        }
    }
}
