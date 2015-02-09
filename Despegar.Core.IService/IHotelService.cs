using Despegar.Core.Neo.Business.Hotels;
using Despegar.Core.Neo.Business.Hotels.BookingFields;
using Despegar.Core.Neo.Business.Hotels.CitiesAvailability;
using Despegar.Core.Neo.Business.Hotels.HotelDetails;
using Despegar.Core.Neo.Business.Hotels.HotelsAutocomplete;
using Despegar.Core.Neo.Business.Hotels.UserReviews;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.IService
{
    public interface IHotelService
    {
        Task<HotelsAutocomplete> GetHotelsAutocomplete(string hotelString);
        Task<CitiesAvailability> GetHotelsAvailability(string checkin, string checkout, int destinationNumber, string distribution, string currency, int offset, int limit, string extraParameters);
        //Task<CitiesAvailability> GetNearHotelsAvailability(double latitude, double longitude, string checkin, string checkout, string distribution, string currency, int offset, int limit, string sort, string order); //http://backoffice.despegar.com/v3/mapi-hotels/docs/method/-City-availability
        Task<HotelDatails> GetHotelsDetail(string idHotel, string checkin, string checkout, string distribution);
        Task<HotelsBookingFields> GetBookingFields(HotelsBookingFieldsRequest bookingFieldPost);
        Task<HotelUserReviews> GetHotelUserReviews(string hotelId, int limit, int offset, string language);
    }
}