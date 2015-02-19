using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Flight.BookingCompletePost
{
    public class Installment
    {
        public string quantity { get; set; }
        public string complete_card_code { get; set; }
        public string card_type { get; set; }
        public string bank_code { get; set; }
        public string card_code { get; set; }
    }
}
