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
using Despegar.Core.Business.Hotels.HotelDetails;
using Despegar.Core.Business.Hotels;
using Despegar.Core.Business.Hotels.UserReviews;

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

        public async Task<CitiesAvailability> GetHotelsAvailability(string checkin, string checkout, int destinationNumber, string distribution, string currency, int offset, int limit, string extraParameters)
        {
            string serviceUrl = String.Format(ServiceURL.GetServiceURL(ServiceKey.HotelsAvailability), checkin, checkout, destinationNumber, distribution, currency, offset, limit, extraParameters);
            IConnector connector = context.GetServiceConnector(ServiceKey.HotelsAutocomplete);

            return await connector.GetAsync<CitiesAvailability>(serviceUrl);            
        }

        public async Task<HotelDatails> GetHotelsDetail(string idHotel, string checkin, string checkout, string distribution)
        {
            string serviceUrl = String.Format(ServiceURL.GetServiceURL(ServiceKey.HotelsGetDetails), idHotel, checkin, checkout, distribution);
            IConnector connector = context.GetServiceConnector(ServiceKey.HotelsGetDetails);

            return await connector.GetAsync<HotelDatails>(serviceUrl);
        }

        //public async Task<CitiesAvailability> GetNearHotelsAvailability(double latitude, double longitude, string checkin, string checkout, string distribution, string currency, int offset , int limit , string sort , string order)
        //{
        //    string serviceUrl = String.Format(ServiceURL.GetServiceURL(ServiceKey.HotelsAvailability), latitude, longitude, checkin, checkout, distribution, currency, offset , limit , sort , order);
        //    IConnector connector = context.GetServiceConnector(ServiceKey.HotelsAutocomplete);

        //    return await connector.GetAsync<CitiesAvailability>(serviceUrl);
            
        //}

        public async Task<HotelsBookingFields> GetBookingFields(HotelsBookingFieldsRequest bookingFieldPost)
        {
            string serviceUrl = ServiceURL.GetServiceURL(ServiceKey.HotelsBookingFields);
            IConnector connector = context.GetServiceConnector(ServiceKey.HotelsBookingFields);

            var result = await connector.PostAsync<HotelsBookingFields>(serviceUrl, bookingFieldPost);

            int i = 1;
            foreach (var passenger in result.form.passengers)
            {
                passenger.Index = i;
                i++;
            }

            return result;
        }

        public async Task<HotelUserReviews> GetHotelUserReviews(string hotelId, int limit, int offset, string language)
        {
            //NOT IMPLEMENTED YET (this use api v3 connector and this is not implemented)
            string serviceUrl = String.Format(ServiceURL.GetServiceURL(ServiceKey.HotelsAvailability), hotelId,  limit,  offset,  language);
            IConnector connector = context.GetServiceConnector(ServiceKey.HotelsAutocomplete);

            return await connector.GetAsync<HotelUserReviews>(serviceUrl);
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