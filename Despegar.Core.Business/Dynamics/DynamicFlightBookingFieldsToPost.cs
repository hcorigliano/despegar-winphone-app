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
            result.document.type = passenger.document.type.coreValue;
            result.document.number = passenger.document.number.coreValue;
            result.first_name = passenger.first_name.coreValue;
            result.last_name = passenger.last_name.coreValue;
            result.nationality = passenger.nationality.coreValue;
            result.birthdate = passenger.birthdate.coreValue;
            result.gender = passenger.gender.coreValue;

            //Agregar todos los que falten
            return result;
        }

        public static dynamic BuildPhones(Phone phone)
        {
            dynamic result = new ExpandoObject();
            result.type = phone.type.coreValue;
            result.area_code = phone.area_code.coreValue;
            result.number = phone.number.coreValue;
            result.country_code = phone.country_code.coreValue;

            return result;
        }

        public static object ToDynamic(BookingFields bookingFields)
        {
            //TODO: Estos son los datos minimos para hacer una compra de un vuelo en Arg.
            bookingFields = FillBookingFields(bookingFields);

            dynamic result = new ExpandoObject();
            result.form = new ExpandoObject();

            result.form.passengers = bookingFields.form.passengers.Select(p => BuildPassenger(p)).ToList();
            
            result.form.contact = new ExpandoObject();
            result.form.contact.phones = bookingFields.form.contact.phones.Select(p => BuildPhones(p)).ToList();
            result.form.contact.email = bookingFields.form.contact.email.coreValue;

            result.form.payment = new ExpandoObject();
            result.form.payment.card = new ExpandoObject();
            result.form.payment.card.security_code = bookingFields.form.payment.card.security_code.coreValue;
            result.form.payment.card.owner_gender = bookingFields.form.payment.card.owner_gender.coreValue;
            result.form.payment.card.expiration = bookingFields.form.payment.card.expiration.coreValue;
            result.form.payment.card.number = bookingFields.form.payment.card.number.coreValue;
            result.form.payment.card.owner_name = bookingFields.form.payment.card.owner_name.coreValue;
            result.form.payment.card.owner_document = new ExpandoObject();
            result.form.payment.card.owner_document.type = bookingFields.form.payment.card.owner_document.type.coreValue;
            result.form.payment.card.owner_document.number = bookingFields.form.payment.card.owner_document.number.coreValue;
            result.form.payment.installment = new ExpandoObject();
            result.form.payment.installment.card_type = bookingFields.form.payment.installment.card_type.coreValue;
            result.form.payment.installment.card_code = bookingFields.form.payment.installment.card_code.coreValue;
            if (bookingFields.form.payment.installment.complete_card_code.coreValue != null)
            {
                result.form.payment.installment.complete_card_code = bookingFields.form.payment.installment.complete_card_code.coreValue;
            }
            result.form.payment.installment.quantity = Convert.ToInt32(bookingFields.form.payment.installment.quantity.coreValue);

            if (bookingFields.form.payment.invoice != null)
            {
                result.form.payment.invoice = new ExpandoObject();
                result.form.payment.invoice.fiscal_id = bookingFields.form.payment.invoice.fiscal_id.coreValue;
                result.form.payment.invoice.address = new ExpandoObject();
                result.form.payment.invoice.address.number = bookingFields.form.payment.invoice.address.number.coreValue;
                if (bookingFields.form.payment.invoice.address.floor.coreValue != null)
                {
                    result.form.payment.invoice.address.floor = bookingFields.form.payment.invoice.address.floor.coreValue;
                }
                if (bookingFields.form.payment.invoice.address.department.coreValue != null)
                {
                    result.form.payment.invoice.address.department = bookingFields.form.payment.invoice.address.department.coreValue;
                }
                result.form.payment.invoice.address.city_id = bookingFields.form.payment.invoice.address.city_id.coreValue;
                result.form.payment.invoice.address.state = bookingFields.form.payment.invoice.address.state.coreValue;
                result.form.payment.invoice.address.country = bookingFields.form.payment.invoice.address.country.value;
                result.form.payment.invoice.address.postal_code = bookingFields.form.payment.invoice.address.postal_code.coreValue;
                result.form.payment.invoice.address.street = bookingFields.form.payment.invoice.address.street.coreValue;
                result.form.payment.invoice.fiscal_status = bookingFields.form.payment.invoice.fiscal_status.coreValue;
                if(bookingFields.form.payment.invoice.fiscal_status.coreValue != "FINAL")
                {
                    result.form.payment.invoice.fiscal_name = bookingFields.form.payment.invoice.fiscal_name.coreValue;
                }
            }

            return result;

        }

        private static BookingFields FillBookingFields(BookingFields bookingFields)
        {
            bookingFields.form.contact.email.coreValue = "testchas@despegar.com";
            bookingFields.form.contact.phones[0].area_code.coreValue = "11";
            bookingFields.form.contact.phones[0].country_code.coreValue = "54";
            bookingFields.form.contact.phones[0].number.coreValue = "44444444";
            bookingFields.form.contact.phones[0].type.coreValue = "HOME";
            bookingFields.form.passengers[0].birthdate.coreValue = "1988-11-27";
            bookingFields.form.passengers[0].document.number.coreValue = "12123123";
            bookingFields.form.passengers[0].document.type.coreValue = "LOCAL";
            bookingFields.form.passengers[0].first_name.coreValue = "Test";
            bookingFields.form.passengers[0].last_name.coreValue = "Booking";
            bookingFields.form.passengers[0].gender.coreValue = "MALE";
            bookingFields.form.passengers[0].nationality.coreValue = "AR";
            bookingFields.form.payment.card.expiration.coreValue = "2015-11";
            bookingFields.form.payment.card.number.coreValue = "4242424242424242";
            bookingFields.form.payment.card.owner_document.number.coreValue = "12123123";
            bookingFields.form.payment.card.owner_document.type.coreValue = "LOCAL";
            bookingFields.form.payment.card.owner_gender.coreValue = "MALE";
            bookingFields.form.payment.card.owner_name.coreValue = "Test Booking";
            bookingFields.form.payment.card.security_code.coreValue = "123";
            bookingFields.form.payment.installment.card_code.coreValue = "VI";
            bookingFields.form.payment.installment.card_type.coreValue = "CREDIT";
            bookingFields.form.payment.installment.quantity.coreValue = "1";
            if (bookingFields.form.payment.invoice != null)
            {
                bookingFields.form.payment.invoice.address.city_id.coreValue = "6585";
                bookingFields.form.payment.invoice.address.country.coreValue = "AR";
                bookingFields.form.payment.invoice.address.department.coreValue = "A";
                bookingFields.form.payment.invoice.address.number.coreValue = "1234";
                bookingFields.form.payment.invoice.address.postal_code.coreValue = "7777";
                bookingFields.form.payment.invoice.address.state.coreValue = "B";
                bookingFields.form.payment.invoice.address.street.coreValue = "La Calle";
                bookingFields.form.payment.invoice.fiscal_id.coreValue = "20121231238";
                bookingFields.form.payment.invoice.fiscal_name.coreValue = "RazonSocial";
                bookingFields.form.payment.invoice.fiscal_status.coreValue = "INSCR";
            }
            bookingFields.form.payment.installment.complete_card_code.coreValue = "VI";
            return bookingFields;
        }

    }
}
