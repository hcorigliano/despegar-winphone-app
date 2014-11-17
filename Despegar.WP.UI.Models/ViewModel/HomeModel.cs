using Despegar.Core.Business.Configuration;
using Despegar.Core.IService;
using Despegar.LegacyCore;
using Despegar.LegacyCore.Connector;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Model.ViewModel;
using Despegar.WP.UI.Models.Classes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;

namespace Despegar.WP.UI.Model
{
    public class HomeViewModel : ViewModelBase
    {
        private IConfigurationService configurationService;
        private INavigator Navigator;

        public HomeViewModel(INavigator navigator, IConfigurationService configuracion)             
        {
            this.Navigator = navigator;
            this.configurationService = configuracion;
        }

        public async Task<List<Product>> GetProducts(string country)
        {
            Configuration configuration = await configurationService.GetConfigurations();
            var Site = configuration.sites.FirstOrDefault(s => s.code == country);
            return Site.products;
        }

        public ICommand NavigateToHotelsLegacy
        {
            get
            {
                return new RelayCommand<string>((legacyPath) => LoadBrowser(legacyPath)); 
            }
        }

        public ICommand NavigateToFlights 
         {
            get
            { 
                return new RelayCommand(() => Navigator.GoTo(ViewModelPages.FlightsSearch, null)); 
            }
        }

        public ICommand NavigateToCountrySelection
        {
            get
            {
                return new RelayCommand(() => Navigator.GoTo(ViewModelPages.CountrySelecton, null));
            }
        }  
      
        private void LoadBrowser(string relativePath)
        {
            APIConnector.Instance.Channel = ApplicationConfig.Instance.Country = GlobalConfiguration.Site;  // TODO: Legacy code
            ApplicationConfig.Instance.ResetBrowsingPages(new Uri(Despegar.LegacyCore.ViewModel.HomeViewModel.Domain + relativePath));

            Navigator.GoTo(ViewModelPages.LegacyBrowser, null);            
        }

    }
}