using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Business.Flight.BookingFields
{
    public class FieldDataType
    {
        public bool required { get; set; }
        public string data_type { get; set; }
        public string coreValue { get; set; }
        public bool corePostEnable { get; set; }
        public string value { get; set; }
    }
}
