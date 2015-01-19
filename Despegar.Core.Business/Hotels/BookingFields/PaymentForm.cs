using Despegar.Core.Business.Common.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Hotels.BookingFields
{
    public class PaymentForm
    {
        public bool required { get; set; }
        public CardField card { get; set; }
        public InvoiceArg invoice { get; set; }
        public Installment installment { get; set; }
        //public object billingAddress { get; set; }  not used anymore

    }
}
