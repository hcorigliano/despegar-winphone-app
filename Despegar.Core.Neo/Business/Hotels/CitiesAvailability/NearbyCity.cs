using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Hotels.CitiesAvailability
{
    public class NearbyCity
    {
        public string id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public IdCodeAndName country { get; set; }
        public string distance { get; set; }
    }
}
