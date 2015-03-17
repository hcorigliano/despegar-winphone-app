using Despegar.Core.Neo.Business.Hotels;
using Despegar.Core.Neo.Business.Hotels.BookingCompletePostResponse;
using Despegar.Core.Neo.Business.Hotels.BookingFields;
using Despegar.Core.Neo.Business.Hotels.CitiesAvailability;
using Despegar.Core.Neo.Business.Hotels.HotelDetails;
using Despegar.Core.Neo.Business.Hotels.HotelsAutocomplete;
using Despegar.Core.Neo.Business.Hotels.SearchBox;
using Despegar.Core.Neo.Business.Hotels.UserReviews;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Contract.API
{
    public interface IMAPIHotels
    {
        Task<HotelsAutocomplete> GetHotelsAutocomplete(string hotelString);
        Task<CitiesAvailability> GetHotelsAvailability(HotelSearchModel model);        
        Task<HotelDatails> GetHotelsDetail(string idHotel, string checkin, string checkout, string distribution);
        Task<HotelsBookingFields> GetBookingFields(HotelsBookingFieldsRequest bookingFieldPost);
        //Task<HotelUserReviews> GetHotelUserReviews(string hotelId, int limit, int offset, string language);

        Task<BookingCompletePostResponse> CompleteBooking(object bookingData, string id, string item_id);
    }
}