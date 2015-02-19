using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Flight.Itineraries
{
    public class Sorting
    {
        public List<SortingOption> values { get; set; }
        public string criteria { get; set; }

        public static Sorting Copy(Sorting original) 
        {
            return new Sorting()
            {
                criteria = original.criteria,
                values = original.values.Select(x => new SortingOption() { label = x.label, selected = x.selected, type = x.type, value = x.value }).ToList()
            };
        }

    }
}
