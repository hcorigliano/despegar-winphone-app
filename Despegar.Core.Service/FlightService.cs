
using Despegar.Core.Business;
using Despegar.Core.Business.Flight.BookingCompletePostResponse;
using Despegar.Core.Business.Flight.BookingFields;
using Despegar.Core.Business.Flight.CitiesAutocomplete;
using Despegar.Core.Business.Flight.Itineraries;
using Despegar.Core.Business.Flight.SearchBox;
using Despegar.Core.Connector;
using Despegar.Core.IService;
using System;
using System.Threading.Tasks;

namespace Despegar.Core.Service
{
    public class FlightService : IFlightService
    {
        private CoreContext context;

        public FlightService(CoreContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Retrieves Flights Search result
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<FlightsItineraries> GetItineraries(FlightSearchModel searchModel)
        {           
            IConnector connector = context.GetServiceConnector(ServiceKey.FlightItineraries);
            return await connector.GetAsync<FlightsItineraries>(searchModel.GetQueryUrl());
        }
        
        /// <summary>
        /// retrieves Autocompletes city list
        /// </summary>
        /// <param name="cityString"></param>
        /// <returns></returns>
        public async Task<CitiesAutocomplete> GetCitiesAutocomplete(string cityString)
        {
            string serviceUrl = String.Format(ServiceURL.GetServiceURL(ServiceKey.FlightCitiesAutocomplete), cityString);
            IConnector connector = context.GetServiceConnector(ServiceKey.FlightCitiesAutocomplete);

            return await connector.GetAsync<CitiesAutocomplete>(serviceUrl);
        }

        /// <summary>
        /// Retrieves the booking fields
        /// </summary>
        /// <param name="bookingFieldPost"></param>
        /// <returns></returns>
        public async Task<BookingFields> GetBookingFields(BookingFieldPost bookingFieldPost)
        {
            string serviceUrl = ServiceURL.GetServiceURL(ServiceKey.FlightsBookingFields);
            IConnector connector = context.GetServiceConnector(ServiceKey.FlightsBookingFields);
            //#if DEBUG
            //serviceUrl += "&test=test";
            //#endif


            return await connector.PostAsync<BookingFields>(serviceUrl, bookingFieldPost);
        }

        /// <summary>
        /// Completes the Booking process
        /// </summary>
        /// <param name="bookingCompletePost"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BookingCompletePostResponse> CompleteBooking(object bookingCompletePost,string id)
        {
            string serviceUrl = String.Format(ServiceURL.GetServiceURL(ServiceKey.BookingCompletePost),id);
            IConnector connector = context.GetServiceConnector(ServiceKey.BookingCompletePost);

            return await connector.PostAsync<BookingCompletePostResponse>(serviceUrl, bookingCompletePost);
        }
        
    }
}