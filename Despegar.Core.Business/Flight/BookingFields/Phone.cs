using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingFields
{
    public class Phone
    {
        public bool required { get; set; }
        public FieldDataTypeOpt type { get; set; }
        public FieldDataTypeVal number { get; set; }
        public string data_type { get; set; }
        public int min_quantity { get; set; }
        public FieldDataTypeValValue country_code { get; set; }
        public FieldDataTypeValValue area_code { get; set; }
    }
}
