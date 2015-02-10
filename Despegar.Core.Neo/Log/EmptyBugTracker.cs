using Despegar.Core.Neo.Contract.Log;
using System;

namespace Despegar.Core.Neo.Log
{
    internal class EmptyBugTracker : IBugTracker
    {
        public void LeaveBreadcrumb(string breadcrumb)
        {            
        }

        public void LogException(Exception exception)
        {
        }

        public void LogException(Exception exception, object extrasExtraDataList)
        {
        }

        public void SetExtraData(string key, string value)
        {
        }

        public void LogEvent(string eventName)
        {
        }

        public void LogURL(string url)
        {
        }
    }
}