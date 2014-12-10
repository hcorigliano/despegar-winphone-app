﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingFields
{
    public class Card2
    {
        public bool required { get; set; }
        public RegularField number { get; set; }
        public Expiration expiration { get; set; }
        public string data_type { get; set; }
        public RegularField security_code { get; set; }
        public RegularOptionsField owner_type { get; set; }
        public RegularField owner_name { get; set; }
        public OwnerDocument owner_document { get; set; }
        public RegularOptionsField owner_gender { get; set; }
    }
}
