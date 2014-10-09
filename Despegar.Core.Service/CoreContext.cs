﻿using Despegar.Core.Business;
using Despegar.Core.Business.Culture;
using Despegar.Core.Connector;
using Despegar.Core.IService;
using Despegar.Core.Log;
using System;
using System.Collections.Generic;

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
        private Dictionary<ServiceKey, MockKey> appliedMocks = new Dictionary<ServiceKey, MockKey>();
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
        /// Enables a specified Mock for a given Service
        /// </summary>
        /// <param name="serviceKey">The Service Key</param>
        /// <param name="mockKey">The Mock Key</param>
        public void AddMock(ServiceKey serviceKey, MockKey mockKey)
        {
            if (!appliedMocks.ContainsKey(serviceKey))
            {
                appliedMocks.Add(serviceKey, mockKey);
            }
            else
            {
                appliedMocks[serviceKey] = mockKey;
            }

            Logger.LogCore(String.Format("Enabled mock '{0}' for service '{1}'", mockKey.ToString(), serviceKey.ToString()));
        }

        /// <summary>
        /// Disables a specified Mock for a given Service
        /// </summary>
        /// <param name="serviceKey">The Service Key</param>
        public void RemoveMock(ServiceKey serviceKey)
        {
            if (!appliedMocks.ContainsKey(serviceKey))            
                return;

            appliedMocks.Remove(serviceKey);
            Logger.LogCore(String.Format("Disabled mock for '{0}' service ", serviceKey.ToString()));
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
        /// <param name="site">Example: AR,CO,MX etc.</param>
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

        #endregion

        #region ** Core private **

        internal IConnector GetServiceConnector(ServiceKey key)
        {
            if (appliedMocks.ContainsKey(key))
            {
                Logger.LogCore(String.Format("Returning Mock Connector for service '{0}'", key.ToString()));

                string mockedReponse =  Mocks.GetMock(this.appliedMocks[key]);
                return new MockConnector(mockedReponse);
            }

            // Return the real connector
            return mapiConnector;
        }

        #endregion
    
    }
}