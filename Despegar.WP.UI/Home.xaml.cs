using Despegar.LegacyCore;
using Despegar.LegacyCore.Connector;
using Despegar.LegacyCore.ViewModel;
using Despegar.WP.UI.Classes;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Model;
using Despegar.WP.UI.Product.Legacy;
using Despegar.WP.UI.Strings;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Despegar.WP.UI.Developer;
using Windows.UI.Xaml;
// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Despegar.WP.UI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Home : Page
    {
        private NavigationHelper navigationHelper;
        public HomeViewModel ViewModel; 

        public Home()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
            

            //this.ViewModel = new HomeViewModel();
            //this.DataContext = ViewModel;

            // Developer Tools
            this.CheckDeveloperTools();
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

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            string selectedButton = (string)((e.ClickedItem as Grid).DataContext);

            switch (selectedButton)
            {
                case "Hotels":
                    LoadBrowser(AppResources.GetLegacyString("HomeProductHotelsUrl"), e);
                    break;
                case "Flights":
                    PagesManager.GoTo(typeof(Product.Flights.FlightSearch), e);
                    break;                
                default:
                    throw new InvalidOperationException();
            }
        }

        private void LoadBrowser(string relativePath, ItemClickEventArgs e)
        {
            APIConnector.Instance.Channel = ApplicationConfig.Instance.Country = GlobalConfiguration.Site;  // TODO: Legacy code
            ApplicationConfig.Instance.ResetBrowsingPages(new Uri(Despegar.LegacyCore.ViewModel.HomeViewModel.Domain + relativePath));

            PagesManager.GoTo(typeof(Browser), e);
        }

        private void AppBarButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            PagesManager.GoTo(typeof(CountrySelection), e);
        }
    }
}