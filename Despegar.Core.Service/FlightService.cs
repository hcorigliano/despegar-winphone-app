
using Despegar.Core.Business;
using Despegar.Core.Connector;
using Despegar.Core.IService;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using Despegar.Core.Business.Flight.CitiesAutocomplete;
using Despegar.Core.Business.Flight.Itineraries;
using Despegar.Core.Business.Flight.BookingFields;

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
        /// Retrieves an itinerarie for a flight
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="departure_date"></param>
        /// <param name="adults"></param>
        /// <param name="return_date"></param>
        /// <param name="children"></param>
        /// <param name="infants"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="order_by"></param>
        /// <param name="order_type"></param>
        /// <param name="currency_code"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<FlightsItineraries> GetItinerariesFlights(string from, string to, string departure_date, int adults, string return_date, int children, int infants, int offset, int limit, string order_by, string order_type, string currency_code, string filter)
        {
            string serviceUrl = String.Format(ServiceURL.GetServiceURL(ServiceKey.FlightItineraries),from,to,departure_date,adults,return_date,children,infants,offset,limit,order_by,order_type,currency_code,filter);
            IConnector connector = context.GetServiceConnector(ServiceKey.FlightItineraries);

            return await connector.GetAsync<FlightsItineraries>(serviceUrl);
        }
       

        /// <summary>
        /// Retrieves an list of Cities
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
    }
}