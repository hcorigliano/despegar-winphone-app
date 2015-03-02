using Despegar.Core.Neo.Business.Notifications;
using Despegar.Core.Neo.Contract;
using Despegar.Core.Neo.Contract.API;
using Despegar.Core.Neo.Contract.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Despegar.Core.Neo.API.MAPI
{
    public class MAPINotifications : IMAPINotifications
    {
        private IMapiConnector connector;
        private ICoreContext context;

        public MAPINotifications(ICoreContext context, IMapiConnector connector)
        {
            this.connector = connector;
            this.context = context;
        }

        public async Task<PushResponse> RegisterOnDespegarCloud(PushRegistrationRequest putBody)
        {
            string serviceUrl = ServiceURL.GetServiceURL (ServiceKey.RegisterOnDespegarCloud, String.Empty);

#if DEBUG
            serviceUrl = serviceUrl.Replace("https://mobile.despegar.com/", "http://mobile.despegar.it/");
#endif
            return await connector.PutAsync<PushResponse>(serviceUrl, putBody, ServiceKey.RegisterOnDespegarCloud);
        }
    }
}
