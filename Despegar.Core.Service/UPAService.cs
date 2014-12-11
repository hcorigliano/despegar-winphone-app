using Despegar.Core.Connector;
using Despegar.Core.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Despegar.Core.Business.Configuration;


namespace Despegar.Core.Service
{
    public class UPAService : IUPAService
    {
        private CoreContext coreContext;

        public UPAService(CoreContext coreContext)
        {
            // TODO: Complete member initialization
            this.coreContext = coreContext;
        }
       public async Task<UpaField> GetUPA()
        {
            string serviceUrl = "t";
            IConnector connector = new UPAConnector();

            return await connector.GetAsync<UpaField>(serviceUrl);

        }
    }
}
