using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingFields
{
    public class Address
    {
        public bool required { get; set; }
        public RegularField country { get; set; }
        public RegularOptionsField state { get; set; }
        public RegularField city { get; set; }
        public RegularField street { get; set; }
        public RegularField number { get; set; }
        public RegularField floor { get; set; }
        public RegularField department { get; set; }
        public string data_type { get; set; }
        public RegularField city_id { get; set; }
        public RegularField postal_code { get; set; }
    }
}