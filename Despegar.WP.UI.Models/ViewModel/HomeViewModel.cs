using Despegar.Core.Business.Configuration;
using Despegar.Core.IService;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Model.ViewModel;
using Despegar.WP.UI.Models.Classes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using Despegar.WP.UI.Model.ViewModel.Classes;
using Despegar.Core.Log;

namespace Despegar.WP.UI.Model
{
    public class HomeViewModel : ViewModelBase
    {
        private IConfigurationService configurationService;
        private INavigator Navigator;

        public HomeViewModel(INavigator navigator, IConfigurationService configuracion, HomeParameters parameters, IBugTracker t) : base(t)
        {
            this.Navigator = navigator;
            this.configurationService = configuracion;

            if (parameters != null)
            {
                if (parameters.ClearStack)
                    navigator.ClearStack();
            }
        }

        public async Task<List<Product>> GetProducts(string country)
        {
            IsLoading = true;

            Configuration configuration;

            try
            {
                configuration = await configurationService.GetConfigurations();
            }
            catch (Exception)
            {
                IsLoading = false;
                return null;
            }

            if (configuration != null)
            {
                GlobalConfiguration.CoreContext.SetConfiguration(configuration);
                var Site = configuration.sites.FirstOrDefault(s => s.code == country);

                IsLoading = false;
                return Site.products;

            }
            else
            {

                IsLoading = false;
                return null;
            }
        }

        public ICommand NavigateToHotelsLegacy
        {
            get
            {
                return new RelayCommand(() => { return; }); 
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
    }
}