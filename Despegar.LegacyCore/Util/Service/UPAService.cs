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
    public static class UPAService
    {
        public static async Task<string> Get()
        {
            APIConnector ConnectorAPI = APIConnector.Instance;

            HttpRequestMessage httpMessage = ConnectorAPI.MessageBuilderForUpa("get_id");
            Logger.Info("[connector:req] UPA service called: " + httpMessage.RequestUri.ToString());

            string serviceData = await ConnectorAPI.SendAsync(httpMessage);
            UPAResponse response = JsonConvert.DeserializeObject<UPAResponse>(serviceData);
            Logger.Info("[connector:res] get UPA id service received: " + response.id);
            return response.id;
        }
    }
}