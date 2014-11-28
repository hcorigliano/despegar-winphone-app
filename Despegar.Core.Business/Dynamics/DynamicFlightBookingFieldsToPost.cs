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
            result.birthdate = passenger.birthdate.coreValue;
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
            //TODO: Estos son los datos minimos para hacer una compra de un vuelo en Arg.
            dynamic result = new ExpandoObject();
            result.form = new ExpandoObject();
            
            result.form.passangers = bookingFields.form.passengers.Select(p => BuildPassenger(p)).ToList();
            
            result.form.contact = new ExpandoObject();
            result.form.contact.phones = bookingFields.form.contact.phones.Select(p => BuildPhones(p)).ToList();
            result.form.contact.email = bookingFields.form.contact.email.CoreValue;

            result.form.payment = new ExpandoObject();
            result.form.payment.card = new ExpandoObject();
            result.form.payment.card.security_code = bookingFields.form.payment.card.security_code.CoreValue;
            result.form.payment.card.owner_gender = bookingFields.form.payment.card.owner_gender.CoreValue;
            result.form.payment.card.expiration = bookingFields.form.payment.card.expiration.coreValue;
            result.form.payment.card.number = bookingFields.form.payment.card.number.CoreValue;
            result.form.payment.card.owner_name = bookingFields.form.payment.card.owner_name.CoreValue;
            result.form.payment.card.owner_document = new ExpandoObject();
            result.form.payment.card.owner_document.type = bookingFields.form.payment.card.owner_document.type.CoreValue;
            result.form.payment.card.owner_document.number = bookingFields.form.payment.card.number.CoreValue;
            result.form.payment.installment = new ExpandoObject();
            result.form.payment.installment.card_type = bookingFields.form.payment.installment.card_type.CoreValue;
            result.form.payment.installment.card_code = bookingFields.form.payment.installment.card_code.CoreValue;
            result.form.payment.installment.complete_card_code = bookingFields.form.payment.installment.complete_card_code.CoreValue;
            result.form.payment.installment.quantity = bookingFields.form.payment.installment.quantity.coreValue;

            if (bookingFields.form.payment.invoice != null)
            {
                result.form.payment.invoice = new ExpandoObject();
                result.form.payment.invoice.fiscal_id = bookingFields.form.payment.invoice.fiscal_id.CoreValue;
                result.form.payment.invoice.address = new ExpandoObject();
                result.form.payment.invoice.address.number = bookingFields.form.payment.invoice.address.number.CoreValue;
                result.form.payment.invoice.address.floor = bookingFields.form.payment.invoice.address.floor.CoreValue;
                result.form.payment.invoice.address.city_id = bookingFields.form.payment.invoice.address.city_id.CoreValue;
                result.form.payment.invoice.address.state = bookingFields.form.payment.invoice.address.state.CoreValue;
                result.form.payment.invoice.address.country = bookingFields.form.payment.invoice.address.country.value;
                result.form.payment.invoice.address.postal_code = bookingFields.form.payment.invoice.address.postal_code.CoreValue;
                result.form.payment.invoice.address.department = bookingFields.form.payment.invoice.address.department.CoreValue;
                result.form.payment.invoice.address.street = bookingFields.form.payment.invoice.address.street.CoreValue;
                result.form.payment.invoice.fiscal_status = bookingFields.form.payment.invoice.fiscal_status.CoreValue;
            }

            return result;

        }
    }
}
