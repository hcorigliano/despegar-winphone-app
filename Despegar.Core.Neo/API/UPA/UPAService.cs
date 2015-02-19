using Despegar.Core.Neo.Business.Configuration;
using Despegar.Core.Neo.Contract.API;
using Despegar.Core.Neo.Contract.Connector;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.API.UPA
{
    public class UPAService : IUPAService
    {
        private IUPAConnector connector;

        public UPAService(IUPAConnector connector)
        {
            this.connector = connector;
        }

        public async Task<UpaField> GetUPA()
        {
            string serviceUrl = ServiceURL.GetServiceURL(ServiceKey.UpaRegister);
            return await connector.GetAsync<UpaField>(serviceUrl, ServiceKey.UpaRegister);
        }
    }
}