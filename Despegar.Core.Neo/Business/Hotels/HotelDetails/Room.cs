using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Hotels.HotelDetails
{
    public class Room
    {
        public string code { get; set; }
        public string reference { get; set; }
        public string name { get; set; }
        public int max_capacity { get; set; }
        public int availability { get; set; }
        public List<BedOption> bed_options { get; set; }
        public string information { get; set; }
        public List<Amenity> amenities { get; set; }
        public List<string> pictures { get; set; }
    }
}
