using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Hotels.BookingFields
{
    public class PaymentOptions
    {
        public List<HotelPayment> at_destination { get; set; }
        public List<HotelPayment> without_interest { get; set; }
        public List<HotelPayment> with_interest { get; set; }
        public object messages { get; set; }
    }
}