using Despegar.Core.Neo.Business.Common.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Hotels.BookingFields
{
    public class Contact
    {
        public bool required { get; set; }
        private RegularField emailField;
        public RegularField email { get { return emailField; } set { emailField = value; emailConfirmation = new EmailConfirmField(value); } }
        public List<Phone> phones { get; set; }

        // Custom
        public RegularField emailConfirmation { get; set; }
        public Phone Phone { get { return phones.FirstOrDefault(); } }

        public bool IsFrozen { get; set; }
    }
}