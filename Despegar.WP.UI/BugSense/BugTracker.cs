using BugSense;
using BugSense.Core.Model;
using Despegar.WP.UI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Despegar.Core.Log;

namespace Despegar.WP.UI.BugSense
{
    /// <summary>
    /// Tracks crashes and leaves breadcrumbs
    /// </summary>
    public class BugTracker : IBugTracker
    {
        private static IBugTracker instance;

        public static IBugTracker Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BugTracker();
                    breadCrumbCounter = 0;
                }

                return instance;
            }
        }
        private static int breadCrumbCounter;

        public void LeaveBreadcrumb(string breadcrumb) 
        {
            BugSenseHandler.Instance.LeaveBreadCrumb("[" + breadCrumbCounter + "] " + breadcrumb);
            breadCrumbCounter++;
        }

        public void LogException(Exception exception)
        {
            BugSenseHandler.Instance.LogException(exception);
        }

        public void LogException(Exception exception, object extrasExtraDataList) 
        {
            BugSenseHandler.Instance.LogException(exception, extrasExtraDataList as LimitedCrashExtraDataList);
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