using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingFields
{
    public class Phone
    {
        public bool required { get; set; }        
        public string data_type { get; set; }
        public int min_quantity { get; set; }
        public RegularOptionsField type { get; set; }
        public RegularField number { get; set; }
        public RegularField country_code { get; set; }
        public RegularField area_code { get; set; }
    }
}