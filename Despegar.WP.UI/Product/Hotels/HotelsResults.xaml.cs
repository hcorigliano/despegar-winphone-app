using Despegar.Core.Neo.Business.Hotels.CitiesAvailability;
using Despegar.Core.Neo.InversionOfControl;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Controls;
using Despegar.WP.UI.Model.Common;
using Despegar.WP.UI.Model.ViewModel;
using Despegar.WP.UI.Model.ViewModel.Hotels;
using Windows.ApplicationModel.Resources;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace Despegar.WP.UI.Product.Hotels
{
    public sealed partial class HotelsResults : Page
    {
        private ModalPopup loadingPopup = new ModalPopup(new Loading());
        private HotelsResultsViewModel ViewModel { get; set; }

        public HotelsResults()
        {
            this.InitializeComponent();
        }

        # region ** ERROR HANDLING **
        private async void ErrorHandler(object sender, ViewModelErrorArgs e)
        {
            ViewModel.BugTracker.LeaveBreadcrumb("Hotels results Error raised - " + e.ErrorCode);

            ResourceLoader manager = new ResourceLoader();
            MessageDialog dialog;

            switch (e.ErrorCode)
            {
                case "SEARCH_ERROR":
                    int errorID = (int)e.Parameter;                    
                    switch (errorID) 
                    { 
                        case 2380:
                            dialog = new MessageDialog(manager.GetString("Hotels_Search_ERROR_CHECKIN_INVALID"), "Error");
                            break;
                        case 2399:
                            dialog = new MessageDialog(manager.GetString("Hotels_Search_ERROR_MAX_DAYS_LIMIT"), "Error");
                            break;
                        default:
                            dialog = new MessageDialog(manager.GetString("Hotels_Search_ERROR"), "Error");
                            break;
                    }
                    
                    await dialog.ShowSafelyAsync();                   
                    break;
                case "SEARCH_NO_RESULTS":
                    dialog = new MessageDialog(manager.GetString("Hotels_Search_ERROR_SEARCH_NO_RESULTS"), "Error");
                    await dialog.ShowSafelyAsync();    
                    break;
                case "UNKNOWN_ERROR":
                    dialog = new MessageDialog(manager.GetString("Hotels_Search_UNKNOWN_ERROR"), "Error");
                    await dialog.ShowSafelyAsync();
                    break;
            }

            // Back to search box
            ViewModel.Navigator.GoBack();
        }
        #endregion

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            BottomAppBar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            if (e.NavigationMode == NavigationMode.New)
            {                
                    ViewModel = IoC.Resolve<HotelsResultsViewModel>();
                    ViewModel.PropertyChanged += Checkloading;
                    ViewModel.ViewModelError += ErrorHandler;
                    ViewModel.OnNavigated(e.Parameter);                    
                    await ViewModel.LoadResults();
                    this.DataContext = ViewModel;
                    //((HotelsResultsViewModel)this.DataContext).SearchModel.NotifyPropertiesChanged();        
            }
            BottomAppBar.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            if (!ViewModel.IsLoading)
            {                
                ViewModel.Navigator.GoBack();
            }
        }

        private void ReSearchTapped(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.Navigator.GoBack();
        }

        private void Checkloading(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsLoading")
            {
                if ((sender as ViewModelBase).IsLoading)
                    loadingPopup.Show();
                else
                    loadingPopup.Hide();
            }
        }

        private void HotelSelected(object sender, TappedRoutedEventArgs e)
        {
            ListView listView = ((ListView)sender);
            
            if (ViewModel != null)
            {
                ViewModel.GoToDetails((HotelItem)listView.SelectedItem, listView.SelectedIndex);
            }
        }

    }
}