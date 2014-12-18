using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Despegar.Core.Connector;
using Despegar.Core.IService;
using Despegar.Core.Business.Common.State;
using Despegar.Core.Business;

namespace Despegar.Core.Service
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
    }
}
