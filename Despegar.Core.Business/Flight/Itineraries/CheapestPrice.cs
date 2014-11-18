using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.Itineraries
{
    public class CheapestPrice
    {
        public int savings { get; set; }
        public int total_price { get; set; }
        public string departure_date { get; set; }
        public string return_date { get; set; }
    }
}
