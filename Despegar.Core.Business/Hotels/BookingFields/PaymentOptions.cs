using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Hotels.BookingFields
{
    public class PaymentOptions
    {
        public List<Payment> at_destination { get; set; }
        public List<Payment> without_interest { get; set; }
        public List<Payment> with_interest { get; set; }
        public object messages { get; set; }
    }
}