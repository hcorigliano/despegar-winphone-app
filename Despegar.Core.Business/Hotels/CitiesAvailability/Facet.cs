using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Hotels.CitiesAvailability
{
    public class Facet
    {
        public string criteria { get; set; }
        public string label { get; set; }
        public string type { get; set; }
        public List<FacetValue> values { get; set; }
        public MaxAndMinValue value { get; set; }
        public MaxAndMinValue selected { get; set; }
        public object unit { get; set; }
    }
}
