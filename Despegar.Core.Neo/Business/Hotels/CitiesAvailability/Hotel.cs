using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Hotels.CitiesAvailability
{
    public class Hotel
    {
        public string id { get; set; }
        public int stars { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string main_picture { get; set; }
        public double rating { get; set; }
        public int reviews_qty { get; set; }
        public GeoLocation geo_location { get; set; }
        public City city { get; set; }
        public List<Amenity> amenities { get; set; }
    }
}
