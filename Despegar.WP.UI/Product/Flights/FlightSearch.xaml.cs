using Despegar.Core.Neo.Business.CustomErrors;
using Despegar.Core.Neo.Business.Enums;
using Despegar.Core.Neo.InversionOfControl;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Controls;
using Despegar.WP.UI.Model.Classes.Flights;
using Despegar.WP.UI.Model.Common;
using Despegar.WP.UI.Model.ViewModel;
using Despegar.WP.UI.Model.ViewModel.Flights;
using System.ComponentModel;
using Windows.ApplicationModel.Resources;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Despegar.WP.UI.Product.Flights
{
    public sealed partial class FlightSearch : Page
    {
        private ModalPopup loadingPopup = new ModalPopup(new Loading());
        public FlightSearchViewModel ViewModel { get; set; }        

        public FlightSearch()
        {
            this.InitializeComponent();
            this.CheckDeveloperTools();
        }

        # region ** ERROR HANDLING **
        private async void ErrorHandler(object sender, ViewModelErrorArgs e) 
        {
            ResourceLoader manager = new ResourceLoader();
            MessageDialog dialog;

            switch(e.ErrorCode) 
            {               
                case "SEARCH_INVALID_WITH_MESSAGE":
                    CustomError message = e.Parameter as CustomError;
                    if (message == null) break;

                    string msg = manager.GetString(message.Code);
                    
                    if (message.HasDates)
                    {
                        string msgunformated = msg;
                        msg = string.Format(msgunformated, message.Date);
                    }

                    dialog = new MessageDialog(msg, manager.GetString("Flights_Search_ERROR_SEARCH_INVALID_TITLE"));
                    await dialog.ShowSafelyAsync();
                    break;
            }
        }
        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                HardwareButtons.BackPressed += HardwareButtons_BackPressed;

                ViewModel = IoC.Resolve<FlightSearchViewModel>();
                ViewModel.ViewModelError += ErrorHandler;
                ViewModel.PropertyChanged += Checkloading;
                ViewModel.OnNavigated(e.Parameter);
                this.DataContext = ViewModel;


                if (e.Parameter != null)
                {
                    var parameters = e.Parameter as FlightSearchNavigationData;

                    // Set Current Pivot Item
                    switch (parameters.SearchModel.PageMode)
                    {
                        case FlightSearchPages.RoundTrip:
                            MainPivotControl.SelectedIndex = 0;
                            break;
                        case FlightSearchPages.OneWay:
                            MainPivotControl.SelectedIndex = 1;
                            break;
                        case FlightSearchPages.Multiple:
                            MainPivotControl.SelectedIndex = 2;
                            break;
                    }
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (ViewModel != null)
            {
                if (ViewModel.IsLoading)
                {
                    e.Handled = true;
                }
                else
                {
                    ViewModel.BugTracker.LeaveBreadcrumb("Flight search View - Back button pressed");
                    ViewModel.Navigator.GoBack();
                    e.Handled = true;
                }
            }
        }

        private void Checkloading(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsLoading")
            {
                if ((sender as ViewModelBase).IsLoading)
                    loadingPopup.Show();
                else
                    loadingPopup.Hide();
            }
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch(((Pivot)sender).SelectedIndex)
            {
                case 0:
                    ViewModel.SetSearchMode(FlightSearchPages.RoundTrip);
                    this.BottomAppBar.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    ViewModel.SetSearchMode(FlightSearchPages.OneWay);
                    this.BottomAppBar.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    ViewModel.SetSearchMode(FlightSearchPages.Multiple);
                    this.BottomAppBar.Visibility = Visibility.Visible;
                    break;
            }
        }                                
    }
}