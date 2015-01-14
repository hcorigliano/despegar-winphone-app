using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Hotels.BookingFields
{
    public class CheckoutMethodKey
    {
        public bool required { get; set; }
        public PaymentForm payment { get; set; }
    }
}
