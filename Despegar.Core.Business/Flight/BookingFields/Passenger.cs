using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingFields
{
    public class Passenger
    {
        public bool required { get; set; }
        public string type { get; set; }
        public Document document { get; set; }
        public FieldDataTypeOpt gender { get; set; }
        public FieldDataTypeVal nationality { get; set; }
        public Birthdate birthdate { get; set; }
        public string data_type { get; set; }
        public FieldDataTypeVal first_name { get; set; }
        public FieldDataTypeVal last_name { get; set; }
        public int full_name_max_length { get; set; }
    }
}
