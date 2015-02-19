using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Flight.BookingCompletePost
{
    public class Passenger
    {
        public string type { get; set; }
        public string first_name { get; set; }
        public string mid_name { get; set; }
        public string last_name { get; set; }
        public Document document { get; set; }
        public string gender { get; set; }
        public string nationality { get; set; }
        public string birthdate { get; set; }
        public string frequent_flyer_number { get; set; }
    }
}
