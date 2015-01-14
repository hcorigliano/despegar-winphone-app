using Despegar.Core.Business;
using Despegar.Core.Business.Hotels.CitiesAvailability;
using Despegar.Core.Business.Hotels.HotelsAutocomplete;
using Despegar.Core.Connector;
using Despegar.Core.Exceptions;
using Despegar.Core.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Despegar.Core.Business.Hotels.BookingFields;

namespace Despegar.Core.Service
{
    public class HotelService : IHotelService
    {
        private CoreContext context;

        public HotelService(CoreContext context)
        {
            this.context = context;
        }

        public async Task<HotelsAutocomplete> GetHotelsAutocomplete(string hotelString)
        {
            string serviceUrl = string.Format(ServiceURL.GetServiceURL(ServiceKey.HotelsAutocomplete),hotelString);
            IConnector connector = context.GetServiceConnector(ServiceKey.HotelsAutocomplete);

            return await connector.GetAsync<HotelsAutocomplete>(serviceUrl);
        }

        public async Task<CitiesAvailability> GetHotelsAvailability(int number , string checkin , string checkout , string distribution , string currency , int offset , int limit , string sort , string order)
        {
            string serviceUrl = String.Format(ServiceURL.GetServiceURL(ServiceKey.HotelsAvailability),number , checkin , checkout , distribution , currency , offset , limit , sort , order);
            IConnector connector = context.GetServiceConnector(ServiceKey.HotelsAutocomplete);

            return await connector.GetAsync<CitiesAvailability>(serviceUrl);            
        }

        public async Task<CitiesAvailability> GetNearHotelsAvailability(double latitude, double longitude, string checkin, string checkout, string distribution, string currency, int offset , int limit , string sort , string order)
        {
            string serviceUrl = String.Format(ServiceURL.GetServiceURL(ServiceKey.HotelsAvailability), latitude, longitude, checkin, checkout, distribution, currency, offset , limit , sort , order);
            IConnector connector = context.GetServiceConnector(ServiceKey.HotelsAutocomplete);

            return await connector.GetAsync<CitiesAvailability>(serviceUrl);
            
        }

        public async Task<HotelsBookingFields> GetBookingFields(BookingFieldsPost bookingFieldPost)
        {
            string serviceUrl = ServiceURL.GetServiceURL(ServiceKey.HotelsBookingFields);
            IConnector connector = context.GetServiceConnector(ServiceKey.HotelsBookingFields);

            return await connector.PostAsync<HotelsBookingFields>(serviceUrl, bookingFieldPost);
        }

        //public async Task<BookingCompletePostResponse> CompleteBooking(object bookingCompletePost, string id)
        //{
        //    string serviceUrl = String.Format(ServiceURL.GetServiceURL(ServiceKey.HotelsBookingCompletePost), id);
        //    IConnector connector = context.GetServiceConnector(ServiceKey.HotelsBookingCompletePost);

        //    try
        //    {
        //        return await connector.PostAsync<BookingCompletePostResponse>(serviceUrl, bookingCompletePost);
        //    }
        //    catch (APIErrorException e)
        //    {
        //        return new BookingCompletePostResponse() { Error = e.ErrorData };
        //    }
        //    catch (Exception e)
        //    {
        //        throw e; // redundants
        //    }
        //}
    }
}