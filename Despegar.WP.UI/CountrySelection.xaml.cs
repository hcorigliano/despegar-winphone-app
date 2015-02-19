using Despegar.Core.Neo.Business.Configuration;
using Despegar.Core.Neo.InversionOfControl;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Model.ViewModel;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Despegar.WP.UI
{
    public sealed partial class CountrySelection : Page
    {
        public CountrySelectionViewModel ViewModel { get; set; }

        public CountrySelection()
        {
            this.InitializeComponent();           
            this.CheckDeveloperTools();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            ViewModel = IoC.Resolve<CountrySelectionViewModel>();
            DataContext = ViewModel;

            ViewModel.OnNavigated(null);
            ViewModel.LoadConfigurations();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //persist data in phone
            Site countrySelected = e.ClickedItem as Site;

            // Persist selection
            var roamingSettings = ApplicationData.Current.RoamingSettings;
            roamingSettings.Values["countryCode"] = countrySelected.code;
            roamingSettings.Values["countryName"] = countrySelected.name; // not used?
            roamingSettings.Values["countryLanguage"] = countrySelected.language; // not used?
                    
            ViewModel.ChangeCountry(countrySelected);
            ViewModel.NavigateToHome.Execute(null);
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            ViewModel.Navigator.GoBack();
        }
    }
}