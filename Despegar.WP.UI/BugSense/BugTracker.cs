using BugSense;
using BugSense.Core.Model;
using Despegar.Core.Neo.Contract.Log;
using System;

namespace Despegar.WP.UI.BugSense
{
    /// <summary>
    /// Tracks crashes and leaves breadcrumbs
    /// </summary>
    public class SplunkMintBugTracker : IBugTracker
    {        
        private int breadCrumbCounter;

        public void LeaveBreadcrumb(string breadcrumb) 
        {
            BugSenseHandler.Instance.LeaveBreadCrumb("[" + breadCrumbCounter + "] " + breadcrumb);
            breadCrumbCounter++;
        }

        /// <summary>
        ///  Warning, This method will send a "Crash report" to Splunk mint and it will count for the Error rate.
        /// </summary>
        /// <param name="exception"></param>
        public void LogException(Exception exception)
        {
            //BugSenseHandler.Instance.LogException(exception);
        }

        public void LogException(Exception exception, object extrasExtraDataList)
        {
            //BugSenseHandler.Instance.LogException(exception, extrasExtraDataList as LimitedCrashExtraDataList);
        }

        public void SetExtraData(string key, string value)
        {
            LimitedCrashExtraDataList extras = BugSenseHandler.Instance.CrashExtraData;
            extras.Add(new CrashExtraData
            {
                Key = key,
                Value = value
            });
        }

        public void LogEvent(string eventName)
        {
            BugSenseHandler.Instance.LogEvent(eventName);
        }

        public void LogURL(string url)
        {
            this.SetExtraData("LastURL", url);
        }
    }
}