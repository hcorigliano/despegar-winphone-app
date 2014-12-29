using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Despegar.Core.Business.Configuration;
using Despegar.Core.Business;
using Despegar.Core.Connector;
using Despegar.Core.IService;

namespace Despegar.Core.Service
{
    public class ConfigurationService : IConfigurationService
    {
        private CoreContext context;

        public ConfigurationService(CoreContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Retrives Configuration for all Countries.
        /// </summary>
        /// <returns></returns>
        public async Task<Configuration> GetConfigurations()
        {
            string serviceUrl = ServiceURL.GetServiceURL(ServiceKey.Configuration);
            IConnector connector = context.GetServiceConnector(ServiceKey.Configuration);

            return await connector.GetAsync<Configuration>(serviceUrl);
        }

        /// <summary>
        /// Checks if the app needs to be updated
        /// </summary>
        /// <returns></returns>
        public async Task<UpdateFields> CheckUpdate(string AppVersion, string OsVersion, string Source, string Device)     
        {
            string serviceUrl = string.Format(ServiceURL.GetServiceURL(ServiceKey.Update), AppVersion, OsVersion,  Source, Device);
            IConnector connector = context.GetServiceConnector(ServiceKey.Update);

            return await connector.GetAsync<UpdateFields>(serviceUrl);
        }

        public async Task<Countries> GetCountries()
        {
            string serviceUrl = ServiceURL.GetServiceURL(ServiceKey.Countries);
            IConnector connector = context.GetServiceConnector(ServiceKey.Countries);

            return await connector.GetAsync<Countries>(serviceUrl);  

        }

        /// <summary>
        /// Auto completes cities
        /// </summary>
        /// <returns></returns>
        public async Task<List<CitiesFields>> AutoCompleteCities(string CountryCode, string Search, string CityResult)
        {
            string serviceUrl = String.Format(ServiceURL.GetServiceURL(ServiceKey.CitiesAutocomplete),  CountryCode, Search, CityResult);
            IConnector connector = context.GetServiceConnector(ServiceKey.CitiesAutocomplete);

            return await connector.GetAsync<List<CitiesFields>>(serviceUrl);
        

        }

    }
}
