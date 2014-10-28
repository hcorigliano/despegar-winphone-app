using Despegar.Core.Business;
using Despegar.Core.Business.Culture;
using Despegar.Core.Connector;
using Despegar.Core.IService;
using Despegar.Core.Log;
using System.Linq;
using System;
using System.Collections.Generic;
using Windows.Storage;

namespace Despegar.Core.Service
{
    /// <summary>
    /// Core class for the Despegar.Core
    /// It is the main object that the "library" exposes to user code.
    /// There should only exists one CoreContext object in memory for a given application.
    /// </summary>
    public class CoreContext : ICoreContext
    {
#if DECOLAR
        public static bool IsDECOLAR = true;
#else
        public static bool IsDECOLAR = false;
#endif

        /// <summary>
        /// Contains the list of services to mock when they are called
        /// </summary>
        private List<Mock> appliedMocks = new List<Mock>();
        private string site;
        private string x_client;
        private string uow;

        // Connectors
        private MapiConnector mapiConnector;

        #region ** Public Interface **

        public string GetSite()
        {                       
            return site;
        }

        public string GetLanguage() 
        { 
            return IsDECOLAR ? "PT" : "ES"; 
        }

        /// <summary>
        /// Enables a specified Mock
        /// </summary>        
        /// <param name="mockKey">The Mock Key</param>
        public void EnableMock(MockKey mockKey)
        {
            if (!appliedMocks.Any(x => x.MockID == mockKey))
            {
                appliedMocks.Add(Mock.GetMock(mockKey));
            }

            Logger.LogCore(String.Format("Enabled mock '{0}'", mockKey.ToString()));
        }

        /// <summary>
       /// Disables a specific Mock
       /// </summary>
        /// <param name="mockKey"></param>
        public void DisableMock(MockKey mockKey)
        {
            if (!appliedMocks.Any( x => x.MockID == mockKey))
                return;

            appliedMocks.Remove(Mock.GetMock(mockKey));
            Logger.LogCore(String.Format("Disabled mock '{0}' ", mockKey.ToString()));
        }

        /// <summary>
        /// Indicates whether a Mock is Enabled or not
        /// </summary>
        /// <param name="mockKey">The Mock Key</param>
        /// <returns>A boolean indicating the Mock status</returns>
        public bool IsMockEnabled(MockKey mockKey) {
            return appliedMocks.Any(x => x.MockID == mockKey);
        }

        /// <summary>
        /// Configures the Core Context. This method should be call on Core Initialization
        /// </summary>
        /// <param name="x_client"></param>
        /// <param name="uow"></param>
        public void Configure(string x_client, string uow) 
        {
            Logger.LogCore("Initializing Core...");

            this.x_client = x_client;
            this.uow = uow;

            // Init Connectors
            if (mapiConnector == null)
                mapiConnector = new MapiConnector();

            Logger.LogCore("Core Initialized.");
        }

        /// <summary>
        /// Reconfigures the Core for the new Site
        /// </summary>
        /// <param name="code">Example: AR,CO,MX etc.</param>
        /// <param name="name">Name of country. Example: Argentina , Mexico , Colombia , etc.</param>
        /// 
        public void SetSite(string siteCode)
        {
            Logger.LogCore("Changing Site...");

            this.site = siteCode;         
            mapiConnector.Configure(x_client, uow, site, GetLanguage());

            Logger.LogCore("Site Changed.");
        }

        public IFlightService GetFlightService() 
        {
            return new FlightService(this);
        }

        public IConfigurationService GetConfigurationService()
        {
            return new ConfigurationService(this);
        }

        #endregion

        #region ** Core private **

        internal IConnector GetServiceConnector(ServiceKey key)
        {
            var mock = appliedMocks.FirstOrDefault(x=> x.ServiceID == key);
            if (mock != null)
            {
                Logger.LogCore(String.Format("Returning Mock Connector for service '{0}'", key.ToString()));
                return new MockConnector(mock.Content);
            }

            // Return the real connector
            return mapiConnector;
        }

        #endregion
    
    }
}