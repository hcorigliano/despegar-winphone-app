using Despegar.Core.Neo.API;
using Despegar.Core.Neo.Contract;
using Despegar.Core.Neo.Contract.Log;
using Despegar.Core.Neo.Exceptions;
using Despegar.Core.Neo.Log;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Connector
{
    public abstract class ConnectorBase : IConnector
    {
        private const int MOCKED_RESPONSE_WAITING_TIME = 2000;
        private ICoreContext context;
        protected ICoreLogger logger;
        protected IBugTracker bugTracker;
        private HttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of Connector
        /// </summary>
        /// <param name="client">The X_CLIENT header</param>
        public ConnectorBase(ICoreContext context, ICoreLogger logger, IBugTracker bugTracker) 
        {
            this.context = context;
            this.logger = logger;
            this.bugTracker = bugTracker;

            // Create Http Client            
            HttpClientHandler handler = new HttpClientHandler();
            if (handler.SupportsAutomaticDecompression)
                handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            this.httpClient = new HttpClient(handler);
        }

        /// <summary>
        /// Performs an HTTP GET request to a JSON service
        /// </summary>
        /// <typeparam name="T">Expected result type</typeparam>
        /// <param name="relativeServiceUrl">Service Resource URL</param>
        /// <returns></returns>
        public virtual async Task<T> GetAsync<T>(string relativeServiceUrl, ServiceKey key) where T : class
        {
            string url = GetBaseUrl() + relativeServiceUrl;

            HttpRequestMessage httpMessage = new HttpRequestMessage(HttpMethod.Get, url);
            SetCustomHeaders(httpMessage);

            return await ProcessRequest<T>(httpMessage, key);            
        }
      
        /// <summary>
        /// Performs an HTTP POST request to a JSON service
        /// </summary>
        /// <typeparam name="T">Expected result type</typeparam>
        /// <param name="relativeServiceUrl">Service Resource URL</param>
        /// <returns></returns>
        public virtual async Task<T> PostAsync<T>(string relativeServiceUrl, object postData, ServiceKey key) where T : class
        {
            string data = String.Empty;
            string url = GetBaseUrl() + relativeServiceUrl;

            try 
            {
              data = JsonConvert.SerializeObject(postData);
            }
            catch(JsonSerializationException ex)
            {
                var e = new JsonSerializerException(String.Format("[Connector]:Could not serialize object of type {0} to call Service: {1}", typeof(T).FullName, url), ex);
                logger.LogException(e);
                throw e;
            }

            HttpRequestMessage httpMessage = new HttpRequestMessage(HttpMethod.Post, url);
            httpMessage.Content = new StringContent(data);
            httpMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            SetCustomHeaders(httpMessage);

            return await ProcessRequest<T>(httpMessage, key);
        }

        /// <summary>
        /// This is the method for sending put command.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="relativeServiceUrl"></param>
        /// <param name="postData"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual async Task<T> PutAsync<T>(string relativeServiceUrl, object postData, ServiceKey key) where T : class
        {
            string data = String.Empty;
            string url = GetBaseUrl() + relativeServiceUrl;

#if DEBUG
            url = url.Replace("https://mobile.despegar.com/", "http://mobile.despegar.it/");
#endif

            try
            {
                data = JsonConvert.SerializeObject(postData);
            }
            catch (JsonSerializationException ex)
            {
                var e = new JsonSerializerException(String.Format("[Connector]:Could not serialize object of type {0} to call Service: {1}", typeof(T).FullName, url), ex);
                logger.LogException(e);
                throw e;
            }

            HttpRequestMessage httpMessage = new HttpRequestMessage(HttpMethod.Put, url);
            httpMessage.Content = new StringContent(data);
            httpMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            SetCustomHeaders(httpMessage);

            return await ProcessRequest<T>(httpMessage, key);
        }

        /// <summary>
        /// Launches the HTTP request and returns the JSON-deserialized response
        /// </summary>
        /// <returns>Deserialized JSON response object</returns>
        protected async Task<T> ProcessRequest<T>(HttpRequestMessage httpMessage, ServiceKey key) where T : class
        {
            SetCommonHeaders(httpMessage);
            string response = String.Empty;
            bool customExceptionThrown = false;
            bool requestSuccess = false;
            HttpResponseMessage httpResponse = null;

            try
            {
                // Log last url
                bugTracker.LogURL(httpMessage.RequestUri.AbsoluteUri);

                // Call Service or Mock it                
                response = context.GetMockedResponse(key);

                if (response != null)
                {
                    // Simulate network latency
                    await Task.Delay(MOCKED_RESPONSE_WAITING_TIME);
                    requestSuccess = true;
                } else {
                   httpResponse = await httpClient.SendAsync(httpMessage);
                   response = await httpResponse.Content.ReadAsStringAsync();
                   requestSuccess = httpResponse.IsSuccessStatusCode;
                }

                // Check HTTP Error Codes
                if (!requestSuccess)
                {
                    customExceptionThrown = true;
                    Exception e = null;
                    try
                    {
                        // Check Empty Response
                        if (String.IsNullOrWhiteSpace(response))
                        {
                            e = new HTTPStatusErrorException(String.Format("[Core:Connector]: HTTP Error code {0} ({1}) Message: {2}", (int)httpResponse.StatusCode, httpResponse.StatusCode.ToString(), response));
                            customExceptionThrown = true;
                        }
                        else
                        {
                            // Try to Parse an API Error
                            e = new APIErrorException("[Core:Connector] MAPI Error: " + response);
                            ((APIErrorException)e).ErrorData = JsonConvert.DeserializeObject<MAPIError>(response);
                        }
                    }
                    catch (Exception ex) 
                    {
                        e = new HTTPStatusErrorException(String.Format("[Core:Connector]: Key " + key.ToString() + ": HTTP Error code {0} ({1}) Message: {2}", (int)httpResponse.StatusCode, httpResponse.StatusCode.ToString(), response), ex);                        
                    }

                    logger.LogException(e);
                    throw e;
                }

                // Deserialize JSON data to .NET object
                return JsonConvert.DeserializeObject<T>(response);
            }
            catch (HttpRequestException ex)
            {
                // HTTP Client error
                var e = new WebConnectivityException(String.Format("[Core:Connector]: Key " + key.ToString() + " Could not connect to Service URL ", httpMessage.RequestUri), ex);
                logger.LogException(e);
                throw e;
            }
            catch (JsonException ex)
            {
                // Deserializer JSON.NET Error
                var e = new JsonSerializerException(String.Format("[Core:Connector]: Key " + key.ToString() + " Service call: {0}. Could not deserialize type '{1}' from service response data: {2}", httpMessage.RequestUri, typeof(T).FullName, response), ex);
                logger.LogException(e);
                throw e;
            }
            catch (Exception ex) 
            {
                if (customExceptionThrown)                 
                    throw ex;

                var e = new Exception(String.Format("[Core:Connector]: Key " + key.ToString() + " Unknown Connector Error when calling Service URL {0}, Exception Message: {1}", httpMessage.RequestUri, ex.ToString()), ex);
                  logger.LogException(e);
                  throw e;
                }
          }

        /// <summary>
        /// Gets the Base Service URL. Example: "https://mobile.despegar.com"
        /// </summary>
        /// <returns></returns>
        protected abstract string GetBaseUrl();        

        /// <summary>
        /// Template Method for adding custom HTTP Headers
        /// </summary>
        /// <param name="message"></param>
        protected abstract void SetCustomHeaders(HttpRequestMessage message);

        private void SetCommonHeaders(HttpRequestMessage message)
        {
            message.Headers.Add("Accept-Encoding", "gzip, deflate");
            message.Headers.Add("Accept", "application/json");
            message.Headers.Add("Accept-Charset", "ISO-8859-1,utf-8");
        }
    }
}