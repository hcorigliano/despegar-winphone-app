using BugSense;
using BugSense.Core.Model;
using Despegar.WP.UI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.BugSense
{
    /// <summary>
    /// Tracks crashes and leaves breadcrumbs
    /// </summary>
    public class BugTracker : IBugTracker
    {
        public void LeaveBreadcrumb(string breadcrumb) 
        {
            BugSenseHandler.Instance.LeaveBreadCrumb(breadcrumb);
        }

        public void LogException(Exception exception)
        {
            BugSenseHandler.Instance.LogException(exception);
        }

        public void LogException(Exception exception, object extrasExtraDataList) 
        {
            BugSenseHandler.Instance.LogException(exception, extrasExtraDataList as LimitedCrashExtraDataList);
        }

        private static IBugTracker instance;
        public static IBugTracker Instance
        {
            get
            {
                if (instance == null)
                    instance = new BugTracker();

                return instance;
            }
        }
    }
}