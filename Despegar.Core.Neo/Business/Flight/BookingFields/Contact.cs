using Despegar.Core.Neo.Business.Common.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Flight.BookingFields
{
    public class Contact
    {
        // MAPI Fields
        public string data_type { get; set; }
        public bool required { get; set; }
        private RegularField emailField;
        public RegularField email { get { return emailField; } set { emailField = value; emailConfirmation = new EmailConfirmField(value); } }
        public RegularField offers { get; set; }
        public List<Phone> phones { get; set; }

        // Custom
        public RegularField emailConfirmation { get; set; }
        public Phone Phone { get { return phones.FirstOrDefault(); } }     
    }
}