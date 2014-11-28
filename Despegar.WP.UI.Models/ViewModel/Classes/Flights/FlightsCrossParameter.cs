using Despegar.Core.Business.Flight.Itineraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model.ViewModel.Classes.Flights
{
    public class FlightsCrossParameter
    {
        public Route Inbound { get; set; }
        public Route Outbound { get; set; }
        public string FlightId { get; set; }
    }
}
