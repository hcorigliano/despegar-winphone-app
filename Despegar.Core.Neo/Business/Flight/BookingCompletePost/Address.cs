using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Flight.BookingCompletePost
{
    public class Address
    {
        public string country { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string city_code { get; set; }
        public string postal_code { get; set; }
        public string street { get; set; }
        public string number { get; set; }
        public string floor { get; set; }
        public string department { get; set; }
    }
}
