using Despegar.Core.Business.Hotels.BookingFields;
using Despegar.Core.Neo.Business.Common.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Hotels.BookingFields
{
    public class PaymentForm
    {
        public bool required { get; set; }
        public CardField card { get; set; }
        public InvoiceArg invoice { get; set; }
        public Installment installment { get; set; }
        public BillingAddress billing_address { get; set; }
    }
}