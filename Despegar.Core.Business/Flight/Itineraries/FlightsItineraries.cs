using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Business.Flight.Itineraries
{
    public class FlightsItineraries
    {
        public object cheapest_price { get; set; }
        public Currencies currencies { get; set; }
        public List<Item> items { get; set; }
        public List<Facet> facets { get; set; }
        public Sorting sorting { get; set; }
        public Paging paging { get; set; }
    }
}
