using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Business.Hotels.BookingFields
{
    public class HotelsBookingFields
    {
        public string id { get; set; }
        public Items items { get; set; }
        public Form form { get; set; }
        public Fees fees { get; set; } 
    }
}