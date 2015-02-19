using Despegar.Core.Neo.API;
using Despegar.Core.Neo.Contract;
using Despegar.Core.Neo.Contract.Connector;
using Despegar.Core.Neo.Contract.Log;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Connector
{
    public class MapiConnector : ConnectorBase, IMapiConnector
    {
        private static readonly string DOMAIN = "https://mobile.despegar.com/v3/";
        private static readonly string APIKEY_WINDOWS_PHONE = "24b56c96e09146298eca3093f6f990c9";
        private string XUoW;
        private string x_client;   // Example: "WindowsPhone8App";
        private string site;
        private string language;

       
        public MapiConnector(ICoreContext context, ICoreLogger logger, IBugTracker bugTracker)
            : base(context, logger, bugTracker)
        {
            logger.Log("Mapi Connector created.");
        }

        public void ConfigureClientAndUow(string x_client, string uow)
        {
            this.x_client = x_client;
            this.XUoW = uow;
        }

        public void ConfigureSiteAndLanguage(string site, string language)
        {
            this.site = site;
            this.language = language;
        }

        /// <summary>
        /// Overrides Base method to include Site and Language parameters for MAPI
        /// </summary>
        /// <typeparam name="T">Expected result type</typeparam>
        /// <param name="relativeServiceUrl">Service Resource URL</param>
        /// <returns></returns>
        public override async Task<T> GetAsync<T>(string relativeServiceUrl, ServiceKey key)
        {
            return await base.GetAsync<T>(IncludeSiteAndLanguage(relativeServiceUrl), key);
        }

        /// <summary>
        /// Overrides Base method to include Site and Language parameters for MAPI
        /// </summary>
        /// <typeparam name="T">Expected result type</typeparam>
        /// <param name="relativeServiceUrl">Service Resource URL</param>
        /// <returns></returns>
        public override async Task<T> PostAsync<T>(string relativeServiceUrl, object postData, ServiceKey key)
        {
            return await base.PostAsync<T>(IncludeSiteAndLanguage(relativeServiceUrl), postData, key);
        }

        /// <summary>
        /// Gets the Base URL for calling a MAPI service
        /// </summary>        
        /// <returns>The MAPI Base URL</returns>
        protected override string GetBaseUrl()
        {
            return new StringBuilder()
              .Append(DOMAIN)                           
              .ToString();
        }

        protected override void SetCustomHeaders(HttpRequestMessage httpMessage)
        {
            httpMessage.Headers.Add("X-ApiKey", APIKEY_WINDOWS_PHONE);
            httpMessage.Headers.Add("X-UOW", XUoW);
          
            //httpMessage.Headers.Add("X-Version", "beta");
            //httpMessage.Headers.Add("XDESP-TOTEM-MOCK-OSS-RESULT", "CHANNEL_REJECTED");
            //httpMessage.Headers.Add("XDESP-TOTEM-MOCK-OSS-FEE-RESULT", "CHANNEL_REJECTED");
            //httpMessage.Headers.Add("XDESP-TOTEM-MOCK-MILES-SECOND-STATUS", "CONFIRMED");
            //httpMessage.Headers.Add("XDESP-TOTEM-MOCK-MILES-STATUS", "CONFIRMED");
            //httpMessage.Headers.Add("XDESP-TOTEM-MOCK-MILES-REQUEST", "SUCCEEDED");

            httpMessage.Headers.Add("X-Client", x_client);           

            // TODO: What happens with new versions of MAPI???
            if (httpMessage.RequestUri.AbsoluteUri.Contains("https://mobile.despegar.com/v3/mapi-hotels/"))
                httpMessage.Headers.Add("X-Version", "mapi-hotels-v3_1.1.0");
        }
    
        private string IncludeSiteAndLanguage(string relativeServiceUrl)
        {
 	        return String.Format(relativeServiceUrl + "&site={0}&language={1}", site, language);
        }
    }
}