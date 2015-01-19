using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Hotels.CitiesAvailability
{
    public class Price
    {
        public int @base { get; set; }
        public int best { get; set; }
        public bool final_price { get; set; }
        public Currency currency { get; set; }
        public int? availability { get; set; }
        public int? discount_percentage { get; set; }
        public string promo_type { get; set; }
    }
}
