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
    public class APICitiesService
    {
        public static async Task<CitiesFields> GetAll(string stringBusqueda, int idState)
        {
            APIConnector ConnectorAPI = APIConnector.Instance;
            HttpRequestMessage httpMessage = ConnectorAPI.MessageBuilderForMapi("cities");

            httpMessage.RequestUri = new Uri(String.Format(httpMessage.RequestUri.ToString(), ApplicationConfig.Instance.Country, stringBusqueda,idState), UriKind.RelativeOrAbsolute);
            Logger.Info("[connector:req] Cities service called (channel:" + APIConnector.Instance.Channel + "): " + httpMessage.RequestUri.ToString());
            string serviceData = await ConnectorAPI.SendAsync(httpMessage);
            CitiesFields cities = JsonConvert.DeserializeObject<CitiesFields>(serviceData);
            
            return cities;
        }
    }
}
