using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Despegar.Core.IService;
using Despegar.Core.Connector;
using Despegar.Core.Connector.Model;

namespace Despegar.Core.Service
{
    public class FlightService : IFlightService
    {
        private MapiConnector _connector;

        public FlightService()
        {
            _connector = new MapiConnector();
        }

        // TODO: Example method, remove
        public async Task<string> GetItineraries(string airlineDescription)
        {
            string serviceUrl = BuildMapiURL("mapi-flights/airlines?description={0}", airlineDescription);
            return await _connector.GetAsync<Airline>(serviceUrl).ToString();
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
            return _connector.GetMapiBaseURL() + serviceUrl;
        }
    }
}
