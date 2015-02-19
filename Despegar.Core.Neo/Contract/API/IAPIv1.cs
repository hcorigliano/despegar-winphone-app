using Despegar.Core.Neo.Business.Common.State;
using Despegar.Core.Neo.Business.CreditCard;
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
    }
}