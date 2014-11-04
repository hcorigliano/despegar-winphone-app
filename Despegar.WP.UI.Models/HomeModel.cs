using Despegar.Core.Business;
using Despegar.Core.Business.Configuration;
using Despegar.Core.Business.Flight.BookingCompletePost;
using Despegar.Core.Business.Flight.BookingCompletePostResponse;
using Despegar.Core.Business.Flight.BookingFields;
using Despegar.Core.Business.Flight.CitiesAutocomplete;
using Despegar.Core.Business.Flight.Itineraries;
using Despegar.Core.IService;
using Despegar.Core.Service;
using System.Dynamic;
using System.Threading.Tasks;


namespace Despegar.WP.UI.Model
{
    public class HomeModel
    {
        //TODO all this code must be changed it is just for first time only.
        private IFlightService flightService;
        private IConfigurationService configurationService;

        public HomeModel()             
        {
		    flightService = GlobalConfiguration.CoreContext.GetFlightService();
            configurationService = GlobalConfiguration.CoreContext.GetConfigurationService();
            
        }

       
        public async Task<CitiesAutocomplete> GetCities(string cityString)
        {
            cityString = "bue";
            return (await flightService.GetCitiesAutocomplete(cityString));
        }

        
        public async Task<FlightsItineraries> GetItineraries(string from, string to, string departure_date, int adults, string return_date, int children, int infants, int offset, int limit, string order_by, string order_type, string currency_code, string filter)
        {
            return ( await flightService.GetItinerariesFlights(from,to,departure_date,adults,return_date,children,infants,offset,limit,order_by,order_type,currency_code,filter));
        }

        //Gets the fields for a booking
        public async Task<BookingFields> GetBooking(BookingFieldPost bookingFieldPost)
        {
            return (await flightService.GetBookingFields(bookingFieldPost));
        }


        public async Task<Configuration> GetConfigurations()
        {
            return (await configurationService.GetConfigurations());
        }

        //post the complete booking
        public async Task<BookingCompletePostResponse> PostBooking(object bookingCompletePost, string id)
        {
            return (await flightService.CompleteBooking(bookingCompletePost, id));
        }

        public void test()
        {
            
            
        }

    }
}