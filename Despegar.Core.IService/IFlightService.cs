using Despegar.Core.Neo.Business.Flight.BookingCompletePostResponse;
using Despegar.Core.Neo.Business.Flight.BookingFields;
using Despegar.Core.Neo.Business.Flight.CitiesAutocomplete;
using Despegar.Core.Neo.Business.Flight.Itineraries;
using Despegar.Core.Neo.Business.Flight.SearchBox;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.IService
{

    /// <summary>
    /// Contract for accessing flight data service.
    /// </summary>
    public interface IFlightService
    {        
        Task<CitiesAutocomplete> GetCitiesAutocomplete(string cityString);
        Task<FlightsItineraries> GetItineraries(FlightSearchModel model);
        Task<FlightBookingFields> GetBookingFields(FlightsBookingFieldRequest bookingFieldPost);
        Task<BookingCompletePostResponse> CompleteBooking(object bookingCompletePost,string id);
        Task<CitiesAutocomplete> GetNearCities(double latitude, double longitude);
    }
}