using BugSense;
using BugSense.Core.Model;
using Despegar.Core.Neo.Contract.Log;
using Despegar.WP.UI.Model;
using Despegar.WP.UI.Model.Common;
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
            string content = "[" + breadCrumbCounter + "] " + breadcrumb;
            BugSenseHandler.Instance.LeaveBreadCrumb(content);

#if DEBUG
            GlobalConfiguration.Bredcrumbs.Add(new Breadcrumb() { ID = breadCrumbCounter.ToString(), Time = DateTime.Now, Content = content});
#endif
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