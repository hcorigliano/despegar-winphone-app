using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingFields
{
    public class Payments
    {
        public object pay_at_destination { get; set; } //TODO: Verify.
        public List<PaymentDetail> with_interest { get; set; }
        public List<PaymentDetail> without_interest { get; set; }
    }
}
