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
    public class HotelsBookingModel
    {

        public HotelsBookingModel()
        {
            Logger.Info("[model:hotels:booking] Hotels Booking model created");
        }

        public async Task<HotelBookingFields> GetBookingFields(string sessionTicket, string device)
        {
            return await APIHotelsService.BookingFields(sessionTicket, device);
        }

        public async Task<HotelBookingBook> Buy(string room, int paymentId, HotelBookingFields data)
        {
            //TODO: MOCKED
            //string mocked = "{\"ticket\" : \"ed7fe80a-2d55-11e4-8880-fa163e7fe991\",\"roomPackId\":\"1\",\"paymentByMethodId\":\""+ paymentId +"\",\"roomPackIdsInputDefinitionMap\" : { \"inputDefinition\" : {\"passengerDefinitions\" : [ {\"firstName\": { \"value\" : \"test\" },\"lastName\": { \"value\" : \"booking\" }} ],\"paymentDefinition\" : {\"cardDefinition\": {\"number\": { \"value\" : \"4242424242424242\" },\"expiration\": { \"value\" : \"2016-01\" },\"securityCode\": { \"value\" : \"123\" },\"bankCode\": { \"value\" : \"\" },\"cardCode\": { \"value\" : \"VI\" },\"cardType\": { \"value\" : \"CREDIT\" },\"ownerGender\": { \"value\" : \"M\" },\"ownerDocumentDefinition\" : {\"type\": { \"value\" : \"LOCAL\" },\"number\": { \"value\" : \"12123123\" }},\"ownerName\": { \"value\" : \"test booking\" }},\"invoiceDefinition\": { \"value\" : {\"taxStatus\": { \"value\" : \"FINAL_CONSUMER\" },\"fiscalDocument\": { \"value\" : \"20121231239\" },\"invoiceName\" : { \"value\" : \"\" },\"billingAddress\" : {\"stateId\": { \"value\" : \"\" },\"cityId\": { \"value\" : \"\" },\"postalCode\": { \"value\" : \"\" },\"street\": { \"value\" : \"\" },\"number\": { \"value\" : \"\" },\"floor\": { \"value\" : \"\" },\"department\": { \"value\" : \"\" }}} }},\"contactDefinition\" : {\"email\": { \"value\" : \"testhoteles@despegar.com\" },\"phoneDefinitions\": [ {\"type\": { \"value\" : \"CELULAR\" },\"countryCode\": { \"value\" : \"54\" },\"areaCode\": { \"value\" : \"11\" },\"number\": { \"value\" : \"54156423\" }}]}} }}";
            //string serialized = mocked.Replace("\\","");

            string serialized = data.data.Serialize().Replace("\\", "");
            Logger.Info(String.Format("[model:booking] Trying to buy with data: {0}", serialized));
            HotelBookingBook bookResponse = await APIHotelsService.Book(room, paymentId, serialized);
            return bookResponse;
        }
    }
}
