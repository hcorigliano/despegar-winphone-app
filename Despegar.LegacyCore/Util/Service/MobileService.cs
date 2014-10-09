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
    public static class MobileService
    {
        public static async Task<Discounts> GetDiscounts()
        {
            APIConnector ConnectorAPI = APIConnector.Instance;

            HttpRequestMessage httpMessage = ConnectorAPI.MessageBuilderForMobile("discounts");
            Logger.Info("[connector:req] Discounts service called: " + httpMessage.RequestUri.ToString());

            string serviceData = await ConnectorAPI.SendAsync(httpMessage);
            Discounts response = JsonConvert.DeserializeObject<Discounts>(serviceData);
            Logger.Info("[connector:res] get discounts service received");
            return response;
        }


        public static async Task<Discounts> GetRemoteConfiguration()
        {
            APIConnector ConnectorAPI = APIConnector.Instance;

            HttpRequestMessage httpMessage = ConnectorAPI.MessageBuilderForMobile("remote_configuration");
            Logger.Info("[connector:req] Remote Configuration service called: " + httpMessage.RequestUri.ToString());

            string serviceData = await ConnectorAPI.SendAsync(httpMessage);
            Discounts response = JsonConvert.DeserializeObject<Discounts>(serviceData);
            Logger.Info("[connector:res] get remote config service received");
            return response;
        }
    }
}