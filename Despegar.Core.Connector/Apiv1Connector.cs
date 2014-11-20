﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Despegar.Core.Log;


namespace Despegar.Core.Connector
{
    public class Apiv1Connector : ConnectorBase
    {
        private static readonly string DOMAIN = "api.despegar.com/v1";
        private static readonly string APIKEY_WINDOWS_PHONE = "24b56c96e09146298eca3093f6f990c9";
        private string XUoW;
        private string x_client;   // Example: "WindowsPhone8App";
        private string site;
        private string currentAPIKey;
        private static readonly Dictionary<string, string> ApiKeysV1 = new Dictionary<string, string>()
        {
            {"_default_","dcb50a69-d4bc-4bc8-af64-97456a16964f"},
        };


        public Apiv1Connector() : base() {
            Logger.LogCore("MAPI Connector created.");
        }

        public void Configure(string x_client, string uow, string site)
        {
            this.x_client = x_client;
            this.XUoW = uow;
            this.site = site;
            this.currentAPIKey = ApiKeysV1[site];
            //this.language = language;
        }

        /// <summary>
        /// Gets the Base URL for calling a MAPI service
        /// </summary>        
        /// <returns>The MAPI Base URL</returns>
        protected override string GetBaseUrl()
        {
            return new StringBuilder()
              .Append("https://")
              .Append(DOMAIN)                           
              .ToString();
        }

        protected override void SetCustomHeaders(HttpRequestMessage httpMessage)
        {
            httpMessage.Headers.Add("X-ApiKey", APIKEY_WINDOWS_PHONE);
            httpMessage.Headers.Add("X-UOW", XUoW);
            httpMessage.Headers.Add("X-Client", x_client);           
            //httpMessage.Headers.Add("Accept-Encoding", ENCODING);
            //httpMessage.Headers.Add("Accept", APP_JSON);
        }
    }
}