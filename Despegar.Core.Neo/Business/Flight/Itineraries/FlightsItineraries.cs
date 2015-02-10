using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Business.Flight.Itineraries
{
    public class FlightsItineraries
    {
        public int total { get; set; }
        public CheapestPrice cheapest_price { get; set; }
        public Currencies currencies { get; set; }
        public List<Item> items { get; set; }
        public List<Facet> facets { get; set; }
        public Sorting sorting { get; set; }
        public Paging paging { get; set; }
    }
}
