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
            context.AddMock(ServiceKey.FlightCitiesAutocomplete, MockKey.FlightCitiesAutocompleteBue);

            flightService = context.GetFlightService();
        }

        public async Task<string> LoadAirlines()
        {
            return (await flightService.GetAirline("tam-lineas-aereas")).Id;
        }
    }
}