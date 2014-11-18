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
    }
}
