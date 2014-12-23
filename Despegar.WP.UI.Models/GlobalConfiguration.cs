using Despegar.Core.Business;
using Despegar.Core.IService;
using Despegar.Core.Log;
using Despegar.Core.Service;
using Despegar.Core.Business.Configuration;


namespace Despegar.WP.UI.Model
{
    /// <summary>
    /// Windows Phone 8 App, Global Config class
    /// </summary>
    public static class GlobalConfiguration
    {
        public static ICoreContext CoreContext { get; set; }
        //private IUPAService upaService;
        public static string Site { get { return CoreContext.GetSite(); } }
        public static string Language { get { return CoreContext.GetLanguage();} }
        public static string UPAId { get; set; }

        /// <summary>
        /// Initializes the CoreContext object and configures it
        /// This method should be called on app Init
        /// </summary>
        public static void InitCore() 
        {
            //Set vars with client info
            ClientDeviceInfo ClientInfo = new ClientDeviceInfo();
            string xclient = ClientInfo.GetClientInfo();
            string uow = ClientInfo.GetUOW();
           
            // TODO: Set Site and Language
            CoreContext = new CoreContext();
            CoreContext.Configure(xclient, uow);

            LoadUPA();

            //TODO : (1)
            //CoreContext.SetSite(SiteCode.Argentina);

            // Enable Verbose logging
            #if DEBUG
             Logger.Configure(true, true);
            #endif

             
#if DECOLAR
            CoreContext.SetSite("BR");
#endif

             // Add Service Mocks
            //CoreContext.EnableMock(MockKey.ConfigurationsDefault); //keep on! No URL yet.
            //CoreContext.AddMock(MockKey.AirlineTest);
            //GlobalConfiguration.CoreContext.AddMock(MockKey.FlightCitiesAutocompleteBue);
            //GlobalConfiguration.CoreContext.AddMock(MockKey.ItinerarieBueToLax);
            //CoreContext.EnableMock(MockKey.BookingFieldBuetoMia);
            //CoreContext.EnableMock(MockKey.BookingFieldsBueLaxChildInfant);
            CoreContext.EnableMock(MockKey.CountriesDefault);
           
        }

        /// <summary>
        /// Changes the Site configuration for the Context
        /// </summary>
        public static void ChangeSite(string siteCode) 
        {
            CoreContext.SetSite(siteCode);       
        }

        public static async void LoadUPA()
        {
            IUPAService upaService = CoreContext.GetUpaService();
            var field = await upaService.GetUPA();

            if (field != null)
                UPAId = field.id;
            else
                UPAId = null;
        }

    }
}
