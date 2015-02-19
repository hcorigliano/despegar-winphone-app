using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Business.Flight.Itineraries
{
    public class Route
    {
        public int choice { get; set; }
        public string duration { get; set; }
        public Airport from { get; set; }
        public Airport to { get; set; }
        public string departure { get; set; }
        public string arrival { get; set; }
        public int layovers { get; set; }
        public List<Segment> segments { get; set; }
    }
}
