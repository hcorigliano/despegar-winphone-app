
using Despegar.Core.Business;
﻿using Despegar.Core.Business.Flight.BookingFields;
using Despegar.Core.Business.Flight.CitiesAutocomplete;
using Despegar.Core.Business.Flight.Itineraries;
using Despegar.Core.IService;
using Despegar.Core.Service;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model
{
    public class HomeModel
    {
        //TODO all this code must be changed it is just for first time only.
        private IFlightService flightService;

        public HomeModel()             
        {
		    flightService = GlobalConfiguration.CoreContext.GetFlightService();		            
            
            //TODO: Sacar Mocked
            //GlobalConfiguration.CoreContext.AddMock(ServiceKey.FlightCitiesAutocomplete, MockKey.FlightCitiesAutocompleteBue);
            //GlobalConfiguration.CoreContext.AddMock(ServiceKey.FlightItineraries, MockKey.ItinerarieBueToLax);
            GlobalConfiguration.CoreContext.AddMock(ServiceKey.FlightsBookingFields, MockKey.BookingFieldBuetoMia);
            
            GetCities("bue");
            GetItineraries("BUE","LAX","2014-10-10", 1, "2014-10-12",0,0,0,10,"","","ARS","");

            BookingFieldPost bookingFieldPost = new BookingFieldPost();
            bookingFieldPost.inbound_choice = 1;
            bookingFieldPost.outbound_choice = 1;
            bookingFieldPost.itinerary_id = "prism_AR_0_FLIGHTS_A-1_C-0_I-0_RT-BUEMIA20141010-MIABUE20141013_xorigin-api!0!C_1550550186!1,1";

            GetBooking(bookingFieldPost);
        }

        public async Task<CitiesAutocomplete> GetCities(string cityString)
        {
            cityString = "bue";
            return (await flightService.GetCitiesAutocomplete(cityString));
        }

        public async Task<FlightsItineraries> GetItineraries(string from, string to, string departure_date, int adults, string return_date, int children, int infants, int offset, int limit, string order_by, string order_type, string currency_code, string filter)
        {
            return ( await flightService.GetItinerariesFlights(from,to,departure_date,adults,return_date,children,infants,offset,limit,order_by,order_type,currency_code,filter));
        }

        public async Task<BookingFields> GetBooking(BookingFieldPost bookingFieldPost)
        {
            return (await flightService.GetBookingFields(bookingFieldPost));
        }

    }
}