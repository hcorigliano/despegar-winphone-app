using Despegar.Core.Neo.API;
using Despegar.Core.Neo.API.MAPI;
using Despegar.Core.Neo.Business.Flight.BookingCompletePostResponse;
using Despegar.Core.Neo.Business.Flight.BookingFields;
using Despegar.Core.Neo.Business.Flight.CitiesAutocomplete;
using Despegar.Core.Neo.Business.Flight.Itineraries;
using Despegar.Core.Neo.Business.Flight.SearchBox;
using Despegar.Core.Neo.Connector;
using Despegar.Core.Neo.Contract;
using Despegar.Core.Neo.Contract.API;
using Despegar.Core.Neo.Contract.Connector;
using Despegar.Core.Neo.Exceptions;
using System;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.API.MAPI
{
    internal class MAPIFlights : IMAPIFlights
    {
        private IMapiConnector connector;
        private ICoreContext context;

        public MAPIFlights(ICoreContext context, IMapiConnector connector)
        {
            this.context = context;
            this.connector = connector;
        }
        
        public async Task<FlightsItineraries> GetItineraries(FlightSearchModel searchModel)
        {
            FlightsItineraries result = await connector.GetAsync<FlightsItineraries>(searchModel.GetQueryUrl(), ServiceKey.FlightItineraries);

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

                    if (item.routes != null && item.inbound == null && item.outbound == null)
                    {
                        //Multiple
                        foreach(var route in item.routes)
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
                
        public async Task<CitiesAutocomplete> GetCitiesAutocomplete(string cityString)
        {
            string serviceUrl = ServiceURL.GetServiceURL(ServiceKey.FlightsCitiesAutocomplete, cityString);

            return await connector.GetAsync<CitiesAutocomplete>(serviceUrl, ServiceKey.FlightsCitiesAutocomplete);
        }

        public async Task<FlightBookingFields> GetBookingFields(FlightsBookingFieldRequest bookingFieldPost)
        {
            string serviceUrl = ServiceURL.GetServiceURL(ServiceKey.FlightsBookingFields, null);            

            return await connector.PostAsync<FlightBookingFields>(serviceUrl, bookingFieldPost, ServiceKey.FlightsBookingFields);
        }

        public async Task<BookingCompletePostResponse> CompleteBooking(object bookingCompletePost,string id)
        {
            string serviceUrl = ServiceURL.GetServiceURL(ServiceKey.FlightsBookingCompletePost, id);

            try
            {
                return await connector.PostAsync<BookingCompletePostResponse>(serviceUrl, bookingCompletePost, ServiceKey.FlightsBookingCompletePost);
            }
            catch (APIErrorException e)
            {               
               return new BookingCompletePostResponse() { Error = e.ErrorData };                
            }
            catch(Exception e)
            {
                throw e; // redundant
            }
        }

        public async Task<CitiesAutocomplete> GetNearCities(double latitude, double longitude)
        {
            string serviceUrl = ServiceURL.GetServiceURL(ServiceKey.FlightsNearCities, latitude, longitude);
            serviceUrl = serviceUrl.Replace(',', '.');

            return await connector.GetAsync<CitiesAutocomplete>(serviceUrl, ServiceKey.FlightsNearCities);
        }
    }
}