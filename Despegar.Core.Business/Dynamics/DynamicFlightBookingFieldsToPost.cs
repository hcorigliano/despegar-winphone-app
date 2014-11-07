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
            dynamic result = new ExpandoObject();
            result.form = new ExpandoObject();
            
            result.form.passangers = bookingFields.form.passengers.Select(p => BuildPassenger(p)).ToList();
            
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
            result.form.payment.card.owner_document.number = bookingFields.form.payment.card.number.coreValue;
            result.form.payment.installment = new ExpandoObject();
            result.form.payment.installment.card_type = bookingFields.form.payment.installment.card_type.coreValue;
            result.form.payment.installment.card_code = bookingFields.form.payment.installment.card_code.coreValue;
            result.form.payment.installment.complete_card_code = bookingFields.form.payment.installment.complete_card_code.coreValue;
            result.form.payment.installment.quantity = bookingFields.form.payment.installment.quantity.coreValue;

            if (bookingFields.form.payment.invoice != null)
            {
                result.form.payment.invoice = new ExpandoObject();
                result.form.payment.invoice.fiscal_id = bookingFields.form.payment.invoice.fiscal_id;
                result.form.payment.invoice.address = new ExpandoObject();
                result.form.payment.invoice.address.number = bookingFields.form.payment.invoice.address.number.coreValue;
                result.form.payment.invoice.address.floor = bookingFields.form.payment.invoice.address.floor.coreValue;
                result.form.payment.invoice.address.city_id = bookingFields.form.payment.invoice.address.city_id.coreValue;
                result.form.payment.invoice.address.state = bookingFields.form.payment.invoice.address.state.coreValue;
                result.form.payment.invoice.address.country = bookingFields.form.payment.invoice.address.country.coreValue;
                result.form.payment.invoice.address.postal_code = bookingFields.form.payment.invoice.address.postal_code.coreValue;
                result.form.payment.invoice.address.department = bookingFields.form.payment.invoice.address.department.coreValue;
                result.form.payment.invoice.address.street = bookingFields.form.payment.invoice.address.street.coreValue;
                result.form.payment.invoice.fiscal_status = bookingFields.form.payment.invoice.fiscal_status.coreValue;
            }

            return result;

        }
      
    }
}
