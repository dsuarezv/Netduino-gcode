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
    public partial class MathTestForm : Form
    {
        private float Tolerance = 0.00001f;
        private StringBuilder mResult = new StringBuilder();

        public MathTestForm()
        {
            InitializeComponent();
        }

        private void MathTestForm_Load(object sender, EventArgs e)
        {
            mResult.AppendFormat("Tolerance: {0}\r\n", Tolerance);

            RunTest();

            ResultTextbox.Text = mResult.ToString();
        }

        private void RunTest()
        {
            TestCos();
            TestSin();
            TestAtan2();
            TestSqrt();
        }

        private void TestSqrt()
        {
            mResult.Append("Testing Sqrt function\r\n");

            for (float x = 0; x < 2000; x += 3.243f)
            {
                Check((float)Math.Sqrt(x), Math2.Sqrt(x), "Sqrt: {0} != {1}");
            }
        }

        private void TestCos()
        {
            mResult.Append("Testing Cos function\r\n");

            for (float x = 0; x < 10f; x += 0.021f)
            {
                Check((float)Math.Cos(x), Math2.Cos(x), "Cos: {0} != {1}");
            }
        }


        private void TestSin()
        {
            mResult.Append("Testing Sin function\r\n");

            for (float x = 0; x < 10f; x += 0.021f)
            {
                Check((float)Math.Sin(x), Math2.Sin(x), "Sin: {0} != {1}");
            }
        }


        private void TestAtan2()
        {
            mResult.Append("Testing Atan2 function\r\n");

            int correct = 0;
            int total = 0;

            for (float x = 0; x < 5f; x += 0.11f)
            {
                for (float y = -20f; y < 20f; y += 0.105f)
                {
                    float m = (float)Math.Atan2(y, x);
                    float m2 = Math2.Atan2(y, x);

                    const float rad = 180 / 3.141592654f;

                    float dm = m * rad;
                    float dm2 = m2 * rad;

                    if (Check(x, y, dm, dm2, "Atan2: x {0}, y {1} | {2} != {3}")) correct++;

                    total++;
                }
            }

            mResult.AppendFormat("Atan2: Total: {0}, OK: {1}\r\n", total, correct);
        }


        private bool Check(float v1, float v2, string errorMsg)
        {
            float diff = (float)Math.Abs(v1 - v2);

            if (diff > Tolerance)
            {
                PrintError(v1, v2, errorMsg);
                return false;
            }

            return true;
        }

        private bool Check(float x, float y, float v1, float v2, string errorMsg)
        {
            float diff = (float)Math.Abs(v1 - v2);

            if (diff > Tolerance)
            {
                PrintError(x, y, v1, v2, errorMsg);
                return false;
            }

            return true;
        }        

        private void PrintError(float x, float y, float v1, float v2, string message)
        {
            mResult.AppendFormat(message + "\r\n", x, y, v1, v2);
        }

        private void PrintError(float v1, float v2, string message)
        {
            //MessageBox.Show(string.Format(message, v1, v2));

            mResult.AppendFormat(message + "\r\n", v1, v2);
        }
    }
}
