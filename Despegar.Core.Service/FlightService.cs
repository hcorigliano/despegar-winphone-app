
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
            FlightsItineraries result = await connector.GetAsync<FlightsItineraries>(searchModel.GetQueryUrl());

            // Add Indexes
            if (result.items != null) 
            {             
                foreach (var item in result.items)
                {
                    // outbound
                    if (item.outbound != null)
                    {                        
                        foreach (var route in item.outbound)
                        {
                            if (route.segments != null)
                            {
                                var i = 1;
                                foreach (var segment in route.segments)
                                {
                                    segment.Index = i;
                                    i++;
                                }
                            }
                        }
                    }
   
                        // inBound
                    if (item.inbound != null)
                    {
                        foreach (var route in item.inbound)
                        {
                            if (route.segments != null)
                            {
                                var i = 1;
                                foreach (var segment in route.segments)
                                {
                                    segment.Index = i;
                                    i++;
                                }
                            }
                        }
                    }          
                }
             }

            return result;
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