// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved

using Despegar.WP.UI.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.PushNotifications;
using Windows.Storage;

namespace Despegar.WP.UI.PushNotificationHelper
{
    public sealed class ChannelAndWebResponse
    {
        public PushNotificationChannel Channel { get; set; }
        public String WebResponse { get; set; }
    }

    [DataContract]
    internal class UrlData
    {
        [DataMember]
        public String Url;
        [DataMember]
        public String ChannelUri;
        [DataMember]
        public bool IsAppId;
        [DataMember]
        public DateTime Renewed;
    }

    public sealed class Notifier
    {
#if DECOLAR
            private const String APP_TILE_ID_KEY = "DecolarTileIds";
#else
        private const String APP_TILE_ID_KEY = "DespegarTileIds";
#endif

#if DECOLAR
            private const String MAIN_APP_TILE_KEY = "Decolar.com";
#else
        private const String MAIN_APP_TILE_KEY = "Despegar.com";
#endif

        private const int DAYS_TO_RENEW = 15; // Renew if older than 15 days
        private Dictionary<String, UrlData> urls;

        public Notifier()
        {
            this.urls = new Dictionary<String, UrlData>();
            List<String> storedUrls = null;
            IPropertySet currentData = ApplicationData.Current.LocalSettings.Values;

            try
            {
                String urlString = (String)currentData[APP_TILE_ID_KEY];
                using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(urlString)))
                {
                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(List<String>));
                    storedUrls = (List<String>)deserializer.ReadObject(stream);
                }
            }
            catch (Exception) { }

            if (storedUrls != null)
            {
                for (int i = 0; i < storedUrls.Count; i++)
                {
                    String key = storedUrls[i];
                    try
                    {
                        String dataString = (String)currentData[key];
                        using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(dataString)))
                        {
                            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(UrlData));
                            this.urls[key] = (UrlData)deserializer.ReadObject(stream);
                        }
                    }
                    catch (Exception) { }
                }
            }
        }

        private UrlData TryGetUrlData(String key)
        {
            UrlData returnedData = null;
            lock (this.urls)
            {
                if (this.urls.ContainsKey(key))
                {
                    returnedData = this.urls[key];
                }
            }

            return returnedData;
        }

        private void SetUrlData(String key, UrlData dataToSet)
        {
            lock (this.urls)
            {
                this.urls[key] = dataToSet;
            }
        }

        // Update the stored target URL
        private void UpdateUrl(String url, String channelUri, String inputItemId, bool isPrimaryTile)
        {
            String itemId = isPrimaryTile && inputItemId == null ? MAIN_APP_TILE_KEY : inputItemId;

            bool shouldSerializeTileIds = TryGetUrlData(itemId) == null;
            UrlData storedData = new UrlData() { Url = url, ChannelUri = channelUri, IsAppId = isPrimaryTile, Renewed = DateTime.Now };
            SetUrlData(itemId, storedData);

            using (MemoryStream stream = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(UrlData));
                serializer.WriteObject(stream, storedData);
                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream))
                {
                    ApplicationData.Current.LocalSettings.Values[itemId] = reader.ReadToEnd();
                }
            }

            if (shouldSerializeTileIds)
            {
                SaveAppTileIds();
            }
        }

        private void SaveAppTileIds()
        {
            List<String> dataToStore;

            lock (this.urls)
            {
                dataToStore = new List<String>(this.urls.Count);
                foreach (String key in this.urls.Keys)
                {
                    dataToStore.Add(key);
                }
            }

            using (MemoryStream stream = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<String>));
                serializer.WriteObject(stream, dataToStore);
                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream))
                {
                    ApplicationData.Current.LocalSettings.Values[APP_TILE_ID_KEY] = reader.ReadToEnd();
                }
            }
        }

        // This method checks the freshness of each channel, and returns as necessary
        public IAsyncAction RenewAllAsync(bool force)
        {
            DateTime now = DateTime.Now;
            TimeSpan daysToRenew = new TimeSpan(DAYS_TO_RENEW, 0, 0, 0);
            List<Task<ChannelAndWebResponse>> renewalTasks;
            lock (this.urls)
            {
                renewalTasks = new List<Task<ChannelAndWebResponse>>(this.urls.Count);
                foreach (var keyValue in this.urls)
                {
                    UrlData dataForUpload = keyValue.Value;
                    if (force || ((now - dataForUpload.Renewed) > daysToRenew))
                    {
                        string upaid = GlobalConfiguration.UPAId;
                        string brand = GlobalConfiguration.Brand;
                        string language = GlobalConfiguration.Language;
                        if (keyValue.Key == MAIN_APP_TILE_KEY)
                        {
                            renewalTasks.Add(OpenChannelAndUploadAsync(dataForUpload.Url, upaid, brand, language).AsTask());
                        }
                        else
                        {
                            renewalTasks.Add(OpenChannelAndUploadAsync(dataForUpload.Url, keyValue.Key, dataForUpload.IsAppId,upaid,brand,language).AsTask());
                        }

                    }
                }
            }
            return Task.WhenAll(renewalTasks).AsAsyncAction();
        }

        // Instead of using the async and await keywords, actual Tasks will be returned.
        // That way, components consuming these APIs can await the returned tasks
        public IAsyncOperation<ChannelAndWebResponse> OpenChannelAndUploadAsync(String url, string upaId, string brand, string countryId)
        {
            IAsyncOperation<PushNotificationChannel> channelOperation = PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            return ExecuteChannelOperation(channelOperation, url, MAIN_APP_TILE_KEY, true,  upaId,  brand,  countryId);
        }

        public IAsyncOperation<ChannelAndWebResponse> OpenChannelAndUploadAsync(String url, String inputItemId, bool isPrimaryTile, string upaId, string brand, string countryId)
        {
            IAsyncOperation<PushNotificationChannel> channelOperation;
            if (isPrimaryTile)
            {
                channelOperation = PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync(inputItemId);
            }
            else
            {
                channelOperation = PushNotificationChannelManager.CreatePushNotificationChannelForSecondaryTileAsync(inputItemId);
            }

            return ExecuteChannelOperation(channelOperation, url, inputItemId, isPrimaryTile, upaId, brand, countryId);
        }

        private IAsyncOperation<ChannelAndWebResponse> ExecuteChannelOperation(IAsyncOperation<PushNotificationChannel> channelOperation, String url, String itemId, bool isPrimaryTile, string upaId, string brand, string countryId)
        {
            return channelOperation.AsTask().ContinueWith<ChannelAndWebResponse>((Task<PushNotificationChannel> channelTask) =>
            {
                PushNotificationChannel newChannel = channelTask.Result;
                String webResponse = "URI already uploaded";

                // Upload the channel URI if the client hasn't recorded sending the same uri to the server
                UrlData dataForItem = TryGetUrlData(itemId);

                if (dataForItem == null || newChannel.Uri != dataForItem.ChannelUri)
                {
                    HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                    webRequest.Method = "PUT";
                    webRequest.ContentType = "application/x-www-form-urlencoded";

                    //byte[] channelUriInBytes = Encoding.UTF8.GetBytes("ChannelUri=" + WebUtility.UrlEncode(newChannel.Uri) + "&ItemId=" + WebUtility.UrlEncode(itemId));

                    byte[] channelUriInBytes = Encoding.UTF8.GetBytes("upa_id=" + WebUtility.UrlDecode(upaId) + "&token=" + WebUtility.UrlEncode(newChannel.Uri) + "&brand=" + WebUtility.UrlEncode(brand) + "&device_type=" + WebUtility.UrlEncode("wphone") + "&country_id=" + countryId);
                    Task<Stream> requestTask = webRequest.GetRequestStreamAsync();
                    using (Stream requestStream = requestTask.Result)
                    {
                        requestStream.Write(channelUriInBytes, 0, channelUriInBytes.Length);
                    }

                    Task<WebResponse> responseTask = webRequest.GetResponseAsync();
                    using (StreamReader requestReader = new StreamReader(responseTask.Result.GetResponseStream()))
                    {
                        webResponse = requestReader.ReadToEnd();
                    }
                }

                // Only update the data on the client if uploading the channel URI succeeds.
                // If it fails, you may considered setting another AC task, trying again, etc.
                // OpenChannelAndUploadAsync will throw an exception if upload fails
                UpdateUrl(url, newChannel.Uri, itemId, isPrimaryTile);

                return new ChannelAndWebResponse { Channel = newChannel, WebResponse = webResponse };
            }).AsAsyncOperation();
        }
    }
}
