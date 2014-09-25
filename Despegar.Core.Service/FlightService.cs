using Despegar.Core.Business.Flight;
using Despegar.Core.Connector;
using Despegar.Core.IService;
using System;
using System.Threading.Tasks;

namespace Despegar.Core.Service
{
    public class FlightService : IFlightService
    {
        private MapiConnector _connector;

        public FlightService(string xClient)
        {
            _connector = new MapiConnector(xClient);
        }
        
        /// <summary>
        /// Retrieves an airline info
        /// </summary>
        /// <param name="airlineDescription">the airline description</param>
        /// <returns></returns>
        public async Task<Airline> GetAirline(string airlineDescription)
        {
            string serviceUrl = BuildMapiURL("mapi-flights/airlines?description={0}", airlineDescription);
            return await _connector.GetAsync<Airline>(serviceUrl);
        }

        /// <summary>
        /// Arranges the Mapi Service URL replacing the params
        /// </summary>
        /// <param name="pattern">Service Relative URL pattern to format</param>
        /// <param name="parameters">Parameters to include in the URL</param>
        /// <returns></returns>
        private string BuildMapiURL(string pattern, params string[] parameters)
        {
            string serviceUrl = String.Format(pattern, parameters);
            return _connector.GetBaseUrl() + serviceUrl;
        }
        
    }
}