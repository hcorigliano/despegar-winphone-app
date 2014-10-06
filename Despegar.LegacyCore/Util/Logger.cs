using System;
using System.Diagnostics;


namespace Despegar.LegacyCore.Util
{

    public class Logger
    {

        public static void Info(String msg)
        {
            Debug.WriteLine(msg);
        }

        public static void Warn(String msg)
        {
            Debug.WriteLine("!! " + msg);
        }
    }
}
