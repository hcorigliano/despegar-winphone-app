using Despegar.Core.Business;
using Despegar.Core.IService;
using Despegar.Core.Log;
using Despegar.Core.Service;

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
   
        /// <summary>
        /// Initializes the CoreContext object and configures it
        /// This method should be called on app Init
        /// </summary>
        public static void InitCore() 
        {            
            // TODO: Set Site and Language
            CoreContext = new CoreContext();
            CoreContext.Configure("WindowsPhone8App", "wp8-uow");

            //TODO : (1)
            //CoreContext.SetSite(SiteCode.Argentina);


            // Enable Verbose logging
            #if DEBUG
             Logger.Configure(true, true);
            #endif

            // Add Service Mocks
            CoreContext.EnableMock(MockKey.ConfigurationsDefault); //keep on! No URL yet.
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
    }
}
