using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Business.Hotels.CitiesAvailability
{
    public class CitiesAvailability
    {
        public Sorting sorting { get; set; }
        public Extra extra { get; set; }
        public Paging paging { get; set; }
        public List<HotelItem> items { get; set; }
        public List<Facet> facets { get; set; }
        public Currencies currencies { get; set; }
        public HotelsSearchParameters searchDetails { get; set; }
        public SearchStates SearchStatus { get; set; }
    }
}
