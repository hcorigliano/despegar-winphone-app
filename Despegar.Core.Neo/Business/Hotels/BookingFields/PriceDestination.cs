using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Hotels.BookingFields
{
    public class PriceDestination
    {
        public Currency currency { get; set; }
        public decimal total { get; set; }
        public string message { get; set; }
    }
}