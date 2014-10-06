using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Despegar.LegacyCore.Connector;
using Despegar.LegacyCore.Connector.Domain.API;
using System.Net.Http;
using Newtonsoft.Json;
using Despegar.LegacyCore.Util;
using Despegar.LegacyCore.ViewModel;


namespace Despegar.LegacyCore.Service
{
    public class APIStatesService
    {

        public static async Task<StatesFields> GetAll()
        {
            APIConnector ConnectorAPI = APIConnector.Instance;
            HttpRequestMessage httpMessage = ConnectorAPI.MessageBuilder("states");

            httpMessage.RequestUri = new Uri(String.Format(httpMessage.RequestUri.ToString(), ApplicationConfig.Instance.Country), UriKind.RelativeOrAbsolute);
            Logger.Info("[connector:req] States service called (channel:" + APIConnector.Instance.Channel + "): " + httpMessage.RequestUri.ToString());
            string serviceData = await ConnectorAPI.SendAsync(httpMessage);
            StatesFields states = JsonConvert.DeserializeObject<StatesFields>(serviceData);
            if (states.errors != null)
                return AppDelegate.Instance.RequestError(new Exception());

            Logger.Info("[connector:res] States service received: took " + states.meta.time);
            return states;
        }

    }
}
