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
        private static readonly string DOMAIN = "mobile.despegar.it/v3/";
        private static readonly string APIKEY_WINDOWS_PHONE = "24b56c96e09146298eca3093f6f990c9"; //TODO: Site and Language                
        private string XUoW;

        /// <summary>
        /// Initializes a new instance of MapiConnector
        /// </summary>
        /// <param name="client"></param>
        public MapiConnector(string client) : base(client)
        {            
            //TODO: set xuow
            this.XUoW = "W8";            
        }    

        /// <summary>
        /// Gets the Base URL for calling a MAPI service
        /// </summary>        
        /// <returns>The MAPI Base URL</returns>
        public override string GetBaseUrl()
        {
            return new StringBuilder()
              .Append("http://")
              .Append(DOMAIN)                           
              .ToString();
        }

        protected override void SetCustomHeaders(HttpRequestMessage httpMessage)
        {
            httpMessage.Headers.Add("X-ApiKey", APIKEY_WINDOWS_PHONE);
            httpMessage.Headers.Add("X-UOW", this.XUoW);
        }
    }
}