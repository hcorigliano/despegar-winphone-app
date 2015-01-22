using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Hotels.BookingFields
{
    public class Installments
    {
        public int quantity { get; set; }
        public decimal first { get; set; }
        public decimal others { get; set; }
    }
}