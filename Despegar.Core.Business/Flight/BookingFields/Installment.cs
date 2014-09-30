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
        public FieldDataType complete_card_code { get; set; }
        public FieldDataType card_type { get; set; }
        public FieldDataType bank_code { get; set; }
        public FieldDataType card_code { get; set; }
    }
}
