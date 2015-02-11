using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Business.Hotels
{
    public class HotelsSearchParameters
    {        
        public string Checkin { get; set; }
        public string Checkout { get; set; }
        public int destinationNumber { get; set; }
        public string distribution { get; set; }
        public string currency { get; set; }
        public int offset { get; set; }
        public int limit { get; set; }
        public string order { get; set; }
        public string extraParameters { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
}