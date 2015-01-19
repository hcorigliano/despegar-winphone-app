using Despegar.Core.Business.Flight.BookingFields;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Despegar.WP.UI.Model;


namespace Despegar.Core.Business.Dynamics
{
    public class DynamicFlightBookingFieldsToPost
    {
        private static dynamic BuildPassenger(Passenger passenger)
        {
            dynamic result = new ExpandoObject();
            result.type = passenger.type;
            if (passenger.document != null)
            {
                result.document = new ExpandoObject();
                if (passenger.document.type != null)
                    result.document.type = passenger.document.type.CoreValue;
                if (passenger.document.number != null)
                    result.document.number = passenger.document.number.CoreValue;
            }
            result.first_name = passenger.first_name.CoreValue;
            result.last_name = passenger.last_name.CoreValue;
            if (passenger.nationality != null)
                result.nationality = passenger.nationality.CoreValue;
            if (passenger.birthdate != null)
                result.birthdate = passenger.birthdate.CoreValue;
            if (passenger.gender != null)
                result.gender = passenger.gender.CoreValue;

            return result;
        }

        public static dynamic BuildPhones(Phone phone)
        {
            dynamic result = new ExpandoObject();
            result.type = phone.type.CoreValue;
            result.area_code = phone.area_code.CoreValue;
            result.number = phone.number.CoreValue;
            result.country_code = phone.country_code.CoreValue;

            return result;
        }

        public static Task<dynamic> ToDynamic(BookingFields bookingFields)
        {
            return Task.Run(() =>
             {
                 dynamic result = new ExpandoObject();
                 result.form = new ExpandoObject();

                 result.form.booking_status = bookingFields.form.booking_status;

                 result.form.passengers = bookingFields.form.passengers.Select(p => BuildPassenger(p)).ToList();

                 result.form.contact = new ExpandoObject();
                 result.form.contact.phones = bookingFields.form.contact.phones.Select(p => BuildPhones(p)).ToList();
                 result.form.contact.email = bookingFields.form.contact.email.CoreValue;

                 result.form.payment = new ExpandoObject();
                 result.form.payment.card = new ExpandoObject();
                 result.form.payment.card.security_code = bookingFields.form.payment.card.security_code.CoreValue;
                 if (bookingFields.form.payment.card.owner_gender != null)
                     result.form.payment.card.owner_gender = bookingFields.form.payment.card.owner_gender.CoreValue;
                 result.form.payment.card.expiration = bookingFields.form.payment.card.expiration.CoreValue;
                 result.form.payment.card.number = bookingFields.form.payment.card.number.CoreValue;
                 result.form.payment.card.owner_name = bookingFields.form.payment.card.owner_name.CoreValue;
                 if (bookingFields.form.payment.card.owner_document != null)
                 {
                     result.form.payment.card.owner_document = new ExpandoObject();
                     if (bookingFields.form.payment.card.owner_document.type != null)
                         result.form.payment.card.owner_document.type = bookingFields.form.payment.card.owner_document.type.CoreValue;
                     if (bookingFields.form.payment.card.owner_document.number != null)
                         result.form.payment.card.owner_document.number = bookingFields.form.payment.card.owner_document.number.CoreValue;
                 }
                 result.form.payment.installment = new ExpandoObject();
                 result.form.payment.installment.card_type = bookingFields.form.payment.installment.card_type.CoreValue;
                 result.form.payment.installment.card_code = bookingFields.form.payment.installment.card_code.CoreValue;
                 if (bookingFields.form.payment.installment.complete_card_code.CoreValue != null)
                     result.form.payment.installment.complete_card_code = bookingFields.form.payment.installment.complete_card_code.CoreValue;
                 result.form.payment.installment.quantity = Convert.ToInt32(bookingFields.form.payment.installment.quantity.CoreValue);

                 //if (bookingFields.form.payment.invoice != null)
                 if (bookingFields.form.passengers[0].nationality != null && bookingFields.form.passengers[0].nationality.value == "AR") //Is only for Arg in mapi
                 {
                     result.form.payment.invoice = new ExpandoObject();

                     if (bookingFields.form.payment.invoice.fiscal_id != null)
                         result.form.payment.invoice.fiscal_id = bookingFields.form.payment.invoice.fiscal_id.CoreValue;
                     result.form.payment.invoice.address = new ExpandoObject();
                     if (bookingFields.form.payment.invoice.address.number != null)
                         result.form.payment.invoice.address.number = bookingFields.form.payment.invoice.address.number.CoreValue;
                     if (bookingFields.form.payment.invoice.address.floor != null)
                         result.form.payment.invoice.address.floor = bookingFields.form.payment.invoice.address.floor.CoreValue;
                     if (bookingFields.form.payment.invoice.address.department != null)
                         result.form.payment.invoice.address.department = bookingFields.form.payment.invoice.address.department.CoreValue;
                     if (bookingFields.form.payment.invoice.address.city_id != null)
                         result.form.payment.invoice.address.city_id = bookingFields.form.payment.invoice.address.city_id.CoreValue;
                     if (bookingFields.form.payment.invoice.address.city != null)
                         result.form.payment.invoice.address.city = bookingFields.form.payment.invoice.address.city.CoreValue;
                     result.form.payment.invoice.address.state = bookingFields.form.payment.invoice.address.state.CoreValue;
                     result.form.payment.invoice.address.country = bookingFields.form.payment.invoice.address.country.value; //this is fill with the response of service
                     if (bookingFields.form.payment.invoice.address.postal_code != null)
                         result.form.payment.invoice.address.postal_code = bookingFields.form.payment.invoice.address.postal_code.CoreValue;
                     if (bookingFields.form.payment.invoice.address.street != null)
                         result.form.payment.invoice.address.street = bookingFields.form.payment.invoice.address.street.CoreValue;
                     if (bookingFields.form.payment.invoice.fiscal_status != null)
                     {
                         result.form.payment.invoice.fiscal_status = bookingFields.form.payment.invoice.fiscal_status.CoreValue;
                         if (bookingFields.form.payment.invoice.fiscal_status.CoreValue != "FINAL")
                         {
                             result.form.payment.invoice.fiscal_name = bookingFields.form.payment.invoice.fiscal_name.CoreValue;
                         }
                     }
                 }


                 // Voucher

                 if (bookingFields.form.Voucher != null)
                 {
                     result.form.vouchers = bookingFields.form.vouchers.Select(x => x.CoreValue).ToList();
                 }

                 return result;

             });
        }
    }
}