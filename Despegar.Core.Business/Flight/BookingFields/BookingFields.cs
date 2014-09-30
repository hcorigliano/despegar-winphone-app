using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Business.Flight.BookingFields
{
    public class BookingFields
    {
        public string id { get; set; }
        public Price price { get; set; }
        public Payments payments { get; set; }
        public Form form { get; set; }
        public object terms_and_conditions { get; set; }
    }
}
