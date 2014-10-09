using Despegar.Core.Business.Flight.CitiesAutocomplete;
using Despegar.Core.Business.Flight.Itineraries;
using Despegar.Core.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model
{
    public class FlightsSearchBoxModel
    {
        private IFlightService flightService;

        public FlightsSearchBoxModel()
        {
            flightService = GlobalConfiguration.CoreContext.GetFlightService();
        }

        public async Task<CitiesAutocomplete> GetCities(string cityString)
        {           
            return (await flightService.GetCitiesAutocomplete(cityString));
        }

        public async Task<FlightsItineraries> GetItineraries(string from, string to, string departure_date, int adults, string return_date, int children, int infants, int offset, int limit, string order_by, string order_type, string currency_code, string filter)
        {
            return (await flightService.GetItinerariesFlights(from, to, departure_date, adults, return_date, children, infants, offset, limit, order_by, order_type, currency_code, filter));
        }

        
    }
}
