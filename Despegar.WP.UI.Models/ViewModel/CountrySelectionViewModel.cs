using Despegar.Core.Neo.Business.Configuration;
using Despegar.Core.Neo.Contract.API;
using Despegar.Core.Neo.Contract.Log;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Models.Classes;
using System.Linq;
using System.Windows.Input;

namespace Despegar.WP.UI.Model.ViewModel
{
    public class CountrySelectionViewModel : ViewModelBase
    {
        private IMAPICross mapiService;
        
        private Configuration configuration;
        public Configuration Configurations
        {
            get { return configuration; }
            set
            {
                configuration = value;
                OnPropertyChanged();
            }
        }

        public CountrySelectionViewModel(INavigator navigator, IMAPICross mapiService, IBugTracker t) : base(navigator, t) 
        {
            this.Navigator = navigator;
            this.mapiService = mapiService;
        }

        public override void OnNavigated(object navigationParams)
        {
        }

        public async void LoadConfigurations() 
        {
            Configuration temp = await mapiService.GetConfigurations();

            // Remove Brasil in Despegar app
            #if !DECOLAR
                Site site = temp.sites.FirstOrDefault(x => x.code == "BR");
                if(site != null)
                    temp.sites.Remove(site);
            #endif

            Configurations = temp;
        }

        public ICommand NavigateToHome
        {
            get
            {
                return new RelayCommand(() => Navigator.GoTo(ViewModelPages.Home, null));
            }
        }

        public void ChangeCountry(Site countrySelected)
        {
            GlobalConfiguration.CoreContext.SetSite(countrySelected.code);
        }

    }
}