using Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Profitbricks
{
    public class Constants
    {
        public const string PB_USERNAME = "PB_USERNAME";
        public const string PB_PASSWORD = "PB_PASSWORD";
    }

    public class Utilities
    {

        public static Configuration Configuration;

        public static void CheckCreds()
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(Constants.PB_USERNAME, EnvironmentVariableTarget.User)))
                throw new Exception("PB_USERNAME is not set. Please run 'Set-Profitbricks'");
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(Constants.PB_PASSWORD, EnvironmentVariableTarget.User)))
                throw new Exception("PB_PASSWORD is not set. Please run 'Set-Profitbricks'");
        }

      public static String SecureStringToString(SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

    }

    public class StringValue : System.Attribute
    {
        private string _value;

        public StringValue(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }

    }
}
