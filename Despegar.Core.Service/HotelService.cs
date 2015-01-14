using Despegar.Core.Business;
using Despegar.Core.Business.Hotels.CitiesAvailability;
using Despegar.Core.Business.Hotels.HotelsAutocomplete;
using Despegar.Core.Connector;
using Despegar.Core.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Service
{
    public class HotelService : IHotelService
    {
        private CoreContext context;

        public HotelService(CoreContext context)
        {
            this.context = context;
        }

        public async Task<HotelsAutocomplete> GetHotelsAutocomplete(string hotelString)
        {
            string serviceUrl = ServiceURL.GetServiceURL(ServiceKey.HotelsAutocomplete);
            IConnector connector = context.GetServiceConnector(ServiceKey.HotelsAutocomplete);

            return await connector.GetAsync<HotelsAutocomplete>(serviceUrl);
        }

        public async Task<CitiesAvailability> GetHotelsAvailability(string checkin, string checkout, int destinationNumber, string distribution, string currency, int offset, int limit, string order)
        {
            string serviceUrl = String.Format(ServiceURL.GetServiceURL(ServiceKey.HotelsAvailability), checkin, checkout, destinationNumber, distribution, currency, offset, limit, order);
            IConnector connector = context.GetServiceConnector(ServiceKey.HotelsAutocomplete);

            return await connector.GetAsync<CitiesAvailability>(serviceUrl);
            
        }

        //public async Task<CitiesAvailability> GetNearHotelsAvailability(double latitude, double longitude, string checkin, string checkout, string distribution, string currency, int offset , int limit , string sort , string order)
        //{
        //    string serviceUrl = String.Format(ServiceURL.GetServiceURL(ServiceKey.HotelsAvailability), latitude, longitude, checkin, checkout, distribution, currency, offset , limit , sort , order);
        //    IConnector connector = context.GetServiceConnector(ServiceKey.HotelsAutocomplete);

        //    return await connector.GetAsync<CitiesAvailability>(serviceUrl);
            
        //}
    }
}
