using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Despegar.LegacyCore.Util;
using Despegar.LegacyCore.Connector;
using Despegar.LegacyCore.Connector.Domain.API;
using Newtonsoft.Json;
using Despegar.LegacyCore.ViewModel;

namespace Despegar.LegacyCore.Service
{
    public static class APIConfigurationService
    {

        public static async Task<Configurations> GetAll()
        {
            APIConnector ConnectorAPI = APIConnector.Instance;
            HttpRequestMessage httpMessage = ConnectorAPI.MessageBuilder("configuration");
            Logger.Info("[connector:req] Configuration service called (channel:" + APIConnector.Instance.Channel + "): " + httpMessage.RequestUri.ToString());

            string serviceData = await ConnectorAPI.SendAsync(httpMessage);
            Configurations configurations = JsonConvert.DeserializeObject<Configurations>(serviceData);
            if (configurations.errors != null)
                return AppDelegate.Instance.RequestError(new Exception());

            Logger.Info("[connector:res] Configuration service received: took " + configurations.meta.time);
            return configurations;
        }
    }
}
