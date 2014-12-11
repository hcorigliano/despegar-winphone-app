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

        // Custom
        // NOTE FOR DEVS: Do NOT use && operator to include every condition in one IF. It won't trigger the validations of each field.
        public bool IsValid
        { 
         get 
         {
            bool isValid = true;

            // Passengers
            foreach (var passenger in form.passengers) 
            {
                //if (passenger.birthdate.IsValid)
                //    isValid = false;
                if (passenger.first_name != null && !passenger.first_name.IsValid)
                    isValid = false;
                if (passenger.last_name != null && !passenger.last_name.IsValid)
                    isValid = false;
                if (passenger.nationality != null && !passenger.nationality.IsValid)
                    isValid = false;
                if (passenger.gender != null && !passenger.gender.IsValid)
                    isValid = false;
                if (passenger.document != null)
                {
                    if (passenger.document.number != null && !passenger.document.number.IsValid)
                        isValid = false;
                    if (passenger.document.type != null && !passenger.document.type.IsValid)
                        isValid = false;
                }
            }

            // Contact
            if (form.contact.email != null && !form.contact.email.IsValid)                
                isValid = false;
            if (form.contact.emailConfirmation != null && !form.contact.emailConfirmation.IsValid)                
                isValid = false;

            foreach (var phone in form.contact.phones) 
            {
                if (phone.country_code != null && !phone.country_code.IsValid)
                    isValid = false;
                if (phone.area_code != null && !phone.area_code.IsValid)                
                    isValid = false;
                if (phone.number != null && !phone.number.IsValid)
                    isValid = false;
                if (phone.type != null && !phone.type.IsValid)
                    isValid = false;
            }

            // Card
            if (form.payment.card != null)
            {
                if (form.payment.card.security_code != null && !form.payment.card.security_code.IsValid)
                    isValid = false;
                if (form.payment.card.owner_type != null && !form.payment.card.owner_type.IsValid)
                    isValid = false;
                if (form.payment.card.owner_name != null && !form.payment.card.owner_name.IsValid)
                    isValid = false;
                if (form.payment.card.owner_gender != null && !form.payment.card.owner_gender.IsValid)
                    isValid = false;                
                if (form.payment.card.number != null && !form.payment.card.number.IsValid)
                    isValid = false;
                //if (form.payment.card.expiration.IsValid)
                //    isValid = false;
                if (form.payment.card.owner_document != null) 
                {
                    if (form.payment.card.owner_document.number != null && !form.payment.card.owner_document.number.IsValid)
                        isValid = false;
                    if (form.payment.card.owner_document.type != null && !form.payment.card.owner_document.type.IsValid)
                        isValid = false;
                }                                
            }

            // Installment   TODO: Validate selection
            //form.payment.installment.quantity;

             // Invoice Arg
            if (form.payment.invoice != null)
            {
                if (form.payment.invoice.address.city_id != null && !form.payment.invoice.address.city_id.IsValid)
                    isValid = false;
                if (form.payment.invoice.address.country != null && !form.payment.invoice.address.country.IsValid)
                    isValid = false;
                if (form.payment.invoice.address.department != null && !form.payment.invoice.address.department.IsValid)
                    isValid = false;
                if (form.payment.invoice.address.floor != null && !form.payment.invoice.address.floor.IsValid)
                    isValid = false;
                if (form.payment.invoice.address.number != null && !form.payment.invoice.address.number.IsValid)
                    isValid = false;
                if (form.payment.invoice.address.postal_code != null && !form.payment.invoice.address.postal_code.IsValid)
                    isValid = false;
                if (form.payment.invoice.address.state != null && !form.payment.invoice.address.state.IsValid)
                    isValid = false;
                if (form.payment.invoice.address.street != null && !form.payment.invoice.address.street.IsValid)
                    isValid = false;
                if (form.payment.invoice.fiscal_status != null && !form.payment.invoice.fiscal_status.IsValid)
                    isValid = false;
                if (form.payment.invoice.fiscal_id != null && !form.payment.invoice.fiscal_id.IsValid)
                    isValid = false;
                if (form.payment.invoice.fiscal_status != null && form.payment.invoice.fiscal_status.CoreValue != "FINAL" && !form.payment.invoice.fiscal_name.IsValid)
                    isValid = false; 
            }

            return isValid;
         }
      }
    }
}