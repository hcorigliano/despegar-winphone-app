using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Business.Flight.BookingFields
{
    public class FlightBookingFields
    {
        public string id { get; set; }
        public Price price { get; set; }
        public Payments payments { get; set; }
        public Form form { get; set; }

        // NOTE FOR DEVS: Do NOT use && operator to include every condition in one IF. It won't trigger the validations of each field.      
        public bool IsValid(out string sectionID)
        {
            bool passengerValid = true;
            bool contactValid = true;
            bool cardValid = true;
            bool invoiceValid = true;
            bool installmentValid = true;
            bool voucherValid = true;

            sectionID = String.Empty;

            // Passengers           
            foreach (var passenger in form.passengers) 
            {
                if (passenger.birthdate != null && !passenger.birthdate.IsValid)
                    passengerValid = false;
                if (passenger.first_name != null && !passenger.first_name.IsValid)
                    passengerValid = false;
                if (passenger.last_name != null && !passenger.last_name.IsValid)
                    passengerValid = false;
                if (passenger.nationality != null && !passenger.nationality.IsValid)
                    passengerValid = false;
                if (passenger.gender != null && !passenger.gender.IsValid)
                    passengerValid = false;
                if (passenger.document != null)
                {
                    if (passenger.document.number != null && !passenger.document.number.IsValid)
                        passengerValid = false;
                    if (passenger.document.type != null && !passenger.document.type.IsValid)
                        passengerValid = false;
                }
            }
           
            // Contact
            if (form.contact.email != null && !form.contact.email.IsValid)
                contactValid = false;
            if (form.contact.emailConfirmation != null && !form.contact.emailConfirmation.IsValid)
                contactValid = false;

            foreach (var phone in form.contact.phones) 
            {
                if (phone.country_code != null && !phone.country_code.IsValid)
                    contactValid = false;
                if (phone.area_code != null && !phone.area_code.IsValid)
                    contactValid = false;
                if (phone.number != null && !phone.number.IsValid)
                    contactValid = false;
                if (phone.type != null && !phone.type.IsValid)
                    contactValid = false;
            }

            // Card
            if (form.payment.card != null)
            {
                if (form.payment.card.security_code != null && !form.payment.card.security_code.IsValid)
                    cardValid = false;
                if (form.payment.card.owner_type != null && !form.payment.card.owner_type.IsValid)
                    cardValid = false;
                if (form.payment.card.owner_name != null && !form.payment.card.owner_name.IsValid)
                    cardValid = false;
                if (form.payment.card.owner_gender != null && !form.payment.card.owner_gender.IsValid)
                    cardValid = false;                
                if (form.payment.card.number != null && !form.payment.card.number.IsValid)
                    cardValid = false;
                if (form.payment.card.expiration != null && !form.payment.card.expiration.IsValid)
                    cardValid = false;
                if (form.payment.card.owner_document != null) 
                {
                    if (form.payment.card.owner_document.number != null && !form.payment.card.owner_document.number.IsValid)
                        cardValid = false;
                    if (form.payment.card.owner_document.type != null && !form.payment.card.owner_document.type.IsValid)
                        cardValid = false;
                }                                
            }

            // Installment
            if (form.payment.installment != null)
            {
                if (form.payment.installment.card_code != null && !form.payment.installment.card_code.IsValid)
                    installmentValid = false;
                if (form.payment.installment.card_type != null && !form.payment.installment.card_type.IsValid)
                    installmentValid = false;
                if (form.payment.installment.complete_card_code != null && !form.payment.installment.complete_card_code.IsValid)
                    installmentValid = false;
                //if (form.payment.installment.quantity != null && !form.payment.installment.quantity.IsValid)
                //    installmentValid = false;
            }

             // Invoice Arg
            if (form.passengers[0].nationality != null && form.passengers[0].nationality.value == "AR" && form.payment.invoice != null)
            {
                if (form.payment.invoice.address.city_id != null && !form.payment.invoice.address.city_id.IsValid)
                    invoiceValid = false;
                if (form.payment.invoice.address.country != null && !form.payment.invoice.address.country.IsValid)
                    invoiceValid = false;
                if (form.payment.invoice.address.department != null && !form.payment.invoice.address.department.IsValid)
                    invoiceValid = false;
                if (form.payment.invoice.address.floor != null && !form.payment.invoice.address.floor.IsValid)
                    invoiceValid = false;
                if (form.payment.invoice.address.number != null && !form.payment.invoice.address.number.IsValid)
                    invoiceValid = false;
                if (form.payment.invoice.address.postal_code != null && !form.payment.invoice.address.postal_code.IsValid)
                    invoiceValid = false;
                if (form.payment.invoice.address.state != null && !form.payment.invoice.address.state.IsValid)
                    invoiceValid = false;
                if (form.payment.invoice.address.street != null && !form.payment.invoice.address.street.IsValid)
                    invoiceValid = false;
                if (form.payment.invoice.fiscal_status != null && !form.payment.invoice.fiscal_status.IsValid)
                    invoiceValid = false;
                if (form.payment.invoice.fiscal_id != null && !form.payment.invoice.fiscal_id.IsValid)
                    invoiceValid = false;
                if (form.payment.invoice.fiscal_status != null && form.payment.invoice.fiscal_status.CoreValue != "FINAL" && !form.payment.invoice.fiscal_name.IsValid)
                    invoiceValid = false; 
            }

            // Voucher
            if (!form.Voucher.IsValid)
                voucherValid = false;

            if (!passengerValid)
            {
                sectionID = "PASSENGERS";
                return false;
            }
            if (!contactValid)
            {
                sectionID = "CONTACT";
                return false;
            }
            if (!cardValid || !voucherValid)
            {
                sectionID = "CARD";
                return false;
            }
            if (!invoiceValid) 
            {
                sectionID = "INVOICE";
                return false;
            }
            if (!installmentValid)
            {
                sectionID = "INSTALLMENT";
                return false;
            }

            return true;
         }
    }
}