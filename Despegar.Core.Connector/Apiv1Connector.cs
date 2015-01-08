using System;
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
            {"AR","a8c78c20-fc07-4283-9278-c436e57610bf"},
            {"AU","99a09314-7fc4-465a-9ea8-cedb64bd3571"},
            {"BO","16307f0b-cee5-4128-9a85-51c08e4b3fff"},
            {"BR","b5e22dd1-bc47-48ed-bb8d-d9584865eb09"},
            {"CL","d40a1d5e-418d-4961-9297-f0e8e2ab89f4"},
            {"CO","af42b886-69d2-4bd0-a120-bba62830ea73"},
            {"CR","2fb184bc-e0ca-44dd-bab2-3dac5d5a3a97"},
            {"DO","ef647bad-22fc-444a-9869-c54013d270b0"},
            {"EC","96b16217-bfff-4a9b-a2ec-4b1660a83a84"},
            {"ES","bbdbfda0-fb1e-436a-a4c7-eddd8a968e04"},
            {"GT","d0c90c00-0cb8-46f7-bef0-443ea589b2e6"},
            {"HN","f4534312-9b3b-4040-88f1-5e652d841233"},
            {"MX","aa34efc7-a990-4324-8526-d67b56f1bc3e"},
            {"NI","8df7b8d4-dc96-446c-b73b-e7caa46dfe6d"},
            {"PA","011c0601-04eb-42c4-b9a7-b790d4d9f912"},
            {"PE","5ba7828e-7057-413c-b16a-4b750ec66c6d"},
            {"PR","32b294b6-455e-4a55-a43d-1f2b9a7d6c74"},
            {"PY","5f9a3bf1-4e92-42f7-8ad6-5ba93903633d"},
            {"SV","1620dadc-b0b2-4cdc-9d20-9ce101e8d89c"},
            {"US","4dfa1713-5f67-4ad6-88ae-1dd24124264a"},
            {"UY","f3fd9c45-4e1b-498b-9c51-1be980940e31"},
            {"VE","869e3495-a303-497f-9aa9-e05690815d6a"}
        };


        public Apiv1Connector(IBugTracker bugtracker)
            : base(bugtracker)
        {
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
