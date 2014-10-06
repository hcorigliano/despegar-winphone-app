using Despegar.LegacyCore.Connector;
using Despegar.LegacyCore.Connector.Domain.API;
using Despegar.LegacyCore.Util;
using Despegar.LegacyCore.ViewModel;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;


namespace Despegar.LegacyCore.Service
{
    public class APIHotelsService
    {


        public static async Task<HotelAvailability> Availability(string hotel, string checkin, string checkout, string distribution)
        {
            APIConnector ConnectorAPI = APIConnector.Instance;
            HttpRequestMessage httpMessage = ConnectorAPI.MessageBuilder("hotels_availability_booking");
            string url = string.Format(httpMessage.RequestUri.ToString(), hotel, checkin, checkout, distribution, "true","", "");
            httpMessage.RequestUri = new Uri(url, UriKind.Absolute);

            Logger.Info("[connector:req] Hotels Booking Availability service called (channel:" + APIConnector.Instance.Channel + "): " + httpMessage.RequestUri.ToString());
            string serviceData = await ConnectorAPI.SendAsync(httpMessage);
            HotelAvailability booking = JsonConvert.DeserializeObject<HotelAvailability>(serviceData);
            if (booking.errors != null)
                return AppDelegate.Instance.RequestError(new Exception());

            Logger.Info("[connector:res] Hotels Booking Availability service received: took " + booking.meta.time);
            return booking;
        }


        public static async Task<HotelBookingFields> BookingFields(string sessionTicket, string device)
        {
            APIConnector ConnectorAPI = APIConnector.Instance;
            HttpRequestMessage httpMessage = ConnectorAPI.MessageBuilder("hotels_booking_fields");

            string test = "";
            
            #if DEBUG
            test = "&test=true";
            #endif

            string url = string.Format(httpMessage.RequestUri.ToString(), sessionTicket, test, device); //TODO: HttpUtility.UrlEncode(device)
            httpMessage.RequestUri = new Uri(url, UriKind.Absolute);

            Logger.Info("[connector:req] Hotels Booking Fields service called (channel:" + APIConnector.Instance.Channel + "): " + httpMessage.RequestUri.ToString());
            string serviceData = await ConnectorAPI.SendAsync(httpMessage);
            HotelBookingFields fields = JsonConvert.DeserializeObject<HotelBookingFields>(serviceData);
            if (fields.errors != null)
                return AppDelegate.Instance.RequestError(new Exception());

            Logger.Info("[connector:res] Hotels Booking Fields service received: took " + fields.meta.time);
            return fields;
        }


        public static async Task<HotelBookingBook> Book(string room, int payment, string data)
        {
            APIConnector ConnectorAPI = APIConnector.Instance;

            HttpRequestMessage httpMessage = ConnectorAPI.ContentBuilderSecure("hotels_booking_book", data);

            string url = string.Format(httpMessage.RequestUri.ToString(), room, payment.ToString());
            httpMessage.RequestUri = new Uri(url, UriKind.Absolute);

            Logger.Info("[connector:req] Hotels Book service called (channel:" + APIConnector.Instance.Channel + "): " + httpMessage.RequestUri.ToString());

            string serviceData = await ConnectorAPI.SendAsync(httpMessage);
            HotelBookingBook response = JsonConvert.DeserializeObject<HotelBookingBook>(serviceData);
            Logger.Info("[connector:req] Hotels Book service response took " + response.meta.time + " :" + serviceData);
            return response;
        }
    }
}
