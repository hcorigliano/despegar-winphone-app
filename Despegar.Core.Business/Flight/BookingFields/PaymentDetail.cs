using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingFields
{
    public class PaymentDetail
    {
        public string id { get; set; }
        public Card card { get; set; }
        public Installments installments { get; set; }
        public double interest { get; set; }
        public double financial_cost { get; set; }
        public bool interest_by_bank { get; set; }

        // Custom
        public List<PaymentDetail> AsSimpleCardList { get { return new List<PaymentDetail>() { this }; } }
        public string CustomErrorType { get; set; }
        public bool hasError { get; set; }
    }
}