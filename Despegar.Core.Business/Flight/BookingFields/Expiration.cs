using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingFields
{
    public class Expiration
    {
        public bool required { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string data_type { get; set; }
        public string coreValue { get; set; }
        public bool corePostEnable { get; set; }
    }
}
