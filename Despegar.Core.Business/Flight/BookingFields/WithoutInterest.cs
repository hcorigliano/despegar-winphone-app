﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingFields
{
    public class WithoutInterest
    {
        public string id { get; set; }
        public Card card { get; set; }
        public Installments installments { get; set; }
        public double interest { get; set; }
        public double financial_cost { get; set; }
    }
}
