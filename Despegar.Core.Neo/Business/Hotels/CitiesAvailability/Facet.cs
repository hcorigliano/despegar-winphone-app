using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Hotels.CitiesAvailability
{
    public class Facet
    {
        public string criteria { get; set; }
        public string label { get; set; }
        public string subtype { get; set; }
        public List<Value> values { get; set; }
        public string type { get; set; }
        public string unit { get; set; }
        public MaxAndMin value { get; set; }
        public object selected { get; set; }

        public static Facet Copy(Facet original) 
        {
            return new Facet()
            {
                criteria = original.criteria,
                label = original.label,
                subtype = original.label,
                type = original.type,
                unit = original.unit,
                selected = original.selected,
                value = new MaxAndMin() { max = original.value.max, min = original.value.min },
                values = original.values.Select(x => new Value() { count = x.count, label = x.label, selected = x.selected, value = x.value}).ToList()
            };
        }
    }
}