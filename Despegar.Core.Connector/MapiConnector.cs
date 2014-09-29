using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace Despegar.Core.Connector
{
    public class MapiConnector : ConnectorBase
    {
        private static MapiConnector instance;
        private static readonly string DOMAIN = "mobile.despegar.it/v3/";
        private static readonly string APIKEY_WINDOWS_PHONE = "24b56c96e09146298eca3093f6f990c9";
        private static string XUoW;
        private static string x_client;   // Example: "WindowsPhone8App";
        private static string site;
        private static string language;

        private MapiConnector() : base() { }

        public static MapiConnector GetInstance() 
        {
            if (instance == null) {
                instance = new MapiConnector();
            }

            return instance;
        }

        public static void Configure(string x_client, string uow, string site, string language)
        {
            MapiConnector.x_client = x_client;
            MapiConnector.XUoW = uow;
            MapiConnector.site = site;
            MapiConnector.language = language;
        }

        ///// <summary>
        ///// Overrides Base method to include Site and Language parameters for MAPI
        ///// </summary>
        ///// <typeparam name="T">Expected result type</typeparam>
        ///// <param name="relativeServiceUrl">Service Resource URL</param>
        ///// <returns></returns>
        //public async Task<T> GetAsync<T>(string relativeServiceUrl) where T:class
        //{            
        //    return base.GetAsync<T>(IncludeSiteAndLanguage(relativeServiceUrl));
        //}

        ///// <summary>
        ///// Overrides Base method to include Site and Language parameters for MAPI
        ///// </summary>
        ///// <typeparam name="T">Expected result type</typeparam>
        ///// <param name="relativeServiceUrl">Service Resource URL</param>
        ///// <returns></returns>
        //public async Task<T> PostAsync<T>(string relativeServiceUrl, object postData)
        //{
            
        //}

        /// <summary>
        /// Gets the Base URL for calling a MAPI service
        /// </summary>        
        /// <returns>The MAPI Base URL</returns>
        protected override string GetBaseUrl()
        {
            return new StringBuilder()
              .Append("http://")
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
 	        return String.Format(relativeServiceUrl + "&site={0}&langauge={1}", site, language);
        }
    }
}