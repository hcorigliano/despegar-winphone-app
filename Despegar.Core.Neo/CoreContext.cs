using Despegar.Core.Neo.Business;
using Despegar.Core.Neo.Connector;
using Despegar.Core.Neo.Log;
using System.Linq;
using System;
using System.Collections.Generic;
using Despegar.Core.Neo.Business.Configuration;
using Despegar.Core.Neo.API;
using Despegar.Core.Neo.Contract;
using Despegar.Core.Neo.Contract.API;
using Despegar.Core.Neo.Contract.Log;
using Autofac;
using Despegar.Core.Neo.Contract.Connector;
using Despegar.Core.Neo.InversionOfControl;

namespace Despegar.Core.Neo
{
    /// <summary>
    /// Core class for the Despegar.Core.Neo
    /// It is the main object that the "library" exposes to user code.
    /// There should only exists one CoreContext object in memory for a given application.
    /// </summary>
    public class CoreContext : ICoreContext
    {
        /// <summary>
        /// Contains the list of services to mock when they are called
        /// </summary>
        private List<Mock> appliedMocks = new List<Mock>();
        private string site;
        private string x_client;
        private string uow;
        private IBugTracker bugtracker;
        private ICoreLogger logger;
        private Configuration configuration;
        
        #region ** Public Interface **
        
        public CoreContext(IBugTracker bugTracker, ICoreLogger logger) 
        {
            this.bugtracker = bugTracker;
            this.logger = logger;

            logger.Log("[Core]: Core context created.");
        }

        public string GetSite()
        {                       
            return site;
        }

        public string GetLanguage(bool isDecolar)
        {
            return isDecolar ? "pt" : "es";            
        }

        public string GetUOW()
        {
            //return Despegar.WP.UI.Model. ClientDeviceInfoGetUOW();
            return uow;
        }

        public Configuration GetConfiguration()
        {
            return configuration;
        }

        /// <summary>
        /// Enables a specified Mock
        /// </summary>        
        /// <param name="mockKey">The Mock Key</param>
        public void EnableMock(string mockKey)
        {
            if (!appliedMocks.Any(x => x.MockName == mockKey))
            {
                appliedMocks.Add(Mock.GetMock(mockKey));
            }

            logger.Log(String.Format("[Core:Mocks]: Enabled mock '{0}'", mockKey.ToString()));
        }

        /// <summary>
       /// Disables a specific Mock
       /// </summary>
        /// <param name="mockKey"></param>
        public void DisableMock(string mockKey)
        {
            if (!appliedMocks.Any( x => x.MockName == mockKey))
                return;

            appliedMocks.Remove(Mock.GetMock(mockKey));
            logger.Log(String.Format("[Core:Mocks]: Disabled mock '{0}' ", mockKey.ToString()));
        }

        /// <summary>
        /// Indicates whether a Mock is Enabled or not
        /// </summary>
        /// <param name="mockKey">The Mock Key</param>
        /// <returns>A boolean indicating the Mock status</returns>
        public bool IsMockEnabled(string mockKey)
        {
            return appliedMocks.Any(x => x.MockName == mockKey);
        }

        /// <summary>
        /// Configures the Core Context. This method should be call on Core Initialization
        /// </summary>
        /// <param name="x_client"></param>
        /// <param name="uow"></param>
        public void Configure(string x_client, string uow) 
        {            
            this.x_client = x_client;
            this.uow = uow;
            IoC.Resolve<IMapiConnector>().ConfigureClientAndUow(this.x_client, this.uow);
            IoC.Resolve<IApiv3Connector>().ConfigureClientAndUow(this.x_client, this.uow);
            logger.Log("[Core]: Core Uow and X_Client configured.");
        }

        /// <summary>
        /// Reconfigures the Core for the new Site
        /// </summary>
        /// <param name="code">Example: AR,CO,MX etc.</param>
        /// <param name="name">Name of country. Example: Argentina , Mexico , Colombia , etc.</param>
        /// 
        public void SetSite(string siteCode)
        {
            logger.Log("[Core]: Changing Site...");

            string lang = "es";

            if (siteCode.ToLowerInvariant() == "br")
                lang = "pt";

            this.site = siteCode;
            IoC.Resolve<IMapiConnector>().ConfigureSiteAndLanguage(site, lang);
            IoC.Resolve<IApiv1Connector>().Configure(x_client, uow, site);
            IoC.Resolve<IApiv3Connector>().ConfigureSiteAndLanguage(site, lang);

            logger.Log("[Core]: Site Changed.");
        }

        public void SetConfiguration(Configuration conf)
        {
            configuration = conf;
        }

        #endregion

        #region ** Core internal **

        public string GetMockedResponse(ServiceKey key)
        {
            // Mocked service?
            var mock = appliedMocks.FirstOrDefault(x => x.ServiceID == key);
            if (mock != null)
            {
                logger.Log(String.Format("[CORE:Mocks]: Returning Mock for service '{0}'", key.ToString()));
                return mock.Content;
            }

            // No Mock
            return null;
        }

        #endregion
    }
}