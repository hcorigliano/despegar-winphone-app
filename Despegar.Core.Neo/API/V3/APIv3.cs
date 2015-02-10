using Despegar.Core.Neo.Contract;
using Despegar.Core.Neo.Contract.API;
using Despegar.Core.Neo.Contract.Connector;

namespace Despegar.Core.Neo.API.V3
{
    internal class APIv3 : IAPIv3
    {
        private ICoreContext context;
        private IApiv1Connector connector;

        public APIv3(ICoreContext context, IApiv1Connector connector)
        {
            this.context = context;
            this.connector = connector;
        }       
    }
}