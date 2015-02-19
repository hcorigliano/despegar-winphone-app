using Despegar.Core.Neo.Business.Flight.Itineraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model.Classes.Flights
{
    public class BindableSegment
    { 
        public Segment segment;
        public Airport from { get; set; }
        public Airport to { get; set; }
        public string duration { get; set; }
        public Airline airline { get; set; }
        public string flight_id { get; set; }
        public string departure_date { get; set; }
        public string departure_time { get; set; }
        public string arrival_date { get; set; }
        public string arrival_time { get; set; }
        public string cabin_type { get; set; }
        public string AirportCodeFromto { get; set; }
        public string flightIdAndCabinType { get; set; }
        public string operateBy { get; set; }
        // Contains time between flights or total time for trip
        public string timeInformation { get; set; }
        public string toInformation { get; set; }
        public string fromInformation { get; set; }
    }
        
}
