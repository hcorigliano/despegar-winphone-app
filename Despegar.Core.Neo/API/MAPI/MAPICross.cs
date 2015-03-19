using Despegar.Core.Neo.Business.Common.State;
using Despegar.Core.Neo.Business.Configuration;
using Despegar.Core.Neo.Contract;
using Despegar.Core.Neo.Contract.API;
using Despegar.Core.Neo.Contract.Connector;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Despegar.Core.Neo.API.MAPI
{
    public class MAPICross : IMAPICross
    {
        private ICoreContext context;
        private IMapiConnector connector;

        public MAPICross(ICoreContext context, IMapiConnector connector)
        {
            this.context = context;
            this.connector = connector;
        }

        /// <summary>
        /// Retrives Configuration for all Countries.
        /// </summary>
        public async Task<Configuration> GetConfigurations()
        {            
            string serviceUrl = ServiceURL.GetServiceURL(ServiceKey.Configuration);

            return await connector.GetAsync<Configuration>(serviceUrl, ServiceKey.Configuration);
        }

        public async Task<List<State>> GetStates(string country)
        {
            string serviceUrl = ServiceURL.GetServiceURL(ServiceKey.States, country);

            var states =  await connector.GetAsync<List<State>>(serviceUrl, ServiceKey.States);
            return states.OrderBy(x => x.name).ToList();            
        }

        /// <summary>
        /// Checks if the app needs to be updated
        /// </summary>
        public async Task<UpdateFields> CheckUpdate(string AppVersion, string OsVersion, string Source, string Device)
        {            
            string serviceUrl = ServiceURL.GetServiceURL(ServiceKey.Update, AppVersion, OsVersion, Source, Device);

            return await connector.GetAsync<UpdateFields>(serviceUrl, ServiceKey.Update);
        }

        public async Task<Countries> GetCountries()
        {
            string serviceUrl = ServiceURL.GetServiceURL(ServiceKey.Countries);

            var countries = await connector.GetAsync<Countries>(serviceUrl, ServiceKey.Countries);
            countries.countries = countries.countries.OrderBy(x => x.name).ToList();
            return countries;
        }

        /// <summary>
        /// Auto completes cities
        /// </summary>
        public async Task<List<CitiesFields>> AutoCompleteCities(string CountryCode, string Search, string CityResult)
        {
            string serviceUrl = ServiceURL.GetServiceURL(ServiceKey.CitiesAutocomplete, CountryCode, Search, CityResult);

            return await connector.GetAsync<List<CitiesFields>>(serviceUrl, ServiceKey.CitiesAutocomplete);
        }


        public async Task<City> GetCity(string CityId)
        {
            string serviceUrl = ServiceURL.GetServiceURL(ServiceKey.City, CityId);

            return await connector.GetAsync<City>(serviceUrl, ServiceKey.City);
        }
    }
}