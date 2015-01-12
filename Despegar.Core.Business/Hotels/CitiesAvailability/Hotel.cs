using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Hotels.CitiesAvailability
{
    public class Hotel
    {
        public int id { get; set; }
        public int stars { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string main_picture { get; set; }
        public GeoLocation geo_location { get; set; }
        public double ? rating { get; set; }
        public int reviews_qty { get; set; }
        public List<string> amenities { get; set; }
    }
}
