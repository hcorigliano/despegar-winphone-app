using Despegar.Core.Business.Flight.BookingFields;
using Despegar.Core.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model
{
    public class FlightsCheckoutModel
    {
        private IFlightService flightService;

        public FlightsCheckoutModel()
        {
            flightService = GlobalConfiguration.CoreContext.GetFlightService();
        }

        public async Task<BookingFields> GetBookingFields(BookingFieldPost bookingFieldPost)
        {           
            return (await flightService.GetBookingFields(bookingFieldPost));
        }

    }
}
