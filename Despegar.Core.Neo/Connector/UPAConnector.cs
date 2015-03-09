using Despegar.Core.Neo.API;
using Despegar.Core.Neo.Contract;
using Despegar.Core.Neo.Contract.Connector;
using Despegar.Core.Neo.Contract.Log;
using System.Net.Http;
using System.Text;

namespace Despegar.Core.Neo.Connector
{
    public class UPAConnector : ConnectorBase, IUPAConnector
    {
        private static readonly string DOMAIN = "upa.despegar.com/";

        public UPAConnector(ICoreContext context, ICoreLogger logger, IBugTracker bugTracker)
            : base(context, logger, bugTracker)
        {
            logger.Log("UPA Connector created.");
        }

        protected override string GetBaseUrl()
        {
            return new StringBuilder()
              .Append("https://")
              .Append(DOMAIN)
              .ToString();
        }

        protected override void SetCustomHeaders(HttpRequestMessage message, ServiceKey key)
        {            
        }

        protected override void PostProcessing(HttpResponseMessage httpResponse)
        {
        }
    }
}