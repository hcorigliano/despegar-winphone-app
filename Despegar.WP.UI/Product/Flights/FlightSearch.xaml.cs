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

namespace Despegar.WP.UI.Product.Flights
{
    public sealed partial class FlightSearch : Page
    {
        private NavigationHelper navigationHelper;
        public FlightSearchViewModel ViewModel { get; set; }
       
        public FlightSearch()
        {
            this.InitializeComponent();
            
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            this.CheckDeveloperTools();

            ViewModel = new FlightSearchViewModel(Navigator.Instance, GlobalConfiguration.CoreContext.GetFlightService());
            this.DataContext = ViewModel;
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
            //e.PageState["originFlight"] = airportsContainer.OriginAirportControl.Text;
            //e.PageState["destinyFlight"] = airportsContainer.DestinyAirportControl.Text;
            //e.PageState["originFlightCode"] = airportsContainer.AirportOrigin;
            //e.PageState["destinyFlightCode"] = airportsContainer.AirportDestiny;

            //e.PageState["FlightDateDeparture"] = dateControlContainer.DepartureDateControl.Date;
            //e.PageState["FlightDateReturn"] = dateControlContainer.ReturnDateControl.Date;

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
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
                if (parameters.NavigatedFromMultiples)
                    ViewModel.Navigator.RemoveBackEntry();

                this.DataContext = ViewModel;
            }                        
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //this.navigationHelper.OnNavigatedFrom(e);           
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