using Despegar.Core.Neo.API;
using Despegar.Core.Neo.Contract;
using Despegar.Core.Neo.Contract.Connector;
using Despegar.Core.Neo.Contract.Log;
using Despegar.Core.Neo.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Connector
{
    public class Apiv3Connector : ConnectorBase, IApiv3Connector
    {
        private static readonly string DOMAIN = "https://api.despegar.com/v3/";
        private static readonly string APIKEY_WINDOWS_PHONE = "24b56c96e09146298eca3093f6f990c9";
        private string XUoW;
        private string x_client;   // Example: "WindowsPhone8App";
        private string userAgent;
        private string site;
        private string language;

        public Apiv3Connector(ICoreContext context, ICoreLogger logger, IBugTracker bugTracker)
            : base(context, logger, bugTracker)
        {
            logger.Log("Api V3 Connector created.");
        }

        public void ConfigureClientAndUow(string x_client, string uow, string userAgent)
        {
            this.x_client = x_client;
            this.XUoW = uow;
            this.userAgent = userAgent;
        }

        public void ConfigureSiteAndLanguage(string site, string language)
        {
            this.site = site;
            this.language = language;
        }

        protected override void SetCustomHeaders(HttpRequestMessage httpMessage, ServiceKey key)
        {
            httpMessage.Headers.Add("X-ApiKey", APIKEY_WINDOWS_PHONE);
            httpMessage.Headers.Add("X-UOW", XUoW);
            httpMessage.Headers.Add("X-Client", x_client);
            httpMessage.Headers.UserAgent.ParseAdd(userAgent);
        }

        protected override string GetBaseUrl()
        {
            return new StringBuilder()
              .Append(DOMAIN)
              .ToString();
        }

        private string IncludeSiteAndLanguage(string relativeServiceUrl)
        {
            return String.Format(relativeServiceUrl + "&site={0}&language={1}", site, language);
        }


        async Task<T> IConnector.GetAsync<T>(string url, API.ServiceKey key)
        {
            return await base.GetAsync<T>(url, key);
        }

        Task<T> IConnector.PostAsync<T>(string url, object data, API.ServiceKey key)
        {
            throw new NotImplementedException();
        }

        Task<T> IConnector.PutAsync<T>(string url, object data, API.ServiceKey key)
        {
            throw new NotImplementedException();
        }
        
        protected override void PostProcessing(HttpResponseMessage httpResponse)
        {
        }
    }
}
