using Despegar.Core.Business.Flight.CitiesAutocomplete;
using Despegar.Core.Business.Flight.Itineraries;
using Despegar.Core.IService;
using Despegar.Core.Service;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model
{
    public class HomeModel
    {
        //TODO all this code must be changed it is just for first time only.
        private IFlightService flightService;

        public HomeModel() 
        {
            // TODO: CoreContext should be a Singleton for the APP
            var context = new CoreContext();
            context.Configure("WindowsPhoneApp8", "deviceID", "AR", "ES");
            
            //TODO: Sacar Mocked
            //context.AddMock(ServiceKey.FlightCitiesAutocomplete, MockKey.FlightCitiesAutocompleteBue);
            //context.AddMock(ServiceKey.FlightItineraries, MockKey.ItinerarieBueToLax);

            flightService = context.GetFlightService();
            GetCities("bue");
            GetItineraries("BUE","LAX","2014-10-10", 1, "2014-10-12",0,0,0,10,"","","ARS","");
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

    }
}