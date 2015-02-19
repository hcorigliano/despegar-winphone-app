using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Hotels.CitiesAvailability
{
    public class  Sorting
    {
        public string criteria { get; set; }
        public List<Value> values { get; set; }

        public static Sorting Copy(Sorting original)
        {
            return new Sorting() 
            {
                criteria = original.criteria,
                values = original.values.Select(x => new Value() {count = x.count, value = x.value, selected = x.selected, label = x.label }).ToList()
            };
        }
    }
}
