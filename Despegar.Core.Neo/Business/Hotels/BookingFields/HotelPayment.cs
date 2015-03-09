using Despegar.Core.Neo.Business.Common.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Hotels.BookingFields
{
    public class HotelPayment
    {
        public string id { get; set; }
        public string type { get; set; }
        public string subtype { get; set; }
        public Card card { get; set; }
        public PaymentInstallments installments { get; set; }
        public bool interest_by_bank { get; set; }
        public bool accept_cash { get; set; }
        public decimal financial_cost { get; set; }
    }
}