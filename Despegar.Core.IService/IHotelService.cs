﻿using Despegar.Core.Business.Hotels.BookingFields;
using Despegar.Core.Business.Hotels.CitiesAvailability;
using Despegar.Core.Business.Hotels.HotelsAutocomplete;
using System.Threading.Tasks;

namespace Despegar.Core.IService
{
    public interface IHotelService
    {
        Task<HotelsAutocomplete> GetHotelsAutocomplete(string hotelString);
        Task<CitiesAvailability> GetHotelsAvailability(int number , string checkin , string checkout , string distribution , string currency , int offset , int limit , string sort , string order );
        Task<CitiesAvailability> GetNearHotelsAvailability(double latitude, double longitude, string checkin, string checkout, string distribution, string currency, int offset, int limit, string sort, string order); //http://backoffice.despegar.com/v3/mapi-hotels/docs/method/-City-availability
        Task<BookingFields> GetBookingFields(BookingFieldsPost bookingFieldPost);
    }
}