using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Flight.BookingCompletePost
{
    public class Invoice
    {
        public string card_holder_name { get; set; }
        public string fiscal_status { get; set; }
        public string fiscal_id { get; set; }
        public Address address { get; set; }
    }
}
