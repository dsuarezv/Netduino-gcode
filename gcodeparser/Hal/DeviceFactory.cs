using System;

namespace gcodeparser
{
    public class DeviceFactory
    {
        static Device mInstance = null;

        public static Device Get()
        {
            return mInstance;
        }

        public static void RegisterDevice(Device target)
        {
            mInstance = target;
        }
    }
}
