using Despegar.Core.Neo.Business.Common.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Hotels.BookingFields
{
    public class AdditionalData
    {
        public bool required { get; set; }
        public RegularField comment { get; set; }
        public RegularField subscribe_newsletter { get; set; }
    }
}
