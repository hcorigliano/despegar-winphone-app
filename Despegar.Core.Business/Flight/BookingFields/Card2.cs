using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingFields
{
    public class Card2
    {
        public bool required { get; set; }
        public FieldDataType number { get; set; }
        public Expiration expiration { get; set; }
        public string data_type { get; set; }
        public FieldDataType security_code { get; set; }
        public FieldDataTypeOpt owner_type { get; set; }
        public FieldDataTypeVal owner_name { get; set; }
        public OwnerDocument owner_document { get; set; }
        public FieldDataTypeOpt owner_gender { get; set; }
    }
}
