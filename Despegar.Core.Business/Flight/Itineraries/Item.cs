using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Business.Flight.Itineraries
{
    public class Item
    {
        public string id { get; set; }
        public Currency currency { get; set; }
        public Airline airline { get; set; }
        public int price { get; set; }
        public List<Outbound> outbound { get; set; }
        public List<Inbound> inbound { get; set; }
        public bool final_price { get; set; }
        public string destination_type { get; set; }
        public string destination_country_code { get; set; }
        public string validating_carrier { get; set; }
    }
}
