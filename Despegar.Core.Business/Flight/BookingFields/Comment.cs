using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingFields
{
    public class Comment : FieldDataType
    {
        public FieldDataType reference_code { get; set; }
        public FieldDataType follow_reservation { get; set; }
    }
}
