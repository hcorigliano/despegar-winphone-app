using Despegar.Core.Neo.Business.Configuration;
using Despegar.Core.Neo.Contract.API;
using Despegar.Core.Neo.Contract.Log;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Model.ViewModel;
using Despegar.WP.UI.Model.ViewModel.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Despegar.WP.UI.Model
{
    public class HomeViewModel : ViewModelBase
    {
        private IMAPICross mapiService;

        public HomeViewModel(INavigator navigator, IMAPICross mapiService, IBugTracker t) : base(navigator, t)
        {
            this.Navigator = navigator;
            this.mapiService = mapiService;           
        }

        public override void OnNavigated(object navigationParams)
        {
            this.BugTracker.LeaveBreadcrumb("Home View");
            HomeParameters parameters = navigationParams as HomeParameters;

            if (parameters != null)
            {
                if (parameters.ClearStack)
                     Navigator.ClearStack();
             }
        }

        public async Task<bool> ValidateAppVersion(string appVersion)
        {            
            BugTracker.LeaveBreadcrumb("Validate App Version");   

            try
            {
                UpdateFields data = await mapiService.CheckUpdate(appVersion, "8.1", "X", "X");

                BugTracker.LeaveBreadcrumb("Update Service call succesful");

#if DEBUG
                return false; 
#else
                return data.force_update;
#endif

            }
            catch (Exception)
            {
                OnViewModelError("VALIDATE_APP_ERROR");
                return false;
            }            
        }

        public async Task<List<Product>> GetProducts(string country)
        {
            IsLoading = true;

            Configuration configuration;

            try
            {
                configuration = await mapiService.GetConfigurations();
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

        public ICommand NavigateToHotels
        {
            get
            {
                return new RelayCommand(() => Navigator.GoTo(ViewModelPages.HotelsSearch, null)); 
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