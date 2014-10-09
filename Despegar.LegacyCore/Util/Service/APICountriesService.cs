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
    public class APICountriesService
    {

        public static async Task<GeoCountries> GetAll()
        {
            APIConnector ConnectorAPI = APIConnector.Instance;
            HttpRequestMessage httpMessage = ConnectorAPI.MessageBuilder("geo_countries");
            Logger.Info("[connector:req] Countries service called (channel:" + APIConnector.Instance.Channel + "): " + httpMessage.RequestUri.ToString());

            string serviceData = await ConnectorAPI.SendAsync(httpMessage);
            GeoCountries countries = JsonConvert.DeserializeObject<GeoCountries>(serviceData);
            if (countries.errors != null)
                return AppDelegate.Instance.RequestError(new Exception());

            Logger.Info("[connector:res] Countries service received: took " + countries.meta.time);
            return countries;
        }
    }
}
