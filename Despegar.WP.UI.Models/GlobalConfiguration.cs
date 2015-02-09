using Autofac;
using Despegar.Core.Neo.API;
using Despegar.Core.Neo.Business.Configuration;
using Despegar.Core.Neo.Contract;
using Despegar.Core.Neo.Contract.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Despegar.Core.Neo.InversionOfControl;
using Despegar.WP.UI.Model.InversionOfControl;

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
        private static string upadId;
        public static string UPAId
        {
            get { return upadId != null ? upadId : CoreContext.GetUOW(); }
            set { upadId = value; }
        }

        /// <summary>
        /// Initializes the CoreContext object and configures it
        /// This method should be called on app Init
        /// </summary>
        public async static Task InitCore(IEnumerable<CoreModule> modules) 
        {
            //Set vars with client info
            ClientDeviceInfo ClientInfo = new ClientDeviceInfo();
            string xclient = ClientInfo.GetClientInfo();
            string uow = ClientInfo.GetUOW();

            // Load Core and App IoC registrations / Build IoC Container
            bool isQaBuild = false;
            Module iocModule = new ViewModelModule(isQaBuild);
            var mods = new List<Module>() { iocModule };
            mods.AddRange(modules);
            Despegar.Core.Neo.InversionOfControl.IoC.LoadModules(false, mods);

            // Configure Core
            CoreContext = Despegar.Core.Neo.InversionOfControl.IoC.Resolve<ICoreContext>();
            CoreContext.Configure(xclient, uow);

            await LoadUPA();

#if DECOLAR
            CoreContext.SetSite("BR");
#endif

            // Enable Service Mocks
            CoreContext.EnableMock(MockKey.CountriesDefault);

            // Service Error testing mocks
            //CoreContext.EnableMock(MockKey.ForceUpdateErroneus);
            //CoreContext.EnableMock(MockKey.ConfigurationErroneus);
        }

        private static Product GetProductParameterFromConfiguration(string producto)
        {
            Configuration conf = GlobalConfiguration.CoreContext.GetConfiguration();
            string site = GlobalConfiguration.Site;

            var site2return = conf.sites.Where(s => s.code == site).FirstOrDefault();
            if (site2return == null)
                return null;

            var _s = site2return.products.Where(p => p.name == producto).FirstOrDefault();
            if (_s == null)
                return null;

            return _s;
        }

        public static int GetEmissionAnticipationDayForFlights()
        {
            return GetProductParameterFromConfiguration("flights").emission_anticipation_days;
        }

        public static int GetLastAvailableHoursForFlights()
        {
            int last;
            try
            {
                last = Convert.ToInt32(GetProductParameterFromConfiguration("flights").last_available_hour);
            }
            catch (Exception)
            {
                last = 0;
            }
            return last;
        }

        public static int GetEmissionAnticipationDayForHotels()
        {
            return GetProductParameterFromConfiguration("hotels").emission_anticipation_days;
        }

        public static int GetLastAvailableHoursForHotels()
        {
            int last;
            try
            {
                last = Convert.ToInt32(GetProductParameterFromConfiguration("hotels").last_available_hour);
            }
            catch (Exception)
            {
                last = 0;
            }
            return last;
        }

        private static async Task LoadUPA()
        {
            var roamingSettings = ApplicationData.Current.RoamingSettings;

            if (roamingSettings.Values["UPA"] == null)
            {
                // Adquire UPA ID
                IUPAService upaService = Despegar.Core.Neo.InversionOfControl.IoC.Resolve<IUPAService>();
                UpaField field = await upaService.GetUPA();

                UPAId = field != null ?  field.id : null;
              
                // Save UPA in Mobile Device
                roamingSettings.Values["UPA"] = UPAId;
            }
            else
            {
                // Load UPA from Moible Device
                UPAId = roamingSettings.Values["UPA"].ToString();
            }

        }

    }
}