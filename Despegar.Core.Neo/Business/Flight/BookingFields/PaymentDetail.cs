using Despegar.Core.Neo.Business.Common.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Flight.BookingFields
{
    public class PaymentDetail
    {
        public string id { get; set; }
        public Card card { get; set; }
        public PaymentInstallments installments { get; set; }
        public double interest { get; set; }
        public double financial_cost { get; set; }
        public bool interest_by_bank { get; set; }

        // Custom
        public List<PaymentDetail> AsSimpleCardList { get { return new List<PaymentDetail>() { this }; } }        
    }
}