using Despegar.Core.Neo.Business.Common.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Hotels.BookingFields
{
    public class CheckoutMethodKey
    {
        public bool required { get; set; }
        public PaymentForm payment { get; set; }
        public List<Voucher> vouchers { get; set; }

        public CheckoutMethodKey() 
        {
            //vouchers = new List<RegularField>() { new Voucher() };
        }
    }
}