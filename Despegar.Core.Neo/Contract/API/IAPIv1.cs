using Despegar.Core.Neo.Business.Common.State;
using Despegar.Core.Neo.Business.CreditCard;
using Despegar.Core.Neo.Business.Hotels.UserReviews.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Contract.API
{
    public interface IAPIv1
    {
        Task<ValidationCreditcards> GetCreditCardValidations();
        Task<HotelUserReviewsV1> GetHotelUserReviews(string hotelId, bool cleanEmpty, int page, int pageSize, bool BringTotal);
    }
}