﻿using Despegar.Core.Neo.Business.Common.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Hotels.BookingFields
{
    public class BillingAddress
    {
        public bool required { get; set; }

        public RegularOptionsField country { get; set; }
        public RegularOptionsField state { get; set; }
        public RegularField city { get; set; }
        public RegularField street { get; set; }
        public RegularField floor { get; set; }
        public RegularField number { get; set; }
        public RegularField department { get; set; }
        public RegularField postal_code { get; set; }
    }
}