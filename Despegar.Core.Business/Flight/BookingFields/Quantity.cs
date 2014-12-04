using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingFields
{
    public class Quantity
    {
        // MAPI Fields
        public bool required { get; set; }
        public string data_type { get; set; }
        // Custom
        public string CoreValue { get; set; }
        public bool CorePostEnable { get; set; }
    }
}
