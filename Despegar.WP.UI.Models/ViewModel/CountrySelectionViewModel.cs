using Despegar.Core.Business.Configuration;
using Despegar.Core.IService;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Despegar.WP.UI.Model.ViewModel
{
    public class CountrySelectionViewModel : ViewModelBase
    {
        private IConfigurationService configurationService;
        private INavigator Navigator { get; set; }
        
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

        public CountrySelectionViewModel(INavigator navigator, IConfigurationService configService) 
        {
            configuration = null;
            this.Navigator = navigator;
            this.configurationService = configService;
        }

        public async void LoadConfigurations() 
        {
            Configuration temp = await configurationService.GetConfigurations();

            #if !Decolar
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
