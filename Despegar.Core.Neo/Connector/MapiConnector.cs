using Despegar.Core.Neo.API;
using Despegar.Core.Neo.Contract;
using Despegar.Core.Neo.Contract.Connector;
using Despegar.Core.Neo.Contract.Log;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Connector
{
    public class MapiConnector : ConnectorBase, IMapiConnector
    {
        private static readonly string DOMAIN = "https://mobile.despegar.com/v3/";
        private static readonly string APIKEY_WINDOWS_PHONE = "24b56c96e09146298eca3093f6f990c9";
        private static readonly string MAPI_UPA_COOKIE_NAME = "X-UPACOOKIE";
        private static readonly ServiceKey[] cookieEnabledServices = new ServiceKey[] { ServiceKey.FlightItineraries, ServiceKey.FlightsBookingFields, ServiceKey.FlightsBookingCompletePost, ServiceKey.HotelsAvailability, ServiceKey.HotelsAvailabilityByGeo, ServiceKey.HotelsGetDetails, ServiceKey.HotelsBookingFields, ServiceKey.HotelsBookingCompletePost  };
        private string XUoW;
        private string x_client;   // Example: "WindowsPhone8App";
        private string site;
        private string language;
        private string mapiUPACookie;        
       
        public MapiConnector(ICoreContext context, ICoreLogger logger, IBugTracker bugTracker)
            : base(context, logger, bugTracker)
        {
            logger.Log("Mapi Connector created.");
            this.mapiUPACookie = String.Empty;
        }

        public void ConfigureClientAndUow(string x_client, string uow)
        {
            this.x_client = x_client;
            this.XUoW = uow;
        }

        public void ConfigureSiteAndLanguage(string site, string language)
        {
            this.site = site;
            this.language = language.ToLowerInvariant();;
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
        /// Overrides Base method to include Site and Language parameters for MAPI
        /// </summary>
        /// <typeparam name="T">Expected result type</typeparam>
        /// <param name="relativeServiceUrl">Service Resource URL</param>
        /// <returns></returns>
        public override async Task<T> PatchAsync<T>(string relativeServiceUrl, object postData, ServiceKey key)
        {
            return await base.PatchAsync<T>(IncludeSiteAndLanguage(relativeServiceUrl), postData, key);
        }

        /// <summary>
        /// Overrides Base method 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="relativeServiceUrl"></param>
        /// <param name="postData"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public override async Task<T> PutAsync<T>(string relativeServiceUrl, object postData, ServiceKey key)
        {
            return await base.PutAsync<T>(relativeServiceUrl, postData, key);
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

        protected override void SetCustomHeaders(HttpRequestMessage httpMessage, ServiceKey key)
        {
            // MAPI COOKIE
            SetMAPICookie(httpMessage, key);
            // MAPI Headers
            httpMessage.Headers.Add("X-ApiKey", APIKEY_WINDOWS_PHONE);
            httpMessage.Headers.Add("X-UOW", XUoW);
            httpMessage.Headers.Add("X-Client", x_client);           

            // This makes MAPI point to Hoteles v3
            if (httpMessage.RequestUri.AbsoluteUri.Contains("https://mobile.despegar.com/v3/mapi-hotels/"))
                httpMessage.Headers.Add("X-Version", "mapi-hotels-v3_1.1.0");

            //httpMessage.Headers.Add("X-Version", "beta");  // Apuntar a BETA de MAPI
        }

        private void SetMAPICookie(HttpRequestMessage httpMessage, ServiceKey key)
        {
            if (!String.IsNullOrEmpty(mapiUPACookie))
            {
                if (cookieEnabledServices.Contains(key))
                   httpMessage.Headers.Add(MAPI_UPA_COOKIE_NAME, mapiUPACookie);
            }
        }
    
        private string IncludeSiteAndLanguage(string relativeServiceUrl)
        {
 	        return String.Format(relativeServiceUrl + "&site={0}&language={1}", site, language);
        }

        protected override void PostProcessing(HttpResponseMessage httpResponse)
        {
            // Check if mapi cookie is present
            if (httpResponse != null && httpResponse.Headers.Contains(MAPI_UPA_COOKIE_NAME))
            {
                var values = httpResponse.Headers.GetValues(MAPI_UPA_COOKIE_NAME).FirstOrDefault();
                if (!String.IsNullOrWhiteSpace(values)) 
                { 
                   // Remember not to rewrite the MAPI COOKIE if it is empty.
                    //MAXPOWER
                    mapiUPACookie = values.Replace("Ã¡", "á")
                                           .Replace("Ã©", "é")
                                           .Replace("Ã­", "í")
                                           .Replace("Ã³", "ó")
                                           .Replace("Ãº", "ú")
                                           .Replace("Ã±", "ñ");
                }
            }
        }
    }
}