using Despegar.Core.Business.Configuration;
using Despegar.Core.IService;
using Despegar.LegacyCore;
using Despegar.LegacyCore.Connector;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Models.Classes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Despegar.WP.UI.Model
{
    public class HomeViewModel : ViewModelBase
    {
        private IConfigurationService configurationService;
        private INavigator Navigator;

        public HomeViewModel(INavigator navigator)             
        {
            this.Navigator = navigator;
            configurationService = GlobalConfiguration.CoreContext.GetConfigurationService();
        }

        public List<Configuration> Configurations { get; set; }

        public async Task<Configuration> GetConfigurations()
        {
            return (await configurationService.GetConfigurations());
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