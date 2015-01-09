using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Despegar.LegacyCore.Connector;
using Despegar.LegacyCore.Connector.Domain.API;
using System.Net.Http;
using Despegar.LegacyCore.Util;
using Newtonsoft.Json;

namespace Despegar.LegacyCore.Service
{
    public class APIValidationCreditcards
    {
        public static async Task<ValidationCreditcards> GetAll()
        {
            APIConnector ConnectorAPI = APIConnector.Instance;
            HttpRequestMessage httpMessage = ConnectorAPI.MessageBuilder("validation_creditcards");
            Logger.Info("[connector:req] Creditcards service called (channel:" + APIConnector.Instance.Channel + "): " + httpMessage.RequestUri.ToString());

            string serviceData = await ConnectorAPI.SendAsync(httpMessage);
            ValidationCreditcards data = JsonConvert.DeserializeObject<ValidationCreditcards>(serviceData);
            Logger.Info("[connector:res] Creditcards service received.");
            return data;
        }
    }
}