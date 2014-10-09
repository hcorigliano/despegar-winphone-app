using System;
namespace Despegar.Core.Log
{
    public static class Logger
    {
        private const string CORE_PREFIX = "[CORE]";
        private const string APP_PREFIX = "[APP]";
        private const string EXCEPTION_PREFIX = "[APP:EXCEPTION]";
        private const string CORE_EXCEPTION_PREFIX = "[CORE:EXCEPTION]";
        private static bool verboseExceptions; // Exceptions are logged with more details
        private static bool loggingEnabled;

        /// <summary>
        /// Enables / Disables Logger options (Enabled logging, Verbose mode)
        /// </summary>
        public static void Configure(bool logEnabled, bool verboseLogging)
        {
            verboseExceptions = verboseLogging;
            loggingEnabled = logEnabled;
        }

        /// <summary>
        /// Method for logging Application level events
        /// </summary>
        /// <param name="message"></param>
        public static void Log(string message) 
        {
            if (loggingEnabled)
                DoLog(message, APP_PREFIX);
        }

        /// <summary>
        /// Method for logging Core level events
        /// </summary>
        /// <param name="message"></param>
        public static void LogCore(string message) 
        {
            if (loggingEnabled)
               DoLog(message, CORE_PREFIX);
        }

        /// <summary>
        /// Method for logging Core thrown exceptions 
        /// </summary>
        /// <param name="message"></param>
        public static void LogException(Exception ex)
        {
            if (loggingEnabled)
               DoLog(GetExceptionMessage(ex), EXCEPTION_PREFIX);
        }

        /// <summary>
        /// Method for logging Core error events
        /// </summary>
        /// <param name="message"></param>
        public static void LogCoreException(Exception ex)
        {
            if (loggingEnabled)
                DoLog(GetExceptionMessage(ex), CORE_EXCEPTION_PREFIX);
        }

        private static string GetExceptionMessage(Exception ex)
        {
            return verboseExceptions ? ex.Message + " | " +
                ex.ToString() + " | InnerException: " +
                (ex.InnerException != null ? ex.InnerException.ToString() : "none")
                : ex.Message;
        }

        private static void DoLog(string message, string prefix)    
        {
            System.Diagnostics.Debug.WriteLine(prefix + " " + message);
        }
    }
}