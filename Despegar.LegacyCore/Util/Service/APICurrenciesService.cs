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
    public class APICurrenciesService
    {


        public static async Task<MiscCurrencies> GetAll()
        {
            APIConnector ConnectorAPI = APIConnector.Instance;
            HttpRequestMessage httpMessage = ConnectorAPI.MessageBuilder("misc_currencies");
            Logger.Info("[connector:req] Currencies service called (channel:" + APIConnector.Instance.Channel + "): " + httpMessage.RequestUri.ToString());

            string serviceData = await ConnectorAPI.SendAsync(httpMessage);
            MiscCurrencies currencies = JsonConvert.DeserializeObject<MiscCurrencies>(serviceData);
            if (currencies.errors != null)
                return AppDelegate.Instance.RequestError(new Exception());

            Logger.Info("[connector:res] Currencies service received: took " + currencies.meta.time);
            return currencies;
        }
    }
}
