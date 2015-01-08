﻿using Despegar.Core.Business;
using Despegar.Core.IService;
using Despegar.Core.Log;
using Despegar.Core.Service;
using Windows.Storage;
using System;
using System.Threading.Tasks;


namespace Despegar.WP.UI.Model
{
    /// <summary>
    /// Windows Phone 8 App, Global Config class
    /// </summary>
    public static class GlobalConfiguration
    {
        public static ICoreContext CoreContext { get; set; }
        public static string Site { get { return CoreContext.GetSite(); } }
        public static string Language { get { return CoreContext.GetLanguage();} }
        public static string UPAId { get; set; }

        /// <summary>
        /// Initializes the CoreContext object and configures it
        /// This method should be called on app Init
        /// </summary>
        public async static Task InitCore(IBugTracker bugtracker) 
        {
            //Set vars with client info
            ClientDeviceInfo ClientInfo = new ClientDeviceInfo();
            string xclient = ClientInfo.GetClientInfo();
            string uow = ClientInfo.GetUOW();
                       
            CoreContext = new CoreContext();
            CoreContext.SetBugTracker(bugtracker);
            CoreContext.Configure(xclient, uow);
            await LoadUPA(bugtracker);

            // Enable Verbose logging
#if DEBUG
             Logger.Configure(true, true);
#endif


#if DECOLAR
            CoreContext.SetSite("BR");
#endif

            // Enable Service Mocks
            CoreContext.EnableMock(MockKey.CountriesDefault);

            // Service Error testing mocks
            //CoreContext.EnableMock(MockKey.ForceUpdateErroneus);
            //CoreContext.EnableMock(MockKey.ConfigurationErroneus);
        }

        /// <summary>
        /// Changes the Site configuration for the Context
        /// </summary>
        public static void ChangeSite(string siteCode) 
        {
            CoreContext.SetSite(siteCode);       
        }

        public static async Task LoadUPA(IBugTracker bugtracker)
        {
            IUPAService upaService = CoreContext.GetUpaService();

            var roamingSettings = ApplicationData.Current.RoamingSettings;
            if (roamingSettings.Values["UPA"] == null)
            {
                var field = await upaService.GetUPA(bugtracker);

                if (field != null)
                {
                    UPAId = field.id;
                }
                else
                {
                    UPAId = null;
                }

                roamingSettings.Values["UPA"] = UPAId;

            }
            else
            {
                UPAId = roamingSettings.Values["UPA"].ToString();
            }

        }

    }
}