using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Business.Configuration
{
    public class City
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public GeoLocation geo_location { get; set; }
        public string country_name { get; set; }
        public string country_code { get; set; }
    }
}
