using Despegar.Core.IService;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Model;
using Despegar.WP.UI.Model.ViewModel.Flights;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace Despegar.WP.UI.Product.Flights
{
    public sealed partial class FlightSearch : Page
    {
        private NavigationHelper navigationHelper;
        private IFlightService flightService;
        public FlightSearchViewModel ViewModel { get; set; }
       
        public FlightSearch()
        {
            this.InitializeComponent();
            
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            this.flightService = GlobalConfiguration.CoreContext.GetFlightService();
            ViewModel = new FlightSearchViewModel(Navigator.Instance, flightService);
            this.DataContext = ViewModel;
            this.CheckDeveloperTools();

            // TODO: default it in ViewModel
            ViewModel.AddSegmentMultipleCommand.Execute(null);
            ViewModel.AddSegmentMultipleCommand.Execute(null);            
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // Restore values stored in session state.
            //if (e.PageState != null)
            //{
            //    if (e.PageState.ContainsKey("originFlight"))
            //    {
            //        airportsContainer.OriginAirportControl.Text = e.PageState["originFlight"].ToString();
            //    }

            //    if (e.PageState.ContainsKey("destinyFlight"))
            //    {
            //        airportsContainer.DestinyAirportControl.Text = e.PageState["destinyFlight"].ToString();
            //    }

            //    if (e.PageState.ContainsKey("originFlight"))
            //    {
            //        airportsContainer.AirportOrigin = e.PageState["originFlightCode"].ToString();
            //    }

            //    if (e.PageState.ContainsKey("destinyFlight"))
            //    {
            //        airportsContainer.AirportDestiny = e.PageState["destinyFlightCode"].ToString();
            //    }

            //    if (e.PageState.ContainsKey("FlightDateDeparture"))
            //    {
            //        dateControlContainer.DepartureDateControl.Date = DateTime.Parse(e.PageState["FlightDateDeparture"].ToString());
            //    }

            //    if (e.PageState.ContainsKey("FlightDateReturn"))
            //    {
            //        dateControlContainer.ReturnDateControl.Date = DateTime.Parse(e.PageState["FlightDateReturn"].ToString());
            //    }
            //}
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            //e.PageState["originFlight"] = airportsContainer.OriginAirportControl.Text;
            //e.PageState["destinyFlight"] = airportsContainer.DestinyAirportControl.Text;
            //e.PageState["originFlightCode"] = airportsContainer.AirportOrigin;
            //e.PageState["destinyFlightCode"] = airportsContainer.AirportDestiny;

            //e.PageState["FlightDateDeparture"] = dateControlContainer.DepartureDateControl.Date;
            //e.PageState["FlightDateReturn"] = dateControlContainer.ReturnDateControl.Date;

        }

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

    }
}