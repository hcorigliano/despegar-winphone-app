using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingFields
{
    public class Installment
    {
        public bool required { get; set; }
        public Quantity quantity { get; set; }
        public string data_type { get; set; }
        public RegularField complete_card_code { get; set; }
        public RegularField card_type { get; set; }
        public RegularField bank_code { get; set; }
        public RegularField card_code { get; set; }
    }
}
