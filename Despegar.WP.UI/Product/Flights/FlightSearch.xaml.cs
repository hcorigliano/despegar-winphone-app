using Despegar.WP.UI.Common;
using Despegar.WP.UI.Model;
using Despegar.WP.UI.Model.ViewModel.Flights;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using Despegar.Core.Business.Flight.SearchBox;
using Despegar.WP.UI.Model.Classes.Flights;
using Despegar.Core.Business.Enums;
using Despegar.WP.UI.Controls;
using Windows.UI.Xaml.Data;
using System.ComponentModel;
using Despegar.WP.UI.Model.Common;
using Windows.UI.Popups;
using Despegar.WP.UI.Model.ViewModel;
using Windows.Phone.UI.Input;
using Windows.ApplicationModel.Resources;
using Despegar.Core.Business.CustomErrors;
using System.Collections.Generic;
using Despegar.WP.UI.BugSense;

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

            ViewModel = new FlightSearchViewModel(Navigator.Instance, GlobalConfiguration.CoreContext.GetFlightService(), BugTracker.Instance);
            ViewModel.ViewModelError += ErrorHandler;
            ViewModel.PropertyChanged += Checkloading;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            this.DataContext = ViewModel;            
        }

        # region ** ERROR HANDLING **
        private async void ErrorHandler(object sender, ViewModelErrorArgs e) 
        {
            BugTracker.Instance.LeaveBreadcrumb("Flight Search Error Raised: " + e.ErrorCode);

            ResourceLoader manager = new ResourceLoader();
            MessageDialog dialog;

            switch(e.ErrorCode) 
            {
                case "SEARCH_FAILED":
                    dialog = new MessageDialog(manager.GetString("Flights_Search_ERROR_SEARCH_FAILED"), manager.GetString("Flights_Search_ERROR_SEARCH_FAILED_TITLE"));
                    await dialog.ShowSafelyAsync();
                    break;
                case "SEARCH_INVALID":
                    dialog = new MessageDialog(manager.GetString("Flights_Search_ERROR_SEARCH_INVALID"), manager.GetString("Flights_Search_ERROR_SEARCH_INVALID_TITLE"));
                    await dialog.ShowSafelyAsync();
                    break;
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
            BugTracker.Instance.LeaveBreadcrumb("Flight Search View");

            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
                  
            if (e.Parameter != null)
            {
                // Navigated from somewhere else
                var parameters = e.Parameter as FlightSearchNavigationData;
                ViewModel.InitializeWith(parameters.SearchModel, parameters.PassengerModel);

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

                // If it is coming from Multiples edit, remove the "Multiples edit" View from the stack
                if (parameters.NavigatedFromMultiples && e.NavigationMode == NavigationMode.New)
                {
                    BugTracker.Instance.LeaveBreadcrumb("Flight Search View - Back from Multiples Edit");
                    ViewModel.Navigator.RemoveBackEntry();
                }

                this.DataContext = ViewModel;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            BugTracker.Instance.LeaveBreadcrumb("Flight Search View - Back button pressed");

            if (ViewModel != null)
            {
                if (ViewModel.IsLoading)
                {
                    e.Handled = true;
                }
                else
                {
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