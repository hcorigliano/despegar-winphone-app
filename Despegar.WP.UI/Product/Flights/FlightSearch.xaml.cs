using Despegar.Core.Business.Flight.CitiesAutocomplete;
using Despegar.Core.Business.Flight.Itineraries;
using Despegar.WP.UI.Classes;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Model;
using Despegar.WP.UI.Model.Classes.Flights;
using System;
using System.Collections;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Despegar.WP.UI.Product.Flights
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FlightSearch : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private FlightsSearchBoxModel flightSearchBoxModel = new FlightsSearchBoxModel();

        

       

        public FlightSearch()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;           
            
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
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
            if (e.PageState != null)
            {
                if (e.PageState.ContainsKey("originFlight"))
                {
                    airportsContainer.OriginAirportControl.Text = e.PageState["originFlight"].ToString();
                }

                if (e.PageState.ContainsKey("destinyFlight"))
                {
                    airportsContainer.DestinyAirportControl.Text = e.PageState["destinyFlight"].ToString();
                }

            }


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
            e.PageState["originFlight"] = airportsContainer.OriginAirportControl.Text;
            e.PageState["destinyFlight"] = airportsContainer.DestinyAirportControl.Text;

        }

        #region NavigationHelper registration

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

        #endregion
      

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            CityAutocomplete origin = airportsContainer.OriginAirportControl.Items[0] as CityAutocomplete;
            CityAutocomplete destiny = airportsContainer.DestinyAirportControl.Items[0] as CityAutocomplete;
            //TODO: validate the origin and destiny for any problem.
            //if(origin == null || destiny == null)
            //{
            //    // autocomplete not charge properly
            //    throw new NotImplementedException();
            //}


            
            //FlightsItineraries intinerarie = await flightSearchBoxModel.GetItineraries(origin.code, destiny.code, dateControlContainer.DepartureDateControl.Date.ToString("yyyy-MM-dd"), quantityPassagersContainer.Passagers.AdultPassagerQuantity, dateControlContainer.ReturnDateControl.Date.ToString("yyyy-MM-dd"), 0, 0, 0, 10, "", "", "", "");
            
            FlightsItineraries intinerarie = await flightSearchBoxModel.GetItineraries("BUE", "LAX", "2014-11-11", 1, "2014-11-13", 0, 0, 0, 10, "", "", "", "");


            PagesManager.GoTo(typeof(FlightResults), intinerarie);
        }

        

    }
}
                                    