using Despegar.WP.UI.Common;
using Despegar.WP.UI.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Despegar.Core.Business.Flight.Itineraries;
using Despegar.WP.UI.Model;
using Despegar.WP.UI.Model.Classes.Flights;


// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Despegar.WP.UI.Product.Flights
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FlightDetail : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private FlightsSearchBoxModel flightSearchBoxModel = new FlightsSearchBoxModel();
        private FlightDetailsModel flightDetailModelInbound = new FlightDetailsModel();
        private FlightDetailsModel flightDetailModelOutbound = new FlightDetailsModel();
        private FlightsItineraries intinerarie;

        public FlightDetail()
        {
            this.InitializeComponent();            

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            //FillDataMocked();
        }

        private async void FillDataMocked()
        {
            //BORRAR ESTO
            intinerarie = await flightSearchBoxModel.GetItineraries("BUE", "LAX", "2014-11-11", 1, "2014-11-13", 0, 0, 0, 10, "", "", "", "");

            flightDetailModelInbound.inbound = intinerarie.items[0].inbound[0];
            flightDetailModelOutbound.outbound = intinerarie.items[0].outbound[0];

            SegmentControlInbound.DataContext = flightDetailModelInbound;
            SegmentControlOutbound.DataContext = flightDetailModelOutbound;
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
            RoutesItems routes = e.NavigationParameter as RoutesItems;
            if (routes != null)
            {
                flightDetailModelInbound.inbound = routes.inbound;
                flightDetailModelOutbound.outbound = routes.outbound;

                SegmentControlInbound.DataContext = flightDetailModelInbound;
                SegmentControlOutbound.DataContext = flightDetailModelOutbound;
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

        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            PagesManager.GoTo(typeof(FlightCheckout),null);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            PagesManager.GoBack();
        }
    }
}
