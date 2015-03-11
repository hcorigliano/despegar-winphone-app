using Despegar.Core.Neo.Business.Hotels.UserReviews;
using Despegar.Core.Neo.Contract;
using Despegar.Core.Neo.Contract.API;
using Despegar.Core.Neo.Contract.Connector;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.API.V3
{
    internal class APIv3 : IAPIv3
    {
        private ICoreContext context;
        private IApiv3Connector connector;

        public APIv3(ICoreContext context, IApiv3Connector connector)
        {
            this.context = context;
            this.connector = connector;
        }

        public async Task<HotelUserReviews> GetHotelUserReviews(string hotelId, int limit, int offset, string language, string provider)
        {
            string serviceUrl = ServiceURL.GetServiceURL(ServiceKey.HotelUserReview, hotelId, limit, offset, language,provider);

            return await connector.GetAsync<HotelUserReviews>(serviceUrl, ServiceKey.HotelUserReview);
        }
    }
}