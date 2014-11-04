using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingFields
{
    public class Contact
    {
        public bool required { get; set; }
        public FieldDataTypeVal email { get; set; }
        public List<Phone> phones { get; set; }
        public FieldDataType offers { get; set; }
        public string data_type { get; set; }
    }
}
