using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingCompletePost
{
    public class Payment
    {
        public Installment installment { get; set; }
        public Card card { get; set; }
        public Address billing_address { get; set; }
        public string credit_card_token { get; set; }
        public string use_same_credit_card_information { get; set; }
    }
}
