using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Flight.Itineraries
{
    public class CheapestPrice
    {
        public decimal savings { get; set; }
        public decimal total_price { get; set; }
        public string departure_date { get; set; }
        public string return_date { get; set; }
    }
}
