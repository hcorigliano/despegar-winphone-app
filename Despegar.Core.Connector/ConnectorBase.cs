using Despegar.Core.Exceptions;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace Despegar.Core.Connector
{
    public abstract class ConnectorBase
    {
        private string x_client;   // Example: "WindowsPhone8App";        

        /// <summary>
        /// Initializes a new instance of Connector
        /// </summary>
        /// <param name="client">The X_CLIENT header</param>
        public ConnectorBase(string client) {
            this.x_client = client;            
        }

        /// <summary>
        /// Performs an HTTP GET request to a JSON service
        /// </summary>
        /// <typeparam name="T">Expected result type</typeparam>
        /// <param name="serviceUrl">Service Resource URL</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string serviceUrl) where T : class
        {
            HttpRequestMessage httpMessage = new HttpRequestMessage(HttpMethod.Get, serviceUrl);
            SetCustomHeaders(httpMessage);

            return await base.ProcessRequest<T>(httpMessage);
        }
      
        /// <summary>
        /// Performs an HTTP POST request to a JSON service
        /// </summary>
        /// <typeparam name="T">Expected result type</typeparam>
        /// <param name="serviceUrl">Service Resource URL</param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(string serviceUrl, string data) where T : class
        {
            HttpRequestMessage httpMessage = new HttpRequestMessage(HttpMethod.Post, serviceUrl);
            httpMessage.Content = new StringContent(data);
            httpMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            SetCustomHeaders(httpMessage);

            return await ProcessRequest<T>(httpMessage);
        }

        /// <summary>
        /// Launches the HTTP request and returns the JSON-deserialized response
        /// </summary>
        /// <returns>Deserialized JSON response object</returns>
        protected async Task<T> ProcessRequest<T>(HttpRequestMessage httpMessage) where T : class
        {
            SetCommonHeaders(httpMessage);
            string response = String.Empty;

            // Create Http Client
            HttpClient client = new HttpClient();
            HttpClientHandler handler = new HttpClientHandler();
            if (handler.SupportsAutomaticDecompression)
                handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            try
            {
                // Call Service
                HttpResponseMessage httpResponse = await client.SendAsync(httpMessage);

                if (httpResponse.Content == null)
                {
                    // TODO: Log exception
                    throw new EmptyWebResponseException(String.Format("Requested Service URL '{0}' returned an empty response.", httpMessage.RequestUri));
                }

                response = await httpResponse.Content.ReadAsStringAsync();

                // Deserialize JSON data to .NET object
                return JsonConvert.DeserializeObject<T>(response);
            }
            catch (HttpRequestException ex)
            {
                // HTTP Client error
                throw new WebConnectivityException(String.Format("Could not connect to Service URL ", httpMessage.RequestUri), ex);
                //TODO: Logger.Error(ex.ToString());
            }
            catch (JsonSerializationException ex)
            {
                // Deserializer JSON.NET Error
                throw new JsonDeserializationException(String.Format("Service call: {0}. Could not deserialize type '{1}' from service response data: {2}", httpMessage.RequestUri, typeof(T).FullName, response), ex);
                //TODO: Logger.Error(ex.ToString())
            }
            catch (Exception ex) {
                //TODO: Logger.Error(ex.ToString())
                throw new Exception(String.Format("Unknown Connector Error when calling Service URL {0}", httpMessage.RequestUri), ex);
            }

        }

        /// <summary>
        /// Gets the Base Service URL. Example: "https://mobile.despegar.com"
        /// </summary>
        /// <returns></returns>
        public abstract string GetBaseUrl();

        /// <summary>
        /// Tempalte Method for adding custom HTTP Headers
        /// </summary>
        /// <param name="message"></param>
        protected abstract void SetCustomHeaders(HttpRequestMessage message);

        private void SetCommonHeaders(HttpRequestMessage message)
        {
            message.Headers.Add("Accept-Encoding", "gzip, deflate");
            message.Headers.Add("Accept", "application/json");
            message.Headers.Add("X-Client", x_client);           
        }        
    }
}