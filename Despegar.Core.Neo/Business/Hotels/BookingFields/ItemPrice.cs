using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Hotels.BookingFields
{
    public class ItemPrice
    {
        public Currency currency { get; set; }
        public decimal retention { get; set; }
        public decimal taxes { get; set; }    
        public decimal charges { get; set; }
        public decimal subtotal { get; set; }
        public decimal total { get; set; }
        public Nightly nightly { get; set; }
        public decimal tax_at_destination { get; set; }
        public string promo_type { get; set; }                           
        public decimal discount_percentage { get; set; }       
 
        public decimal totalCharges
        {
            get { return taxes + charges; }
        }

    }
}