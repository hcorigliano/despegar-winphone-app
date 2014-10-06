using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Despegar.LegacyCore.Util;
using Despegar.LegacyCore.Connector;
using Despegar.LegacyCore.Connector.Domain.API;
using Despegar.LegacyCore.Model;
using Newtonsoft.Json;
using System.Net.Http;

namespace Despegar.LegacyCore.Service
{
    public static class DPushNotificationService
    {

        public static async Task Register (string data)
        {
            APIConnector ConnectorAPI = APIConnector.Instance;

            #if !DEBUG
            HttpRequestMessage httpMessage = ConnectorAPI.ContentBuilderForMapi("dpns_register", data);
            Logger.Info("[connector:req] DPNS register service called (channel:" + APIConnector.Instance.Channel + "): " + httpMessage.RequestUri.ToString());

            string serviceData = await ConnectorAPI.SendAsync(httpMessage);
            DespegarPushNotification response = JsonConvert.DeserializeObject<DespegarPushNotification>(serviceData);
            #endif
        }

        public static async Task RegisterBooking (string data)
        {
            APIConnector ConnectorAPI = APIConnector.Instance;

            #if !DEBUG
            HttpRequestMessage httpMessage = ConnectorAPI.ContentBuilderForMapi("dpns_register_booking", data);
            Logger.Info("[connector:req] DPNS register booking service called (channel:" + APIConnector.Instance.Channel + "): " + httpMessage.RequestUri.ToString());

            string serviceData = await ConnectorAPI.SendAsync(httpMessage);
            //DespegarPushNotification response = JsonConvert.DeserializeObject<DespegarPushNotification>(serviceData);
            #endif
        }
    }
}