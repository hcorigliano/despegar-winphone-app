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
        private static JObject BuildFlightPassenger(Despegar.Core.Neo.Business.Flight.BookingFields.Passenger passenger)
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

        private static JObject BuildHotelPassenger(Despegar.Core.Neo.Business.Hotels.BookingFields.Passenger passenger)
        {
            JObject result = new JObject();
            result.Add("first_name", passenger.first_name.CoreValue);
            result.Add("last_name", passenger.last_name.CoreValue);
            //result.Add("room_reference", passenger.last_name.CoreValue); //Is necesary?

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
                foreach (var passenger in bookingFields.form.passengers.Select(p => BuildFlightPassenger(p)))
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

                // Billing Address
                if (bookingFields.form.payment.billing_address != null) 
                {
                    var billingAddress = new JObject();

                    if (bookingFields.form.payment.billing_address.country != null)
                        billingAddress.Add("country", bookingFields.form.payment.billing_address.country.CoreValue);
                    if (bookingFields.form.payment.billing_address.state != null)
                        billingAddress.Add("state", bookingFields.form.payment.billing_address.state.CoreValue);

                    //if (bookingFields.form.payment.billing_address.city_id != null)
                    //    billingAddress.Add("city_id", bookingFields.form.payment.billing_address.city_id.CoreValue);
                    if (bookingFields.form.payment.billing_address.city != null)
                        billingAddress.Add("city", bookingFields.form.payment.billing_address.city.CoreValue);
                    if (bookingFields.form.payment.billing_address.number != null)
                        billingAddress.Add("number", bookingFields.form.payment.billing_address.number.CoreValue);
                    if (bookingFields.form.payment.billing_address.floor != null)
                        billingAddress.Add("floor", bookingFields.form.payment.billing_address.floor.CoreValue);
                    if (bookingFields.form.payment.billing_address.department != null)
                        billingAddress.Add("department", bookingFields.form.payment.billing_address.department.CoreValue);
                    if (bookingFields.form.payment.billing_address.postal_code != null)
                        billingAddress.Add("postal_code", bookingFields.form.payment.billing_address.postal_code.CoreValue);
                    if (bookingFields.form.payment.billing_address.street != null)
                        billingAddress.Add("street", bookingFields.form.payment.billing_address.street.CoreValue);

                    payment.Add("billing_address", billingAddress);
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

        public static Task<object> BuildHotelsForm(HotelsBookingFields bookingFields, HotelPayment selectedCard, bool validateDuplicateCheckouts)
        {
            InvoiceArg invoiceFields = null; 
            if(bookingFields.CheckoutMethodSelected != null && bookingFields.CheckoutMethodSelected.payment != null)
                invoiceFields = bookingFields.CheckoutMethodSelected.payment.invoice;
            
            return Task.Run(() =>
            {
                JObject result = new JObject();
                JObject form = new JObject();

                JArray passengers = new JArray();
                JObject contact = new JObject();
                JObject payment = new JObject();

                // Passengers
                foreach (var passenger in bookingFields.form.passengers.Select(p => BuildHotelPassenger(p)))
                    passengers.Add(passenger);

                // Contact
                JArray phones = new JArray();
                foreach (var phone in bookingFields.form.contact.phones.Select(p => BuildPhones(p)))
                    phones.Add(phone);

                contact.Add("phones", phones);
                contact.Add("email", bookingFields.form.contact.email.CoreValue);

                // Payment
                if (bookingFields.form.CardInfo != null)
                {
                    JObject card = new JObject();
                    if (bookingFields.form.CardInfo.security_code != null)
                        card.Add("security_code", bookingFields.form.CardInfo.security_code.CoreValue);
                    if (bookingFields.form.CardInfo.expiration != null)
                        card.Add("expiration", bookingFields.form.CardInfo.expiration.CoreValue);
                    if (bookingFields.form.CardInfo.number != null)
                        card.Add("number", bookingFields.form.CardInfo.number.CoreValue);
                    if (bookingFields.form.CardInfo.owner_name != null)
                        card.Add("owner_name", bookingFields.form.CardInfo.owner_name.CoreValue);

                    if (bookingFields.form.CardInfo.owner_gender != null)
                        card.Add("owner_gender", bookingFields.form.CardInfo.owner_gender.CoreValue);

                    if (bookingFields.form.CardInfo.owner_document != null)
                    {
                        JObject owner_document = new JObject();

                        if (bookingFields.form.CardInfo.owner_document.type != null)
                            owner_document.Add("type", bookingFields.form.CardInfo.owner_document.type.CoreValue);
                        if (bookingFields.form.CardInfo.owner_document.number != null)
                            owner_document.Add("number", bookingFields.form.CardInfo.owner_document.number.CoreValue);

                        card.Add("owner_document", owner_document);
                    }

                    payment.Add("card", card);
                }

                // Installment
                if (bookingFields.form.CurrentInstallment != null)
                {
                    JObject installment = new JObject();
                    if (bookingFields.form.CurrentInstallment.card_type != null)
                        installment.Add("card_type", bookingFields.form.CurrentInstallment.card_type.CoreValue);
                    if (bookingFields.form.CurrentInstallment.card_code != null)
                        installment.Add("card_code", bookingFields.form.CurrentInstallment.card_code.CoreValue);

                    if (Convert.ToInt32(bookingFields.form.CurrentInstallment.quantity.CoreValue) != 0)
                        installment.Add("quantity", Convert.ToInt32(bookingFields.form.CurrentInstallment.quantity.CoreValue));

                    if (bookingFields.form.CurrentInstallment.complete_card_code != null && bookingFields.form.CurrentInstallment.complete_card_code.CoreValue != null)
                        installment.Add("complete_card_code", bookingFields.form.CurrentInstallment.complete_card_code.CoreValue);

                    payment.Add("installment", installment);
                }

                //invoiceFields 
                
                // Invoice Arg
                if (bookingFields.form.CountrySite != null & bookingFields.form.CountrySite.ToLower().Contains("ar") && invoiceFields != null) // Is only for Arg in mapi
                {
                    JObject invoice = new JObject();
                    JObject address = new JObject();

                    if (invoiceFields.fiscal_id != null)
                        invoice.Add("fiscal_id", invoiceFields.fiscal_id.CoreValue);

                    if (invoiceFields.fiscal_status != null)
                    {
                        invoice.Add("fiscal_status", invoiceFields.fiscal_status.CoreValue);

                        if (invoiceFields.fiscal_status.CoreValue != "FINAL")
                            invoice.Add("fiscal_name", invoiceFields.fiscal_name.CoreValue);
                    }

                    if (invoiceFields.address.number != null)
                        address.Add("number", invoiceFields.address.number.CoreValue);
                    if (invoiceFields.address.floor != null)
                        address.Add("floor", invoiceFields.address.floor.CoreValue);
                    if (invoiceFields.address.department != null)
                        address.Add("department", invoiceFields.address.department.CoreValue);
                    if (invoiceFields.address.city_id != null)
                        address.Add("city_id", invoiceFields.address.city_id.CoreValue);
                    if (invoiceFields.address.city != null)
                        address.Add("city", invoiceFields.address.city.CoreValue);

                    if (invoiceFields.address.state != null)
                        address.Add("state", invoiceFields.address.state.CoreValue);

                    if (invoiceFields.address.country != null)
                        address.Add("country", invoiceFields.address.country.CoreValue); //this is fill with the response of service

                    if (invoiceFields.address.postal_code != null)
                        address.Add("postal_code", invoiceFields.address.postal_code.CoreValue);
                    if (invoiceFields.address.street != null)
                        address.Add("street", invoiceFields.address.street.CoreValue);

                    invoice.Add("address", address);
                    payment.Add("invoice", invoice);
                }

                // Voucher
                if (bookingFields.form.Voucher != null)
                {
                    JArray vouchers = new JArray();

                    //foreach (var voucher in bookingFields.form.Voucher.CoreValue Select(x => x.CoreValue))
                        vouchers.Add(bookingFields.form.Voucher.CoreValue);

                    form.Add("vouchers", vouchers);
                }

                form.Add("passengers", passengers);
                form.Add("contact", contact);
                form.Add("payment", payment);

                result.Add("form", form);
                result.Add("payment_method", selectedCard.id);
                result.Add("validate_duplicated_checkouts", validateDuplicateCheckouts);

                return result as object;

            });
        }
      
    }
}