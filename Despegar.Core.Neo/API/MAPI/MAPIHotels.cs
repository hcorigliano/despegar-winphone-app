using Despegar.Core.Neo.API;
using Despegar.Core.Neo.Business.Hotels;
using Despegar.Core.Neo.Business.Hotels.BookingFields;
using Despegar.Core.Neo.Business.Hotels.CitiesAvailability;
using Despegar.Core.Neo.Business.Hotels.HotelDetails;
using Despegar.Core.Neo.Business.Hotels.HotelsAutocomplete;
using Despegar.Core.Neo.Business.Hotels.UserReviews;
using Despegar.Core.Neo.Connector;
using Despegar.Core.Neo.Contract;
using Despegar.Core.Neo.Contract.API;
using Despegar.Core.Neo.Contract.Connector;
using System;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.API.MAPI
{
    public class MAPIHotels : IMAPIHotels
    {
        private IMapiConnector connector;
        private ICoreContext context;

        public MAPIHotels(ICoreContext context, IMapiConnector connector)
        {
            this.context = context;
            this.connector = connector;
        }

        public async Task<HotelsAutocomplete> GetHotelsAutocomplete(string hotelString)
        {
            string serviceUrl = ServiceURL.GetServiceURL(ServiceKey.HotelsAutocomplete, hotelString);

            return await connector.GetAsync<HotelsAutocomplete>(serviceUrl, ServiceKey.HotelsAutocomplete);
        }

        public async Task<CitiesAvailability> GetHotelsAvailability(string checkin, string checkout, int destinationNumber, string distribution, string currency, int offset, int limit, string extraParameters)
        {
            string serviceUrl = ServiceURL.GetServiceURL(ServiceKey.HotelsAvailability, checkin, checkout, destinationNumber, distribution, currency, offset, limit, extraParameters);

            return await connector.GetAsync<CitiesAvailability>(serviceUrl, ServiceKey.HotelsAvailability);            
        }

        public async Task<HotelDatails> GetHotelsDetail(string idHotel, string checkin, string checkout, string distribution)
        {
            string serviceUrl = ServiceURL.GetServiceURL(ServiceKey.HotelsGetDetails, idHotel, checkin, checkout, distribution);

            return await connector.GetAsync<HotelDatails>(serviceUrl, ServiceKey.HotelsGetDetails);
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

            var result = await connector.PostAsync<HotelsBookingFields>(serviceUrl, bookingFieldPost, ServiceKey.HotelsBookingFields);

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
            string serviceUrl = ServiceURL.GetServiceURL(ServiceKey.HotelUserReview, hotelId, limit, offset, language);

            return await connector.GetAsync<HotelUserReviews>(serviceUrl, ServiceKey.HotelUserReview);
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