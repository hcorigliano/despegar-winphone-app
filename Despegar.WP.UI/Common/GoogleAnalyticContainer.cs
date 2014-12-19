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
        public string prefix = "WindowsPhone8.1/Despegar.com/";

        public void SendView(string pagePath)
        {
            
            Tracker.SendView( prefix + pagePath+ "/" + GlobalConfiguration.Site);
        }
    }
}
