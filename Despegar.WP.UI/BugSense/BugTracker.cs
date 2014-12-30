using BugSense;
using BugSense.Core.Model;
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
    public static class BugTracker
    {
        public static void LeaveBreadcrumb(string breadcrumb) 
        {
            BugSenseHandler.Instance.LeaveBreadCrumb(breadcrumb);
        }

        public static void LogException(Exception exception)
        {
            BugSenseHandler.Instance.LogException(exception);
        }

        public static void LogException(Exception exception, LimitedCrashExtraDataList extrasExtraDataList) 
        {
            BugSenseHandler.Instance.LogException(exception, extrasExtraDataList);
        }
    }
}