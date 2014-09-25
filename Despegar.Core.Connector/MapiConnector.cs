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
        /// Gets the Base URL for calling a MAPI service
        /// </summary>        
        /// <returns>The MAPI Base URL</returns>
        public override string GetBaseURL()
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