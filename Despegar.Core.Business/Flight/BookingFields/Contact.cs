using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingFields
{
    public class Contact
    {
        // MAPI Fields
        public string data_type { get; set; }
        public bool required { get; set; }
        public RegularField email { get; set; }
        public RegularField offers { get; set; }
        public List<Phone> phones { get; set; }

        // Custom
        public Phone Phone { get { return phones.FirstOrDefault(); } }
    }
}