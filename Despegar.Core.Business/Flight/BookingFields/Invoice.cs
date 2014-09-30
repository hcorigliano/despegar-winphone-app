using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingFields
{
    public class Invoice
    {
        public bool required { get; set; }
        public Address address { get; set; }
        public string data_type { get; set; }
        public FieldDataType fiscal_name { get; set; }
        public FieldDataTypeOptValue fiscal_status { get; set; }
        public FieldDataTypeVal fiscal_id { get; set; }
    }
}
