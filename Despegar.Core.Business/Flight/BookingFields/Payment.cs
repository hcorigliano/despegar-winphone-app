using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingFields
{
    public class Payment
    {
        public bool required { get; set; }
        public Installment installment { get; set; }
        public Card2 card { get; set; }
        public Invoice invoice { get; set; }
        public string data_type { get; set; }
    }
}
