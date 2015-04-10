using System;
using Despegar.Core.Neo.Contract.Log;

namespace Despegar.WP.UI.Common
{
    public sealed class GoogleAnalyticContainer : IGoogleAnalytics
    {
       public GoogleAnalytics.Core.Tracker Tracker { get; set; }

       public GoogleAnalyticContainer()
        {
            Tracker = GoogleAnalytics.EasyTracker.GetTracker();
        }

        #if DECOLAR
            public string prefix = "WindowsPhone8.1/Decolar.com/";
        #else
            public string prefix = "WindowsPhone8.1/Despegar.com/";
        #endif
        
        public void SendView(string pagePath)
        {        
            Tracker.SendView( prefix + pagePath);
        }
    }
}
