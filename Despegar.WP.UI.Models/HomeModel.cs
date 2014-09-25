using Despegar.Core.Business.Flight;
using Despegar.Core.Service;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Despegar.WP.UI.Model
{
    public class HomeModel
    {
        //TODO all this code must be changed it is just for first time only.
        private FlightService flight = new FlightService("WindowsPhone8App");

        public string myfirsttext
        {
            get;
            set;
        }

        public async Task<string> LoadAirlines()
        {
            return (await flight.GetAirline("tam-lineas-aereas")).Id;
        }
    }
}
