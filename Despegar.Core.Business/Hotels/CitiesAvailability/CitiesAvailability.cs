using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Business.Hotels.CitiesAvailability
{
    public class CitiesAvailability
    {
        public Currencies currencies { get; set; }
        public List<Item> items { get; set; }
        public bool final_result { get; set; }
        public List<Facet> facets { get; set; }
        public Sorting sorting { get; set; }
        public NearbyCity nearby_city { get; set; }
        public Paging paging { get; set; }
    }
}
