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
        private readonly string DOMAIN = "mobile.despegar.com/v3";
        //TODO: Site and Language

        private string APIKey;
        private string XUoW;

        /// <summary>
        /// Initializes a new instance of MapiConnector
        /// </summary>
        /// <param name="client"></param>
        public MapiConnector(string client) : base(client)
        {
            // MapiConfiguration
            //TODO: set ApiKeys
        }

        /// <summary>
        /// Performs an HTTP GET request to a MAPI service
        /// </summary>
        /// <typeparam name="T">Expected result type</typeparam>
        /// <param name="serviceKey">Service Resource Key</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string serviceUrl) where T:class
        {
            HttpRequestMessage httpMessage = new HttpRequestMessage(HttpMethod.Get, serviceUrl);
            SetMapiHeader(httpMessage);

            return await base.ProcessRequest<T>(httpMessage);
        }
      
        /// <summary>
        /// Performs an HTTP POST request to a MAPI service
        /// </summary>
        /// <typeparam name="T">Expected result type</typeparam>
        /// <param name="serviceKey">Service Resource Key</param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(string serviceUrl, string data) where T : class
        {
            HttpRequestMessage httpMessage = new HttpRequestMessage(HttpMethod.Post, serviceUrl);
            httpMessage.Content = new StringContent(data);
            httpMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            SetMapiHeader(httpMessage);

            return await base.ProcessRequest<T>(httpMessage);
        }

        /// <summary>
        /// Gets the Base URL for calling a MAPI service
        /// </summary>        
        /// <returns>The MAPI Base URL</returns>
        public string GetMapiBaseURL()
        {
            return new StringBuilder()
              .Append("http://")
              .Append(this.DOMAIN)                           
              .ToString();
        }

        private void SetMapiHeader(HttpRequestMessage httpMessage)
        {
            httpMessage.Headers.Add("X-ApiKey", this.APIKey);
            httpMessage.Headers.Add("X-UOW", this.XUoW);
        }
    }
}
