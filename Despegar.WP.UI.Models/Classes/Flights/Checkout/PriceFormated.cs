using Despegar.Core.Business.Flight.BookingFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model.Classes.Flights.Checkout
{
    public class PriceFormated : Price
    {
        public string children_quantity { get; set; }
        public string children_base { get; set; }
        public string infant_quantity { get; set; }
        public string infant_base { get; set; }
        public string adult_quantity { get; set; }

        public PriceFormated(BookingFields booking)
        {
            currency = booking.price.currency;
            total = booking.price.total;
            taxes = booking.price.taxes;
            retention = booking.price.retention;
            charges = booking.price.charges;
            adult_base = booking.price.adult_base;
            adults_subtotal = booking.price.adults_subtotal;
            children_subtotal = booking.price.children_subtotal;
            infants_subtotal = booking.price.infants_subtotal;
            final_price = booking.price.final_price;

            children_quantity = booking.form.passengers.Count(p => p.type == "CHILD").ToString();
            infant_quantity = booking.form.passengers.Count(p => p.type == "INFANT").ToString();
            adult_quantity = booking.form.passengers.Count(p => p.type == "ADULT").ToString();

            if (children_subtotal != null)
            children_base = (children_subtotal / Convert.ToInt32(children_quantity)).ToString();
            if(infants_subtotal != null)
            infant_base = (infants_subtotal / Convert.ToInt32(infant_quantity)).ToString();
        }

    }
}
