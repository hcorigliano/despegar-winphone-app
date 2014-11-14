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
    class ConfigurationService : IConfigurationService
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
        public async Task<UpdateFields> CheckUpdate()     
        {
            string serviceUrl = ServiceURL.GetServiceURL(ServiceKey.Update);
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
        public async Task<CitiesFields> AutoCompleteCities(string CountryCode, string Search, string CityResult)
        {
            string serviceUrl = ServiceURL.GetServiceURL(ServiceKey.CitiesAutocomplete);
            String.Format(serviceUrl, CountryCode, Search, CityResult);
            IConnector connector = context.GetServiceConnector(ServiceKey.CitiesAutocomplete);

            return await connector.GetAsync<CitiesFields>(serviceUrl);

        }

    }
}
