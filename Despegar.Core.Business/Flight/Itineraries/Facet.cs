using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Business.Flight.Itineraries
{
    public class Facet
    {
        public string criteria { get; set; }
        public string label { get; set; }
        public string type { get; set; }
        public List<Value2> values { get; set; }
    }
}
