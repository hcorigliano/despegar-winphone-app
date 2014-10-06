using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Despegar.LegacyCore.Util;
using Despegar.LegacyCore.Service;
using Despegar.LegacyCore.Model;
using Despegar.LegacyCore.Connector.Domain.API;

namespace Despegar.LegacyCore.Model
{
    public class FlightsBookingModel
    {

        public FlightsBookingModel()
        {
            Logger.Info("[model:flight:booking] Flights Booking model created");
        }

        public async Task<FlightBookingFields> GetBookingFields(string ticket, string itinerary, string device)
        {
            return await APIFlightsService.BookingFields(ticket, itinerary, device);
        }

        public async Task<FlightBookingBook> Buy(FlightBookingFields data)
        {
            //string mocked = "{\"ticket\" : \"8de4ec16-a619-11e3-82ea-fa163e3c254e\",\"roomPackIdsInputDefinitionMap\" : { \"inputDefinition\" : {\"passengerDefinitions\" : [ {\"firstName\": { \"value\" : \"test\" },\"lastName\": { \"value\" : \"booking\" }},{\"firstName\": { \"value\" : \"test\" },\"lastName\": { \"value\" : \"hoteles\" }} ],\"paymentDefinition \" : { \"cardDefinition\": {\"number\": { \"value\" : \"4242424242424242\" },\"expiration\": { \"value\" : \"2014-04\" },\"securityCode\": { \"value\" : \"123\" },\"bankCode\": { \"value\" : \"\" },\"cardCode\": { \"value\" : \"VI\" },\"cardType\": { \"value\" : \"CREDIT\" },\"ownerName\": { \"value\" : \"test hoteles\" },\"ownerGender\": { \"value\" : \"M\" },\"ownerDocumentDefinition\" : {\"type\": { \"value\" : \"LOCAL\" },\"number\": { \"value\" : \"12345678\" }}} },\"contactDefinition\" : {\"email\": { \"value\" : \"testhoteles@despegar.com\" },\"phoneDefinitions\": [ {\"type\": { \"value\" : \"CELULAR\" },\"countryCode\": { \"value\" : \"54\" },\"areaCode\": { \"value\" : \"11\" },\"number\": { \"value\" : \"12345678\" }}]}} }}";
            //string serialized = mocked.Replace("\\", "");

            string serialized = data.data.Serialize();
            Logger.Info(String.Format("[model:flight:booking] Trying to buy with data: {0}", serialized));
            FlightBookingBook bookResponse = await APIFlightsService.Book(serialized);
            return bookResponse;
        }
    }
}
