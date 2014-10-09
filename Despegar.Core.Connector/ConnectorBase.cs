﻿using Despegar.Core.Exceptions;
using Newtonsoft.Json;
using Despegar.Core.Log;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Despegar.Core.Connector
{
    public abstract class ConnectorBase : IConnector
    {
        public object syncLock = new Object();        
        private HttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of Connector
        /// </summary>
        /// <param name="client">The X_CLIENT header</param>
        public ConnectorBase() {
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
        public virtual async Task<T> GetAsync<T>(string relativeServiceUrl) where T : class
        {
            string url = GetBaseUrl() + relativeServiceUrl;

            HttpRequestMessage httpMessage = new HttpRequestMessage(HttpMethod.Get, url);
            SetCustomHeaders(httpMessage);

            return await ProcessRequest<T>(httpMessage);
        }
      
        /// <summary>
        /// Performs an HTTP POST request to a JSON service
        /// </summary>
        /// <typeparam name="T">Expected result type</typeparam>
        /// <param name="relativeServiceUrl">Service Resource URL</param>
        /// <returns></returns>
        public virtual async Task<T> PostAsync<T>(string relativeServiceUrl, object postData) where T : class
        {
            string data = String.Empty;
            string url = GetBaseUrl() + relativeServiceUrl;

            try 
            {
              data = JsonConvert.SerializeObject(postData);
            }
            catch(JsonSerializationException e)
            {
                throw new JsonDeserializationException(String.Format("[Connector]:Could not serialize object of type {0} to call Service: {1}", typeof(T).FullName, url), ex);
                Logger.LogCoreException(e);
                throw e;
            }

            HttpRequestMessage httpMessage = new HttpRequestMessage(HttpMethod.Post, url);
            httpMessage.Content = new StringContent(data);
            httpMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json; charset=utf-8");
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

            try
            {
                // Call Service
                HttpResponseMessage httpResponse = await httpClient.SendAsync(httpMessage);
                response = await httpResponse.Content.ReadAsStringAsync();

                // Check HTTP Error Codes
                if (!httpResponse.IsSuccessStatusCode)
                {
                    // TODO: Log exception
                    throw new HTTPStatusErrorException(String.Format("[Connector]: HTTP Error code {0} Message: {1}" , httpResponse.StatusCode.ToString(), response));
                }

                // Check Empty Response
                if (String.IsNullOrEmpty(response))
                {
                    var e = new HTTPStatusErrorException(String.Format("[Connector]: HTTP Error code {0} Message: {1}", httpResponse.StatusCode.ToString(), response));
                    Logger.LogCoreException(e);
                    throw e;
                }

                // Deserialize JSON data to .NET object
                return JsonConvert.DeserializeObject<T>(response);
            }
            catch (HttpRequestException ex)
            {
                // HTTP Client error
                var e = new WebConnectivityException(String.Format("[Connector]: Could not connect to Service URL ", httpMessage.RequestUri), ex);
                Logger.LogCoreException(e);
                throw e;
            }
            catch (JsonSerializationException ex)
            {
                // Deserializer JSON.NET Error
                var e = new JsonSerializerException(String.Format("[Connector]: Service call: {0}. Could not deserialize type '{1}' from service response data: {2}", httpMessage.RequestUri, typeof(T).FullName, response), ex);
                Logger.LogCoreException(e);
                throw e;
            }
            catch (Exception ex) {
                var e = new Exception(String.Format("[Connector]: Unknown Connector Error when calling Service URL {0}", httpMessage.RequestUri), ex);
                Logger.LogCoreException(e);
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