using Despegar.Core.Neo.Business.Common.Checkout;
using Despegar.Core.Neo.Business.Flight.BookingFields;
using Despegar.Core.Neo.Business.Hotels.BookingFields;
using Newtonsoft.Json.Linq;
using System;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;


namespace Despegar.Core.Neo.Business.Forms
{
    public class BookingFormBuilder
    {
        #region UTILS
        private static JObject BuildPassenger(Despegar.Core.Neo.Business.Flight.BookingFields.Passenger passenger)
        {
            JObject result = new JObject();
            result.Add("type", passenger.type);
            result.Add("first_name", passenger.first_name.CoreValue);
            result.Add("last_name", passenger.last_name.CoreValue);


            if (passenger.document != null)
            {
                JObject document = new JObject();
                if (passenger.document.type != null)
                    document.Add("type", passenger.document.type.CoreValue);
                if (passenger.document.number != null)
                    document.Add("number", passenger.document.number.CoreValue);

                result.Add("document", document);
            }

            if (passenger.nationality != null)
                result.Add("nationality", passenger.nationality.CoreValue);
            if (passenger.birthdate != null)
                result.Add("birthdate", passenger.birthdate.CoreValue);
            if (passenger.gender != null)
                result.Add("gender", passenger.gender.CoreValue);

            return result;
        }

        private static JObject BuildPhones(Phone phone)
        {
            JObject result = new JObject();
            result.Add("type", phone.type.CoreValue);
            result.Add("area_code", phone.area_code.CoreValue);
            result.Add("number", phone.number.CoreValue);
            result.Add("country_code", phone.country_code.CoreValue);

            return result;
        }      
        #endregion

        public static Task<object> BuildFlightsForm(FlightBookingFields bookingFields)
        {
            return Task.Run(() =>
            {
                JObject result = new JObject();
                JObject form = new JObject();

                JArray passengers = new JArray();
                JObject contact = new JObject();
                JObject payment = new JObject();

                // Passengers
                foreach (var passenger in bookingFields.form.passengers.Select(p => BuildPassenger(p)))
                   passengers.Add(passenger);

                // Contact
                JArray phones = new JArray();
                foreach(var phone in bookingFields.form.contact.phones.Select(p => BuildPhones(p)))
                    phones.Add(phone);

                contact.Add("phones", phones);
                contact.Add("email",  bookingFields.form.contact.email.CoreValue);

                // Payment
                JObject card = new JObject();
                card.Add("security_code", bookingFields.form.payment.card.security_code.CoreValue);
                card.Add("expiration", bookingFields.form.payment.card.expiration.CoreValue);
                card.Add("number", bookingFields.form.payment.card.number.CoreValue);
                card.Add("owner_name", bookingFields.form.payment.card.owner_name.CoreValue);

                if (bookingFields.form.payment.card.owner_gender != null)
                    card.Add("owner_gender", bookingFields.form.payment.card.owner_gender.CoreValue);                        

                if (bookingFields.form.payment.card.owner_document != null)
                {
                    JObject owner_document = new JObject();

                    if (bookingFields.form.payment.card.owner_document.type != null)
                        owner_document.Add("type", bookingFields.form.payment.card.owner_document.type.CoreValue);
                    if (bookingFields.form.payment.card.owner_document.number != null)
                        owner_document.Add("number", bookingFields.form.payment.card.owner_document.number.CoreValue);

                    card.Add("owner_document", owner_document);                     
                }

                payment.Add("card", card);

                // Installment
                JObject installment = new JObject();
                installment.Add("card_type", bookingFields.form.payment.installment.card_type.CoreValue);
                installment.Add("card_code", bookingFields.form.payment.installment.card_code.CoreValue);
                installment.Add("quantity", Convert.ToInt32(bookingFields.form.payment.installment.quantity.CoreValue));

                if (bookingFields.form.payment.installment.complete_card_code.CoreValue != null)
                    installment.Add("complete_card_code", bookingFields.form.payment.installment.complete_card_code.CoreValue);
                
                payment.Add("installment", installment);

                // Invoice Arg
                if (bookingFields.form.passengers[0].nationality != null && bookingFields.form.passengers[0].nationality.value == "AR") // Is only for Arg in mapi
                {
                    JObject invoice = new JObject();
                    JObject address = new JObject();

                    if (bookingFields.form.payment.invoice.fiscal_id != null)
                        invoice.Add("fiscal_id", bookingFields.form.payment.invoice.fiscal_id.CoreValue);

                    if (bookingFields.form.payment.invoice.fiscal_status != null)
                    {
                        invoice.Add("fiscal_status", bookingFields.form.payment.invoice.fiscal_status.CoreValue);

                        if (bookingFields.form.payment.invoice.fiscal_status.CoreValue != "FINAL")                        
                           invoice.Add("fiscal_name", bookingFields.form.payment.invoice.fiscal_name.CoreValue);                        
                    }

                    if (bookingFields.form.payment.invoice.address.number != null)
                        address.Add("number", bookingFields.form.payment.invoice.address.number.CoreValue);
                    if (bookingFields.form.payment.invoice.address.floor != null)
                        address.Add("floor", bookingFields.form.payment.invoice.address.floor.CoreValue);
                    if (bookingFields.form.payment.invoice.address.department != null)
                        address.Add("department", bookingFields.form.payment.invoice.address.department.CoreValue);
                    if (bookingFields.form.payment.invoice.address.city_id != null)
                        address.Add("city_id", bookingFields.form.payment.invoice.address.city_id.CoreValue);
                    if (bookingFields.form.payment.invoice.address.city != null)
                        address.Add("city", bookingFields.form.payment.invoice.address.city.CoreValue);

                    address.Add("state", bookingFields.form.payment.invoice.address.state.CoreValue);
                    address.Add("country", bookingFields.form.payment.invoice.address.country.CoreValue); //this is fill with the response of service

                    if (bookingFields.form.payment.invoice.address.postal_code != null)
                        address.Add("postal_code", bookingFields.form.payment.invoice.address.postal_code.CoreValue);
                    if (bookingFields.form.payment.invoice.address.street != null)
                        address.Add("street", bookingFields.form.payment.invoice.address.street.CoreValue);
                    
                    invoice.Add("address", address);
                    payment.Add("invoice", invoice);
                }

                // Voucher
                if (bookingFields.form.Voucher != null)
                {
                    JArray vouchers = new JArray();

                    foreach(var voucher in bookingFields.form.vouchers.Select(x => x.CoreValue))
                        vouchers.Add(voucher);

                    form.Add("vouchers", vouchers);                    
                }

                form.Add("booking_status", bookingFields.form.booking_status);

                form.Add("passengers", passengers);
                form.Add("contact", contact);
                form.Add("payment", payment);

                result.Add("form", form);

                return result as object;

            });
        }

        //public static Task<object> BuildHotelsForm(HotelsBookingFields bookingFields)
        //{
        //    return Task.Run(() =>
        //    {
        //        JObject result = new JObject();
        //        JObject form = new JObject();

        //        JArray passengers = new JArray();
        //        JObject contact = new JObject();
        //        JObject payment = new JObject();

        //        // Passengers
        //        foreach (var passenger in bookingFields.form.passengers.Select(p => BuildPassenger(p)))
        //            passengers.Add(passenger);

        //        // Contact
        //        JArray phones = new JArray();
        //        foreach (var phone in bookingFields.form.contact.phones.Select(p => BuildPhones(p)))
        //            phones.Add(phone);

        //        contact.Add("phones", phones);
        //        contact.Add("email", bookingFields.form.contact.email.CoreValue);

        //        // Payment
        //        JObject card = new JObject();
        //        card.Add("security_code", bookingFields.form.payment.card.security_code.CoreValue);
        //        card.Add("expiration", bookingFields.form.payment.card.expiration.CoreValue);
        //        card.Add("number", bookingFields.form.payment.card.number.CoreValue);
        //        card.Add("owner_name", bookingFields.form.payment.card.owner_name.CoreValue);

        //        if (bookingFields.form.payment.card.owner_gender != null)
        //            card.Add("owner_gender", bookingFields.form.payment.card.owner_gender.CoreValue);

        //        if (bookingFields.form.payment.card.owner_document != null)
        //        {
        //            JObject owner_document = new JObject();

        //            if (bookingFields.form.payment.card.owner_document.type != null)
        //                owner_document.Add("type", bookingFields.form.payment.card.owner_document.type.CoreValue);
        //            if (bookingFields.form.payment.card.owner_document.number != null)
        //                owner_document.Add("number", bookingFields.form.payment.card.owner_document.number.CoreValue);

        //            card.Add("owner_document", owner_document);
        //        }

        //        payment.Add("card", card);

        //        // Installment
        //        JObject installment = new JObject();
        //        installment.Add("card_type", bookingFields.form.payment.installment.card_type.CoreValue);
        //        installment.Add("card_code", bookingFields.form.payment.installment.card_code.CoreValue);
        //        installment.Add("quantity", Convert.ToInt32(bookingFields.form.payment.installment.quantity.CoreValue));

        //        if (bookingFields.form.payment.installment.complete_card_code.CoreValue != null)
        //            installment.Add("complete_card_code", bookingFields.form.payment.installment.complete_card_code.CoreValue);

        //        payment.Add("installment", installment);

        //        // Invoice Arg
        //        if (bookingFields.form.passengers[0].nationality != null && bookingFields.form.passengers[0].nationality.value == "AR") // Is only for Arg in mapi
        //        {
        //            JObject invoice = new JObject();
        //            JObject address = new JObject();

        //            if (bookingFields.form.payment.invoice.fiscal_id != null)
        //                invoice.Add("fiscal_id", bookingFields.form.payment.invoice.fiscal_id.CoreValue);

        //            if (bookingFields.form.payment.invoice.fiscal_status != null)
        //            {
        //                invoice.Add("fiscal_status", bookingFields.form.payment.invoice.fiscal_status.CoreValue);

        //                if (bookingFields.form.payment.invoice.fiscal_status.CoreValue != "FINAL")
        //                    invoice.Add("fiscal_name", bookingFields.form.payment.invoice.fiscal_name.CoreValue);
        //            }

        //            if (bookingFields.form.payment.invoice.address.number != null)
        //                address.Add("number", bookingFields.form.payment.invoice.address.number.CoreValue);
        //            if (bookingFields.form.payment.invoice.address.floor != null)
        //                address.Add("floor", bookingFields.form.payment.invoice.address.floor.CoreValue);
        //            if (bookingFields.form.payment.invoice.address.department != null)
        //                address.Add("department", bookingFields.form.payment.invoice.address.department.CoreValue);
        //            if (bookingFields.form.payment.invoice.address.city_id != null)
        //                address.Add("city_id", bookingFields.form.payment.invoice.address.city_id.CoreValue);
        //            if (bookingFields.form.payment.invoice.address.city != null)
        //                address.Add("city", bookingFields.form.payment.invoice.address.city.CoreValue);

        //            address.Add("state", bookingFields.form.payment.invoice.address.state.CoreValue);
        //            address.Add("country", bookingFields.form.payment.invoice.address.country.CoreValue); //this is fill with the response of service

        //            if (bookingFields.form.payment.invoice.address.postal_code != null)
        //                address.Add("postal_code", bookingFields.form.payment.invoice.address.postal_code.CoreValue);
        //            if (bookingFields.form.payment.invoice.address.street != null)
        //                address.Add("street", bookingFields.form.payment.invoice.address.street.CoreValue);

        //            invoice.Add("address", address);
        //            payment.Add("invoice", invoice);
        //        }

        //        // Voucher
        //        if (bookingFields.form.Voucher != null)
        //        {
        //            JArray vouchers = new JArray();

        //            foreach (var voucher in bookingFields.form.vouchers.Select(x => x.CoreValue))
        //                vouchers.Add(voucher);

        //            form.Add("vouchers", vouchers);
        //        }

        //        form.Add("passengers", passengers);
        //        form.Add("contact", contact);
        //        form.Add("payment", payment);

        //        result.Add("form", form);

        //        return result as object;

        //    });
        //}
    }
}