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
                if (passenger.first_name != null && !passenger.first_name.IsValid)
                    passengerValid = false;
                if (passenger.last_name != null && !passenger.last_name.IsValid)
                    passengerValid = false;               
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
            if (form.CardInfo != null)
            {
                if (form.CardInfo.security_code != null && !form.CardInfo.security_code.IsValid)
                    cardValid = false;
                if (form.CardInfo.owner_type != null && !form.CardInfo.owner_type.IsValid)
                    cardValid = false;
                if (form.CardInfo.owner_name != null && !form.CardInfo.owner_name.IsValid)
                    cardValid = false;
                if (form.CardInfo.owner_gender != null && !form.CardInfo.owner_gender.IsValid)
                    cardValid = false;
                if (form.CardInfo.number != null && !form.CardInfo.number.IsValid)
                    cardValid = false;
                if (form.CardInfo.expiration != null && !form.CardInfo.expiration.IsValid)
                    cardValid = false;                
                if (form.CardInfo.owner_document != null)
                {
                    if (form.CardInfo.owner_document.number != null && !form.CardInfo.owner_document.number.IsValid)
                        cardValid = false;
                    if (form.CardInfo.owner_document.type != null && !form.CardInfo.owner_document.type.IsValid)
                        cardValid = false;
                }
            }

            // Installment
            if (form.CurrentInstallment != null)
            {
                if (form.CurrentInstallment.card_code != null && !form.CurrentInstallment.card_code.IsValid)
                    installmentValid = false;
                if (form.CurrentInstallment.card_type != null && !form.CurrentInstallment.card_type.IsValid)
                    installmentValid = false;
                if (form.CurrentInstallment.complete_card_code != null && !form.CurrentInstallment.complete_card_code.IsValid)
                    installmentValid = false;
                //if (form.CurrentInstallment.quantity != null && !form.CurrentInstallment.quantity.IsValid)
                //    installmentValid = false;
            }

            // Invoice Arg
            var invo = form.Invoice;
            if (form.CountrySite.ToUpper() == "AR" && invo != null)
            {
                if (invo.address.city_id != null && !invo.address.city_id.IsValid)
                    invoiceValid = false;
                if (invo.address.country != null && !invo.address.country.IsValid)
                    invoiceValid = false;
                if (invo.address.department != null && !invo.address.department.IsValid)
                    invoiceValid = false;
                if (invo.address.floor != null && !invo.address.floor.IsValid)
                    invoiceValid = false;
                if (invo.address.number != null && !invo.address.number.IsValid)
                    invoiceValid = false;
                if (invo.address.postal_code != null && !invo.address.postal_code.IsValid)
                    invoiceValid = false;
                if (invo.address.state != null && !invo.address.state.IsValid)
                    invoiceValid = false;
                if (invo.address.street != null && !invo.address.street.IsValid)
                    invoiceValid = false;
                if (invo.fiscal_status != null && !invo.fiscal_status.IsValid)
                    invoiceValid = false;
                if (invo.fiscal_id != null && !invo.fiscal_id.IsValid)
                    invoiceValid = false;
                if (invo.fiscal_status != null && invo.fiscal_status.CoreValue != "FINAL" && !invo.fiscal_name.IsValid)
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