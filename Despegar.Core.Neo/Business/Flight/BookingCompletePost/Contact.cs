using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Flight.BookingCompletePost
{
    public class Contact
    {
        public string email { get; set; }
        public List<Phone> phones { get; set; }
        public string offers { get; set; }
        public string contact_full_name { get; set; }
    }
}
