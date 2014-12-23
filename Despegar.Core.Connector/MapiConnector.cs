using Despegar.Core.Log;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Connector
{
    public class MapiConnector : ConnectorBase
    {
        private static readonly string DOMAIN = "https://mobile.despegar.com/v3/";
        private static readonly string APIKEY_WINDOWS_PHONE = "24b56c96e09146298eca3093f6f990c9";
        private string XUoW;
        private string x_client;   // Example: "WindowsPhone8App";
        private string site;
        private string language;

        public MapiConnector() : base() {
            Logger.LogCore("MAPI Connector created.");
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
        public override async Task<T> GetAsync<T>(string relativeServiceUrl)
        {
            return await base.GetAsync<T>(IncludeSiteAndLanguage(relativeServiceUrl));
        }

        /// <summary>
        /// Overrides Base method to include Site and Language parameters for MAPI
        /// </summary>
        /// <typeparam name="T">Expected result type</typeparam>
        /// <param name="relativeServiceUrl">Service Resource URL</param>
        /// <returns></returns>
        public override async Task<T> PostAsync<T>(string relativeServiceUrl, object postData)
        {
            return await base.PostAsync<T>(IncludeSiteAndLanguage(relativeServiceUrl), postData);
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
            httpMessage.Headers.Add("X-Client", x_client);           
        }
    
        private string IncludeSiteAndLanguage(string relativeServiceUrl)
        {
 	        return String.Format(relativeServiceUrl + "&site={0}&language={1}", site, language);
        }
    }
}