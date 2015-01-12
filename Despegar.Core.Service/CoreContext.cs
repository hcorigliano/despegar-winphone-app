using Despegar.Core.Business;
using Despegar.Core.Connector;
using Despegar.Core.IService;
using Despegar.Core.Log;
using System.Linq;
using System;
using System.Collections.Generic;
using Windows.Storage;
using Despegar.Core.Business.Configuration;

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
        private List<ServiceKey> v1Services = new List<ServiceKey>() { ServiceKey.CreditCardValidation };
        private string site;
        private string x_client;
        private string uow;
        private IBugTracker bugtracker;
        private Configuration configuration;

        // Connectors
        private MapiConnector mapiConnector;
        private Apiv1Connector apiv1Connector;
        

        #region ** Public Interface **

        public string GetSite()
        {                       
            return site;
        }

        public string GetLanguage() 
        { 
            return IsDECOLAR ? "PT" : "ES"; 
        }

        public string GetUOW()
        {
            //return Despegar.WP.UI.Model. ClientDeviceInfoGetUOW();
            return uow;
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
                mapiConnector = new MapiConnector(bugtracker);
            mapiConnector.ConfigureClientAndUow(x_client, this.uow);

            if (apiv1Connector == null)
                apiv1Connector = new Apiv1Connector(bugtracker);
            
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
            mapiConnector.ConfigureSiteAndLanguage(site, GetLanguage());
            apiv1Connector.Configure(x_client, uow, site);

            

            Logger.LogCore("Site Changed.");
        }

        public IHotelService GetHotelService()
        {
            return new HotelService(this);
        }

        public IFlightService GetFlightService() 
        {
            return new FlightService(this);
        }

        public IConfigurationService GetConfigurationService()
        {
            return new ConfigurationService(this);
        }

        public IUPAService GetUpaService()
        {
            return new UPAService(this);
        }

        public ICommonServices GetCommonService()
        {
            return new CommonServices(this);
        }

        public ICouponsService GetCouponsService()
        {
            return new CouponService(this);
        }


        public Configuration GetConfiguration()
        {
            return configuration;
        }


        public void SetConfiguration(Configuration conf)
        {
            configuration = conf;
        }
        #endregion

        #region ** Core private **

        internal IConnector GetServiceConnector(ServiceKey key)
        {
            // Mocked service?
            var mock = appliedMocks.FirstOrDefault(x=> x.ServiceID == key);
            if (mock != null)
            {
                Logger.LogCore(String.Format("Returning Mock Connector for service '{0}'", key.ToString()));
                return new MockConnector(mock.Content);
            }

            if (v1Services.Any(x => x == key))
                return apiv1Connector;

            return mapiConnector;
        }

        #endregion



        public void SetBugTracker(IBugTracker bugtracker)
        {
            this.bugtracker = bugtracker;
        }
    }
}