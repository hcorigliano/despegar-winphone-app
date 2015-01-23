using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Business.Hotels.HotelsAutocomplete
{
    public class HotelAutocomplete
    {
        public string name { get; set; }
        public int id { get; set; }
        public string type { get; set; }
        public GeoLocation geo_location { get; set; }
        public string code { get; set; }
        public string country_code { get; set; }
        public bool has_airport { get; set; }
        public int? city_id { get; set; }


        public override string ToString()
        {
            return this.name;
        }
    }
}
