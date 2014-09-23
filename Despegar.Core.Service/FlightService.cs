using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Despegar.Core.IService;
using Despegar.Core.Connector;

namespace Despegar.Core.Service
{
    public class FlightService : IFlightService
    {
        private MapiConnector _connector;

        public FlightService()
        {
            _connector = new MapiConnector();
        }

        /// <summary>
        /// Get the itineraries from connector
        /// </summary>
        /// <param name="airportCode"> this parameter must be change it.</param>
        /// <returns>this parameter must be change it.</returns>
        public string GetItineraries(string airportCode)
        {
            //TODO this line is an example please change the method invoked.
            
            return _connector.GetString();
        }
    }
}
