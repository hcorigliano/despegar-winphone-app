using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Despegar.LegacyCore.Connector;
using Despegar.LegacyCore.Connector.Domain.API;
using System.Net.Http;
using Newtonsoft.Json;
using Despegar.LegacyCore.Util;
using System.Net;
using Despegar.LegacyCore.ViewModel;


namespace Despegar.LegacyCore.Service
{
    public class APIFlightsService
    {

        public static async Task<FlightAvailability> Availability(string ticket, string itinerary)
        {
            APIConnector ConnectorAPI = APIConnector.Instance;
            HttpRequestMessage httpMessage = ConnectorAPI.MessageBuilder("flights_availability_reprice");
            string url = string.Format(httpMessage.RequestUri.ToString(), ticket, itinerary);
            httpMessage.RequestUri = new Uri(url, UriKind.Absolute);

            Logger.Info("[connector:req] Flights Availability Itineraries service called (channel:" + APIConnector.Instance.Channel + "): " + httpMessage.RequestUri.ToString());

            string serviceData = await ConnectorAPI.SendAsync(httpMessage);
            FlightAvailability booking = JsonConvert.DeserializeObject<FlightAvailability>(serviceData);
            if (booking.errors != null)
                return AppDelegate.Instance.RequestError(new Exception());

            Logger.Info("[connector:res] Hotels Availability Itineraries service received: took " + booking.meta.time);
            return booking;
        }


        public static async Task<FlightBookingFields> BookingFields(string ticket, string itinerary, string device)
        {
            APIConnector ConnectorAPI = APIConnector.Instance;
            HttpRequestMessage httpMessage = ConnectorAPI.MessageBuilder("flights_booking_fields");

            string test = "";
            
            #if DEBUG
            test = "&test=test";
            #endif

            string url = string.Format(httpMessage.RequestUri.ToString(), ticket, itinerary, test, device); //HttpUtility.UrlEncode(device)
            httpMessage.RequestUri = new Uri(url, UriKind.Absolute);

            Logger.Info("[connector:req] Flights Booking Fields service called (channel:" + APIConnector.Instance.Channel + "): " + httpMessage.RequestUri.ToString());

            string serviceData = await ConnectorAPI.SendAsync(httpMessage);
            FlightBookingFields fields = JsonConvert.DeserializeObject<FlightBookingFields>(serviceData);
            if (fields.errors != null)
                return AppDelegate.Instance.RequestError(new Exception());

            Logger.Info("[connector:res] Flights Booking Fields service received: took " + fields.meta.time);
            return fields;
        }


        public static async Task<FlightBookingBook> Book(string data)
        {
            APIConnector ConnectorAPI = APIConnector.Instance;

            HttpRequestMessage httpMessage = ConnectorAPI.ContentBuilder("flights_booking_book", data);
            Logger.Info("[connector:req] Flights Book service called (channel:" + APIConnector.Instance.Channel + "): " + httpMessage.RequestUri.ToString());

            string serviceData = await ConnectorAPI.SendAsync(httpMessage);
            FlightBookingBook response = JsonConvert.DeserializeObject<FlightBookingBook>(serviceData);
            Logger.Info("[connector:req] Flights Book service response took " + response.meta.time + " :" + serviceData);
            return response;
        }
    }
}
