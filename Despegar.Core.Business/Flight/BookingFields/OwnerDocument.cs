using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingFields
{
    public class OwnerDocument
    {
        public bool required { get; set; }
        public FieldDataTypeOpt type { get; set; }
        public FieldDataType number { get; set; }
        public string data_type { get; set; }
    }
}
