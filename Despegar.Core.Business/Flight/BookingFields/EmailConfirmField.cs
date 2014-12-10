using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingFields
{
    public class EmailConfirmField : RegularField
    {
        private RegularField emailField;

        public EmailConfirmField(RegularField value) : base()
        {
            this.emailField = value;
            this.required = true;
            value.PropertyChanged += PropertyHander;
        }

        private void PropertyHander(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CoreValue" && CoreValue != null)
                Validate();
        }

        public override void Validate()
        {
            Errors.Clear();
            CurrentError = null;

            if (required && String.IsNullOrWhiteSpace(CoreValue))
            {
                Errors.Add("REQUIRED");
                CurrentError = "REQUIRED";
                return;
            }

            // Confirm same email
            if (emailField.CoreValue != this.CoreValue) 
            {
                Errors.Add("INVALID_EMAIL_MATCH");
                CurrentError = "INVALID_EMAIL_MATCH";
                return;
            }
        }
    }
}