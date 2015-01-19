using Despegar.Core.Business.Common.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingFields
{
    public class Document
    {
        public bool required { get; set; }
        public RegularOptionsField type { get; set; }
        public RegularField number { get; set; }
        public string data_type { get; set; }
    }
}
