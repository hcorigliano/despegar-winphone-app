using Despegar.Core.Connector;
using Despegar.Core.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace Despegar.Core.Service
{
    public class CoreContext : ICoreContext
    {
        /// <summary>
        /// Contains the list of services to mock when they are called
        /// </summary>
        private Dictionary<ServiceKey, MockKey> enabledMocks = new Dictionary<ServiceKey,MockKey>();

        /// <summary>
        /// Enables a specified Mock for a given Service
        /// </summary>
        /// <param name="serviceKey">The Service Key</param>
        /// <param name="mockKey">The Mock Key</param>
        public void AddMock(ServiceKey serviceKey, MockKey mockKey)
        {
            if (!enabledMocks.ContainsKey(serviceKey))
            {
                enabledMocks.Add(serviceKey, mockKey);
            }
            else
            {
                enabledMocks[serviceKey] = mockKey;
            }
        }

        /// <summary>
        /// Disables a specified Mock for a given Service
        /// </summary>
        /// <param name="serviceKey">The Service Key</param>
        public void RemoveMock(ServiceKey serviceKey)
        {
            if (!enabledMocks.ContainsKey(serviceKey))            
                return;

            enabledMocks.Remove(serviceKey);
        }

        /// <summary>
        /// Configures the Core Context. This method should be call on Core Initialization
        /// </summary>
        /// <param name="x_client"></param>
        /// <param name="uow"></param>
        /// <param name="site"></param>
        /// <param name="language"></param>
        public void Configure(string x_client, string uow, string site, string language) 
        {
            MapiConnector.Configure(x_client, uow, site, language);
        }

        public IFlightService GetFlightService() 
        {
            return new FlightService(this);
        }

        internal IConnector GetServiceConnector(ServiceKey key) 
        {
            if (enabledMocks.ContainsKey(key))
            {
                string mockedReponse =  Mocks.GetMock(this.enabledMocks[key]);
                return new MockConnector(mockedReponse);
            }

            // Return the real connector
            return MapiConnector.GetInstance();
        }
    }
}