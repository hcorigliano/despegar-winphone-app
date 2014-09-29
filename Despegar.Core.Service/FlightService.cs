
using Despegar.Core.Business;
using Despegar.Core.Business.Flight.Airline;
using Despegar.Core.Connector;
using Despegar.Core.IService;
using System;
using System.Threading.Tasks;

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
            string serviceUrl = String.Format(ServiceURL.GetServiceURL(ServiceKey.FlightsAirlines), searchString);
            IConnector connector = context.GetServiceConnector(ServiceKey.FlightsAirlines);

            return await connector.GetAsync<Airline>(serviceUrl);
        }
    }
}