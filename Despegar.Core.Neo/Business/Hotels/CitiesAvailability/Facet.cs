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
    }
}
