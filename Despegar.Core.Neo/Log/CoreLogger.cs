using Despegar.Core.Neo.Contract;
using Despegar.Core.Neo.Contract.Log;
using System;

namespace Despegar.Core.Neo.Log
{
    public class CoreLogger : ICoreLogger
    {

        /// <summary>
        /// Method for logging Application level events
        /// </summary>
        /// <param name="message"></param>
        public void Log(string message) 
        {
                DoLog(message);
        }
   
        /// <summary>
        /// Method for logging Core thrown exceptions 
        /// </summary>
        /// <param name="message"></param>
        public void LogException(Exception ex)
        {
                DoLog(GetExceptionMessage(ex));
        }
        
        private string GetExceptionMessage(Exception ex)
        {
            return  ex.Message + " | " +
                ex.ToString() + " | InnerException: " +
                (ex.InnerException != null ? ex.InnerException.ToString() : "none");
        }

        private void DoLog(string message)    
        {
            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}