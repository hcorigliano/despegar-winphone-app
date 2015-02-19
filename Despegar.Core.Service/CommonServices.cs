using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Despegar.Core.Neo.Connector;
using Despegar.Core.Neo.IService;
using Despegar.Core.Neo.Business.Common.State;
using Despegar.Core.Neo.Business;
using Despegar.Core.Neo.Business.CreditCard;

namespace Despegar.Core.Neo.Service
{
    public class CommonServices : ICommonServices
    {
        private CoreContext context;

        public CommonServices(CoreContext context)
        {
            this.context = context;
        }

        public async Task<List<State>> GetStates(string country)
        {
            string serviceUrl = String.Format(ServiceURL.GetServiceURL(ServiceKey.States), country);
            
            IConnector connector = context.GetServiceConnector(ServiceKey.States);

            return await connector.GetAsync<List<State>>(serviceUrl);
        }

        public async Task<ValidationCreditcards> GetCreditCardValidations()
        {
            string serviceUrl = String.Format(ServiceURL.GetServiceURL(ServiceKey.CreditCardValidation));
            IConnector connector = context.GetServiceConnector(ServiceKey.CreditCardValidation);

            return await connector.GetAsync<ValidationCreditcards>(serviceUrl);
        }
    }
}