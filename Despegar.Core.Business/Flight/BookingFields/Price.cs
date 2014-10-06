using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingFields
{
    public class Price
    {
        public Currency currency { get; set; }
        public double total { get; set; }
        public double taxes { get; set; }
        public double retention { get; set; }
        public double charges { get; set; }
        public double adult_base { get; set; }
        public double adults_subtotal { get; set; }
        public object children_subtotal { get; set; }
        public object infants_subtotal { get; set; }
        public bool final_price { get; set; }
    }
}
