using Despegar.WP.UI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Common
{
    public sealed class GoogleAnalyticContainer
    {
        public GoogleAnalytics.Core.Tracker Tracker { get; set; }

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
