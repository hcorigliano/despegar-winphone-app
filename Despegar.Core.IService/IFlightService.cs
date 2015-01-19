using Despegar.Core.Business.Flight.BookingCompletePostResponse;
using Despegar.Core.Business.Flight.BookingFields;
using Despegar.Core.Business.Flight.CitiesAutocomplete;
using Despegar.Core.Business.Flight.Itineraries;
using Despegar.Core.Business.Flight.SearchBox;
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
        Task<FlightBookingFields> GetBookingFields(BookingFieldPost bookingFieldPost);
        Task<BookingCompletePostResponse> CompleteBooking(object bookingCompletePost,string id);
        Task<CitiesAutocomplete> GetNearCities(double latitude, double longitude);
    }
}