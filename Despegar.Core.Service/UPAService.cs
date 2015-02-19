using Despegar.Core.Neo.Connector;
using Despegar.Core.Neo.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Despegar.Core.Neo.Business.Configuration;
using Despegar.Core.Neo.Log;


namespace Despegar.Core.Neo.Service
{
    public class UPAService : IUPAService
    {
        private CoreContext coreContext;

        public UPAService(CoreContext coreContext)
        {
            // TODO: Complete member initialization
            this.coreContext = coreContext;
        }
        public async Task<UpaField> GetUPA(IBugTracker bugtracker)
        {
            string serviceUrl = "t";
            IConnector connector = new UPAConnector(bugtracker);

            return await connector.GetAsync<UpaField>(serviceUrl);

        }
    }
}
