using Despegar.WP.UI.Common;
using Despegar.WP.UI.Controls;
using Despegar.WP.UI.Model;
using Despegar.WP.UI.Model.ViewModel;
using Despegar.WP.UI.Model.ViewModel.Classes;
using Despegar.WP.UI.Strings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Despegar.WP.UI
{    
    public sealed partial class Home : Page
    {
        private ModalPopup loadingPopup = new ModalPopup(new Loading());
        private NavigationHelper navigationHelper;
        public List<Despegar.Core.Business.Configuration.Product> products;
        public Despegar.WP.UI.Model.HomeViewModel ViewModel { get; set; }
        private IAsyncOperation<IUICommand> asyncCommand = null;


        public Home()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
            

            // Developer Tools
            this.CheckDeveloperTools();  
            
#if DECOLAR
            // Remove Country Selection from BAR for DECOLAR
            CommandBar bar = BottomAppBar as CommandBar;
            bar.PrimaryCommands.RemoveAt(0);
#endif
         
            //Google Analytics
            #if !DEBUG
            GoogleAnalyticContainer ga = new GoogleAnalyticContainer();
            ga.Tracker = GoogleAnalytics.EasyTracker.GetTracker();
            ga.SendView("Home");
            #endif
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

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        // TODO: Refactor. Use DATABINDINGS!!
        private async void SetupMenuItems(string country)
        {
            products = await ViewModel.GetProducts(country);

            // Failed to get configurations
            if (products == null) 
            {
                ResourceLoader manager = new ResourceLoader();
                MessageDialog dialog = new MessageDialog(manager.GetString("Page_Home_Configuration_Error"), "Error");
                await dialog.ShowAsync();
                Application.Current.Exit();
                return;
            }

            foreach (var product in products)
            {
                switch(product.name)
                {

                    case "hotels":
                        Hotels.Visibility = (product.status == "ENABLED") ? Visibility.Visible : Visibility.Collapsed;
                        break;
                    case "flights":
                        Flights.Visibility = (product.status == "ENABLED") ? Visibility.Visible : Visibility.Collapsed;
                        break;
                }
            }
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
            var parameter = e.NavigationParameter as HomeParameters;

            ViewModel = new Despegar.WP.UI.Model.HomeViewModel(Navigator.Instance, GlobalConfiguration.CoreContext.GetConfigurationService(), parameter);
            ViewModel.PropertyChanged += Checkloading;
            SetupMenuItems(GlobalConfiguration.Site);

            this.DataContext = ViewModel;
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

        private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            string selectedButton = (string)((e.ClickedItem as Grid).DataContext);

            switch (selectedButton)
            {
                case "Hotels":
                    ViewModel.NavigateToHotelsLegacy.Execute(AppResources.GetLegacyString("HomeProductHotelsUrl"));
                    break;
                case "Flights":
                    ViewModel.NavigateToFlights.Execute(null);
                    break;
                //case "MyDespegar":
                    //var f= Window.Current.Content as Frame;
                    //f.Navigate(typeof(FlightCheckout), null);
                    //break;
                default:

#if DECOLAR
                    var dialog = new MessageDialog("Esta funcionalidade estará disponível em breve.", "Em Breve");
#else
                    var dialog = new MessageDialog("Proximamente estará disponible esta funcionalidad.", "Proximamente");
#endif
                    if (asyncCommand != null)
                    {
                        asyncCommand.Cancel();
                    }
              
                        asyncCommand = dialog.ShowAsync();
                    
                    break;
            }
        }
        
    }
}