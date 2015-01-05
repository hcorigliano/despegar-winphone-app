using Despegar.WP.UI.Common;
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
using Despegar.WP.UI.Model.ViewModel.Classes.Flights;
using Despegar.WP.UI.BugSense;

namespace Despegar.WP.UI.Product.Flights
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FlightDetail : Page
    {
        private NavigationHelper navigationHelper;
        public FlightDetailsViewModel ViewModel { get; set; }

        public FlightDetail()
        {
            this.InitializeComponent();            

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
            this.CheckDeveloperTools();


            #if !DEBUG
                GoogleAnalyticContainer ga = new GoogleAnalyticContainer();
                ga.Tracker = GoogleAnalytics.EasyTracker.GetTracker();
                ga.SendView("FlightDetail");
            #endif
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
            BugTracker.Instance.LeaveBreadcrumb("Flight Detail View");

            FlightsCrossParameter routes = e.NavigationParameter as FlightsCrossParameter;
            if (routes != null)
            {
                // Multiples are inserted as an Outbound collection of Routes
                ViewModel = new FlightDetailsViewModel(Navigator.Instance, routes, BugTracker.Instance);
            }

            // Check Search type
            if (!ViewModel.IsTwoWaySearch) 
            {
                // Remove "Return" pivot item
                MainPivot.Items.RemoveAt(2);
            }


            if (ViewModel.CrossParameters.MultipleRoutes != null)
            {
                // Remove "Go" pivot item (Only multiple)
                MainPivot.Items.RemoveAt(1);
            }
            else
                MainPivot.Items.RemoveAt(0);

            DataContext = ViewModel;
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
             
    }
}