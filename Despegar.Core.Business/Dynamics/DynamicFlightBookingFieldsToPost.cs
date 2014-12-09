using Despegar.Core.Business.Flight.BookingFields;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Business.Dynamics
{
    public class DynamicFlightBookingFieldsToPost
    {
        private static dynamic BuildPassenger(Passenger passenger) {
            dynamic result = new ExpandoObject();
            result.type = passenger.type;
            result.document = new ExpandoObject();
            result.document.type = passenger.document.type.CoreValue;
            result.document.number = passenger.document.number.CoreValue;
            result.first_name = passenger.first_name.CoreValue;
            result.last_name = passenger.last_name.CoreValue;
            result.nationality = passenger.nationality.CoreValue;
            result.birthdate = passenger.birthdate.CoreValue;
            result.gender = passenger.gender.CoreValue;

            //Agregar todos los que falten
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

        public static object ToDynamic(BookingFields bookingFields)
        {
            bookingFields = FillBookingFields(bookingFields);

            dynamic result = new ExpandoObject();
            result.form = new ExpandoObject();

            result.form.passengers = bookingFields.form.passengers.Select(p => BuildPassenger(p)).ToList();
            
            result.form.contact = new ExpandoObject();
            result.form.contact.phones = bookingFields.form.contact.phones.Select(p => BuildPhones(p)).ToList();
            result.form.contact.email = bookingFields.form.contact.email.CoreValue;

            result.form.payment = new ExpandoObject();
            result.form.payment.card = new ExpandoObject();
            result.form.payment.card.security_code = bookingFields.form.payment.card.security_code.CoreValue;
            result.form.payment.card.owner_gender = bookingFields.form.payment.card.owner_gender.CoreValue;
            result.form.payment.card.expiration = bookingFields.form.payment.card.expiration.CoreValue;
            result.form.payment.card.number = bookingFields.form.payment.card.number.CoreValue;
            result.form.payment.card.owner_name = bookingFields.form.payment.card.owner_name.CoreValue;
            result.form.payment.card.owner_document = new ExpandoObject();
            result.form.payment.card.owner_document.type = bookingFields.form.payment.card.owner_document.type.CoreValue;
            result.form.payment.card.owner_document.number = bookingFields.form.payment.card.owner_document.number.CoreValue;
            result.form.payment.installment = new ExpandoObject();
            result.form.payment.installment.card_type = bookingFields.form.payment.installment.card_type.CoreValue;
            result.form.payment.installment.card_code = bookingFields.form.payment.installment.card_code.CoreValue;
            if (bookingFields.form.payment.installment.complete_card_code.CoreValue != null)
            {
                result.form.payment.installment.complete_card_code = bookingFields.form.payment.installment.complete_card_code.CoreValue;
            }
            result.form.payment.installment.quantity = Convert.ToInt32(bookingFields.form.payment.installment.quantity.CoreValue);

            if (bookingFields.form.payment.invoice != null)
            {
                result.form.payment.invoice = new ExpandoObject();
                result.form.payment.invoice.fiscal_id = bookingFields.form.payment.invoice.fiscal_id.CoreValue;
                result.form.payment.invoice.address = new ExpandoObject();
                result.form.payment.invoice.address.number = bookingFields.form.payment.invoice.address.number.CoreValue;
                if (bookingFields.form.payment.invoice.address.floor.CoreValue != null)
                {
                    result.form.payment.invoice.address.floor = bookingFields.form.payment.invoice.address.floor.CoreValue;
                }
                if (bookingFields.form.payment.invoice.address.department.CoreValue != null)
                {
                    result.form.payment.invoice.address.department = bookingFields.form.payment.invoice.address.department.CoreValue;
                }
                result.form.payment.invoice.address.country = bookingFields.form.payment.invoice.address.country.value;
                result.form.payment.invoice.address.postal_code = bookingFields.form.payment.invoice.address.postal_code.CoreValue;
                result.form.payment.invoice.address.street = bookingFields.form.payment.invoice.address.street.CoreValue;
                result.form.payment.invoice.fiscal_status = bookingFields.form.payment.invoice.fiscal_status.CoreValue;
                if (bookingFields.form.payment.invoice.fiscal_status.CoreValue != "FINAL")
                {
                    result.form.payment.invoice.fiscal_name = bookingFields.form.payment.invoice.fiscal_name.CoreValue;
                }
            }

            return result;

        }

        private static BookingFields FillBookingFields(BookingFields bookingFields)
        {
            bookingFields.form.contact.email.CoreValue = "bookingvuelos@despegar.com";
            bookingFields.form.contact.phones[0].area_code.CoreValue = "11";
            bookingFields.form.contact.phones[0].country_code.CoreValue = "54";
            bookingFields.form.contact.phones[0].number.CoreValue = "44444444";
            bookingFields.form.contact.phones[0].type.CoreValue = "HOME";
            bookingFields.form.passengers[0].birthdate.CoreValue = "1988-11-27";
            bookingFields.form.passengers[0].document.number.CoreValue = "12123123";
            bookingFields.form.passengers[0].document.type.CoreValue = "LOCAL";
            bookingFields.form.passengers[0].first_name.CoreValue = "Test";
            bookingFields.form.passengers[0].last_name.CoreValue = "Booking";
            bookingFields.form.passengers[0].gender.CoreValue = "MALE";
            bookingFields.form.passengers[0].nationality.CoreValue = "AR";
            bookingFields.form.payment.card.expiration.CoreValue = "2015-11";
            bookingFields.form.payment.card.number.CoreValue = "4242424242424242";
            bookingFields.form.payment.card.owner_document.number.CoreValue = "12123123";
            bookingFields.form.payment.card.owner_document.type.CoreValue = "LOCAL";
            bookingFields.form.payment.card.owner_gender.CoreValue = "MALE";
            bookingFields.form.payment.card.owner_name.CoreValue = "Test Booking";
            bookingFields.form.payment.card.security_code.CoreValue = "123";
            bookingFields.form.payment.installment.card_code.CoreValue = "VI";
            bookingFields.form.payment.installment.card_type.CoreValue = "CREDIT";
            bookingFields.form.payment.installment.quantity.CoreValue = "1";
            if (bookingFields.form.payment.invoice != null)
            {
                bookingFields.form.payment.invoice.address.city_id.CoreValue = "6585";
                bookingFields.form.payment.invoice.address.country.CoreValue = "AR";
                bookingFields.form.payment.invoice.address.department.CoreValue = "A";
                bookingFields.form.payment.invoice.address.number.CoreValue = "1234";
                bookingFields.form.payment.invoice.address.postal_code.CoreValue = "7777";
                bookingFields.form.payment.invoice.address.state.CoreValue = "14061";
                bookingFields.form.payment.invoice.address.street.CoreValue = "La Calle";
                bookingFields.form.payment.invoice.fiscal_id.CoreValue = "20121231238";
                bookingFields.form.payment.invoice.fiscal_name.CoreValue = "RazonSocial";
                bookingFields.form.payment.invoice.fiscal_status.CoreValue = "INSCR";
            }
            bookingFields.form.payment.installment.complete_card_code.CoreValue = "VI";
            return bookingFields;
        }

    }
}
