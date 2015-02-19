using Despegar.Core.Neo.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Connector
{
    public class UPAConnector: ConnectorBase
    {
        public UPAConnector(IBugTracker bugtracker)
            : base(bugtracker)
        {
            
        }

        private static readonly string DOMAIN = "upa.despegar.com/";

        protected override string GetBaseUrl()
        {
            return new StringBuilder()
              .Append("https://")
              .Append(DOMAIN)
              .ToString();
        }


        protected override void SetCustomHeaders(System.Net.Http.HttpRequestMessage message)
        {
            
        }
    }
}
