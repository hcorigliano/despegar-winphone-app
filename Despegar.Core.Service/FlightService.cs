using Despegar.Core.Business.Flight;
using Despegar.Core.Connector;
using Despegar.Core.IService;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Diagnostics;
using Despegar.Core.Business.Flight.CitiesAutocomplete;

namespace Despegar.Core.Service
{
    public class FlightService : IFlightService
    {
        private CoreContext context;

        public FlightService(CoreContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Retrieves an airline info
        /// </summary>
        /// <param name="airlineDescription">the airline description</param>
        /// <returns></returns>
        public async Task<Airline> GetAirline(string searchString)
        {
            string serviceUrl = String.Format(ServiceURL.GetServiceURL(ServiceKey.FlightCitiesAutocomplete), searchString);
            IConnector connector = context.GetServiceConnector(ServiceKey.FlightCitiesAutocomplete);

            return await connector.GetAsync<Airline>(serviceUrl);
        }

        /// <summary>
        /// Retrieves an list of Cities
        /// </summary>
        /// <param name="cityString"></param>
        /// <returns></returns>
        public async Task<CitiesAutocomplete> GetCitiesAutocomplete(string cityString)
        {
            string serviceUrl = String.Format(ServiceURL.GetServiceURL(ServiceKey.FlightCitiesAutocomplete), cityString);
            IConnector connector = context.GetServiceConnector(ServiceKey.FlightCitiesAutocomplete);

            return await connector.GetAsync<CitiesAutocomplete>(serviceUrl);
        }
    }
}