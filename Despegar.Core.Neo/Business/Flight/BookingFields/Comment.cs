using Despegar.Core.Neo.Business.Common.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Flight.BookingFields
{
    public class Comment : RegularField
    {
        public RegularField reference_code { get; set; }
        public RegularField follow_reservation { get; set; }
    }
}
