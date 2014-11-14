using Despegar.Core.Business.Flight;
using Despegar.Core.Business.Flight.BookingCompletePost;
using Despegar.Core.Business.Flight.BookingCompletePostResponse;
using Despegar.Core.Business.Flight.BookingFields;
using Despegar.Core.Business.Flight.CitiesAutocomplete;
using Despegar.Core.Business.Flight.Itineraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.IService
{

    /// <summary>
    /// Contract for accessing flight data service.
    /// </summary>
    public interface IFlightService
    {        
        Task<CitiesAutocomplete> GetCitiesAutocomplete(string cityString);
        Task<FlightsItineraries> GetItineraries(FlightSearchModel model);
        Task<BookingFields> GetBookingFields(BookingFieldPost bookingFieldPost);
        Task<BookingCompletePostResponse> CompleteBooking(object bookingCompletePost,string id);
    }
}