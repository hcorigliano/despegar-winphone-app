using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.Itineraries
{
    public class Segment
    {
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
        public List<object> stopovers { get; set; }
        public OperatedBy operated_by { get; set; }

        public bool HasOperatedBy {get{ return operated_by != null; }}
    }
}
