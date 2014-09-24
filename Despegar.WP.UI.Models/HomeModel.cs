using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Despegar.Core.IService;
using Despegar.Core.Service;

namespace Despegar.WP.UI.Model
{
    public class HomeModel
    {
        //TODO all this code must be changed it is just for first time only.
        private FlightService flight = new FlightService();

        public string myfirsttext
        {
            get
            {
                return flight.GetItineraries("jajajaoa");
            }
        }
    }
}
