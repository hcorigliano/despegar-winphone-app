using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingFields
{
    public class Address
    {
        public bool required { get; set; }
        public FieldDataTypeValValue country { get; set; }
        public FieldDataTypeValValue state { get; set; }
        public FieldDataTypeVal street { get; set; }
        public FieldDataTypeVal number { get; set; }
        public FieldDataTypeVal floor { get; set; }
        public FieldDataTypeVal department { get; set; }
        public string data_type { get; set; }
        public FieldDataTypeVal city_id { get; set; }
        public FieldDataTypeVal postal_code { get; set; }
    }
}
