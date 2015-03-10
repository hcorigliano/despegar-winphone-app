using Despegar.Core.Neo.API;
using Despegar.Core.Neo.Business.CreditCard;
using Despegar.Core.Neo.Contract;
using Despegar.Core.Neo.Contract.API;
using Despegar.Core.Neo.Contract.Connector;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Service
{
    internal class APIv1 : IAPIv1
    {
        private ICoreContext context;
        private IApiv1Connector connector;

        public APIv1(ICoreContext context, IApiv1Connector connector)
        {
            this.context = context;
            this.connector = connector;
        }
       
        public async Task<ValidationCreditcards> GetCreditCardValidations()
        {            
            string serviceUrl = ServiceURL.GetServiceURL(ServiceKey.CreditCardValidation);

            return await connector.GetAsync<ValidationCreditcards>(serviceUrl, ServiceKey.CreditCardValidation);
        }


        public async Task<Business.Hotels.UserReviews.V1.HotelUserReviewsV1> GetHotelUserReviews(string hotelId, bool cleanEmpty, int page, int pageSize, bool BringTotal)
        {
            string serviceUrl = ServiceURL.GetServiceURL(ServiceKey.HotelUserReviewV1, hotelId, cleanEmpty, page, pageSize, BringTotal);
            return await connector.GetAsync<Business.Hotels.UserReviews.V1.HotelUserReviewsV1>(serviceUrl, ServiceKey.HotelUserReviewV1);
        }
    }
}