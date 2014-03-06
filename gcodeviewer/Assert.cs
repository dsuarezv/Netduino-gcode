using System;
using System.Collections;

namespace gcodeparser
{
    /// <summary>
    /// Util used to check parameters
    /// </summary>
    public class Assert
    {
        // Localization infraestructure can be applied here when available.

        public static void IsNotNull(object target, string exceptionMessageFormat, params object[] par)
        {
            if (target == null)
            {
                throw new Exception(string.Format(exceptionMessageFormat, par));
            }
        }

        public static void IsTrue(bool condition, string exceptionMessageFormat, params object[] par)
        {
            if (!condition)
            {
                throw new Exception(string.Format(exceptionMessageFormat, par));
            }
        }

        public static void IsUsableString(string target, string exceptionMessageFormat, params object[] par)
        {
            if (target == null || target == string.Empty)
            {
                throw new Exception(string.Format(exceptionMessageFormat, par));
            }
        }

        public static void IsUsableCollection(IList target, string exceptionMessageFormat, params object[] par)
        {
            if (target == null || target.Count == 0)
            {
                throw new Exception(string.Format(exceptionMessageFormat, par));
            }
        }
    }
}
