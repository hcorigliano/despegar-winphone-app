using Despegar.Core.IService;
using Despegar.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Despegar.Core.Business.Culture;
using Despegar.Core.Business;
using Despegar.Core.Log;

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
            GlobalConfiguration.CoreContext.AddMock(ServiceKey.Configurations, MockKey.ConfigurationsDefault); //keep on! No URL yet.
            //CoreContext.AddMock(ServiceKey.FlightsAirlines, MockKey.AirlineTest);
            GlobalConfiguration.CoreContext.AddMock(ServiceKey.FlightCitiesAutocomplete, MockKey.FlightCitiesAutocompleteBue);
            GlobalConfiguration.CoreContext.AddMock(ServiceKey.FlightItineraries, MockKey.ItinerarieBueToLax);
            GlobalConfiguration.CoreContext.AddMock(ServiceKey.FlightsBookingFields, MockKey.BookingFieldBuetoMia);
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
