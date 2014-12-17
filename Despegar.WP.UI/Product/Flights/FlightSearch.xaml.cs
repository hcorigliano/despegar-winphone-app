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

namespace Despegar.WP.UI.Product.Flights
{
    public sealed partial class FlightSearch : Page
    {
        private NavigationHelper navigationHelper;
        private ModalPopup loadingPopup = new ModalPopup(new Loading());
        public FlightSearchViewModel ViewModel { get; set; }        

        public FlightSearch()
        {
            this.InitializeComponent();
            
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            this.CheckDeveloperTools();

            ViewModel = new FlightSearchViewModel(Navigator.Instance, GlobalConfiguration.CoreContext.GetFlightService());
            ViewModel.ViewModelError += ErrorHandler;
            ViewModel.PropertyChanged += Checkloading;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            this.DataContext = ViewModel;            
        }

        # region ** ERROR HANDLING **
        private void ErrorHandler(object sender, ViewModelErrorArgs e) 
        {
            ResourceLoader manager = new ResourceLoader();
            MessageDialog dialog;

            switch(e.ErrorCode) 
            {
                case "SEARCH_FAILED":
                    dialog = new MessageDialog(manager.GetString("Flights_Search_ERROR_SEARCH_FAILED"), manager.GetString("Flights_Search_ERROR_SEARCH_FAILED_TITLE"));
                    dialog.ShowAsync();
                    break;
                case "SEARCH_INVALID":
                    dialog = new MessageDialog(manager.GetString("Flights_Search_ERROR_SEARCH_INVALID"), manager.GetString("Flights_Search_ERROR_SEARCH_INVALID_TITLE"));
                    dialog.ShowAsync();
                    break;
                default:
                    //List<CustomError> message = e.Parameter as List<CustomError>;
                    CustomError message = e.Parameter as CustomError;
                    if (message == null) break;
                    dialog = new MessageDialog(message.Message, manager.GetString("Flights_Search_ERROR_SEARCH_INVALID_TITLE"));
                    dialog.ShowAsync();
                    break;
            }
        }
        #endregion

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

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
           
        }

        // TODO: what to do with this
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            //this.navigationHelper.OnNavigatedTo(e);            
            if (e.Parameter != null)
            {
                // Navigated from somewhere else
                var parameters = e.Parameter as FlightSearchNavigationData;
                ViewModel.InitializeWith(parameters.SearchModel, parameters.PassengerModel);

                // Set Current Pivor Item
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
                    ViewModel.Navigator.RemoveBackEntry();

                this.DataContext = ViewModel;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            //this.navigationHelper.OnNavigatedFrom(e);           
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (ViewModel != null)
            {
                if (ViewModel.IsLoading)
                {
                    e.Handled = true;
                }
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