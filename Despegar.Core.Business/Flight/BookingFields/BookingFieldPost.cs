using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Business.Flight.BookingFields
{
    public class BookingFieldPost
    {
        public string itinerary_id { get; set; }
        public int outbound_choice { get; set; }
        public int ? inbound_choice { get; set; }
        public string mobile_identifier { get; set; }
    }
}
