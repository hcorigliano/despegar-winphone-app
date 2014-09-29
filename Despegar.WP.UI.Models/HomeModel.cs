using Despegar.Core.Business;
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
            flightService = GlobalConfiguration.CoreContext.GetFlightService();
        }

        public async Task<string> LoadAirlines()
        {
            return (await flightService.GetAirline("tam-lineas-aereas")).Id;
        }
    }
}