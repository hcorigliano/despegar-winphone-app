using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

using Despegar.LegacyCore.Util;
using System.Net.Http.Headers;
using Despegar.LegacyCore.ViewModel;
using Windows.ApplicationModel.Resources;

namespace Despegar.LegacyCore.Connector
{
    public class APIConnector
    {
        public HttpClient Client { get; set; }

        private static ResourceLoader legacyManager = new ResourceLoader("LegacyConnectorStrings");
        private static ResourceLoader legacyAPIKeyManager = new ResourceLoader("LegacyAPIKeys");
        private static string ENCODING = "gzip, deflate";
        private static string APP_JSON = "application/json";
        private static string X_CLIENT = "WindowsPhone8App";

        private string x_uow { get; set; }
        private string api_key { get { return legacyAPIKeyManager.GetString(this.Channel); } }
        public string Channel { get; set; }


        public APIConnector()
        {
            Logger.Info("[connector:init] APIConnector Instance created");

            x_uow = String.Format("WP8-{0}", ApplicationConfig.Instance.DeviceId);
            Channel = ApplicationConfig.Instance.Country != "" ? ApplicationConfig.Instance.Country : "_default_";

            HttpClientHandler handler = new HttpClientHandler();
            if (handler.SupportsAutomaticDecompression)
                handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            Client = new HttpClient(handler);
        }


        public async Task<string> SendAsync(HttpRequestMessage httpMessage)
        {
            HttpResponseMessage httpResponse = default(HttpResponseMessage);
            string response = "";

            try 
            {
                httpResponse = await Client.SendAsync(httpMessage);
                if (httpResponse.Content != null)
                    response = await httpResponse.Content.ReadAsStringAsync();
            }

            catch (HttpRequestException ex)
            {
                Logger.Warn(ex.ToString());
                AppDelegate.Instance.RequestError(ex);
            }

            return response;
        }

        // API v1 message builder
        public HttpRequestMessage MessageBuilder(string service)
        {
            StringBuilder url = new StringBuilder()
               .Append(legacyManager.GetString("_http"))
               .Append(legacyManager.GetString("_base_api"))
               .Append(legacyManager.GetString("_version_none"))
               .Append(legacyManager.GetString("api_" + service));

            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, url.ToString());
            message.Headers.Add("X-ApiKey", api_key);
            message.Headers.Add("X-UOW", this.x_uow);
            return SetCommonHeaders(message);
        }

        // API v1 service content builder (for POST)
        public HttpRequestMessage ContentBuilder(string service, string data)
        {
            StringBuilder url = new StringBuilder()
               .Append(legacyManager.GetString("_http"))
               .Append(legacyManager.GetString("_base_api"))
               .Append(legacyManager.GetString("_version_none"))
               .Append(legacyManager.GetString("api_" + service));

            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, url.ToString());
            message.Content = new StringContent(data);
            message.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            message.Headers.Add("X-ApiKey", api_key);
            message.Headers.Add("X-UOW", this.x_uow);
            return SetCommonHeaders(message);
        }

        // API v1 service content builder SECURE (for POST)
        public HttpRequestMessage ContentBuilderSecure(string service, string data)
        {
            StringBuilder url = new StringBuilder()
               .Append(legacyManager.GetString("_https"))
               .Append(legacyManager.GetString("_base_api"))
               .Append(legacyManager.GetString("_version_v1"))
               .Append(legacyManager.GetString("api_" + service));

            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, url.ToString());
            message.Content = new StringContent(data);
            message.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            message.Headers.Add("X-ApiKey", api_key);
            message.Headers.Add("X-UOW", this.x_uow);
            return SetCommonHeaders(message);
        }

        // API v3 service message builder (API keys should change)
        public HttpRequestMessage MessageBuilderForMapi(string service)
        {
            StringBuilder url = new StringBuilder()
               .Append(legacyManager.GetString("_http"))
               .Append(legacyManager.GetString("._base_mapi"))
               .Append(legacyManager.GetString("_version_v3"))
               .Append(legacyManager.GetString("mapi_" + service));

            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, url.ToString());
            message.Headers.Add("X-ApiKey", api_key);
            message.Headers.Add("X-UOW", this.x_uow);
            return SetCommonHeaders(message);
        }

        // API v3 service content builder (for POST) (API keys should change)
        public HttpRequestMessage ContentBuilderForMapi(string service, string data)
        {
            StringBuilder url = new StringBuilder()
               .Append(legacyManager.GetString("_http"))
               .Append(legacyManager.GetString("._base_mapi"))
               .Append(legacyManager.GetString("._version_v3"))
               .Append(legacyManager.GetString("mapi_" + service));

            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, url.ToString());
            message.Content = new StringContent(data);
            message.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            message.Headers.Add("X-ApiKey", api_key);
            message.Headers.Add("X-UOW", this.x_uow);
            return SetCommonHeaders(message);
        }

        // UPA service message builder w/o api keys
        public HttpRequestMessage MessageBuilderForUpa(string service)
        {
            StringBuilder url = new StringBuilder()
               .Append(legacyManager.GetString("_http"))
               .Append(legacyManager.GetString("._base_upa"))
               .Append(legacyManager.GetString("._version_none"))
               .Append(legacyManager.GetString("upa_" + service));
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, url.ToString());
            return SetCommonHeaders(message);
        }

        // Mobile statics message builder
        public HttpRequestMessage MessageBuilderForMobile(string service)
        {
            StringBuilder url = new StringBuilder()
               .Append(legacyManager.GetString("_http"))
               .Append(legacyManager.GetString("_base_mobile"))
               .Append(legacyManager.GetString("._version_none"))
               .Append(legacyManager.GetString("mobile_" + service));
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, url.ToString());
            return SetCommonHeaders(message);
        }

        private HttpRequestMessage SetCommonHeaders(HttpRequestMessage message)
        {
            message.Headers.Add("Accept-Encoding", ENCODING);
            message.Headers.Add("Accept", APP_JSON);
            message.Headers.Add("X-Client", X_CLIENT);
            return message;
        }

        // SHARED INSTANCE
        public static volatile APIConnector instance;
        private static object syncRoot = new Object();
        public static APIConnector Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new APIConnector();
                        }
                    }
                }

                return instance;
            }
        }
    }
}
