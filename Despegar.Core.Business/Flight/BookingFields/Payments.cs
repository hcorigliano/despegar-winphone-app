using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingFields
{
    public class Payments
    {
        public object pay_at_destination { get; set; }
        public List<WithInterest> with_interest { get; set; }
        public List<WithoutInterest> without_interest { get; set; }
    }
}
