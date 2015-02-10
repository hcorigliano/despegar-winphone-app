using Despegar.Core.Neo.Business.Common.State;
using Despegar.Core.Neo.Business.CreditCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.IService
{
    public interface ICommonServices
    {
        Task<List<State>> GetStates(string country);
        Task<ValidationCreditcards> GetCreditCardValidations();
    }
}