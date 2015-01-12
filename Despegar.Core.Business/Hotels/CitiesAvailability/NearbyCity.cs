using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Hotels.CitiesAvailability
{
    public class NearbyCity
    {
        public string code { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int distance { get; set; }
    }
}
