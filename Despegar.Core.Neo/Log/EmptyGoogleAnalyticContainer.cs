using Despegar.Core.Neo.Contract.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Log
{
    public class EmptyGoogleAnalyticContainer : IGoogleAnalytics
    {
        public void SendView(string pagePath)
        {

        }
    }
}
