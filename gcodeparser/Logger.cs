using System;
using Microsoft.SPOT;

namespace gcodeparser
{
    public class Logger
    {
        public static void Log(string msg, params object[] args)
        {
            //string s = string.Format(msg, args);

            //Debug.Print(s);
        }

        public static void Error(string msg, params object[] args)
        {
            //string s = string.Format(msg, args);

            //Debug.Print(msg);
        }
    }
}
