using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Business.Flight.Itineraries
{
    public class Facet
    {
        public string criteria { get; set; }
        public string label { get; set; }
        public string type { get; set; }
        public List<FacetValue> values { get; set; }

        public static Facet Copy(Facet original)
        {
            return new Facet()
            {
                criteria = original.criteria,
                label = original.label,
                type = original.type,
                values = original.values.Select(x => new FacetValue() { 
                    count = x.count, 
                    value = x.value, 
                    label = x.label, 
                    selected = x.selected}).ToList()
            };
        }
    }
}
