using Despegar.Core.Business.Configuration;
using Despegar.Core.IService;
using Despegar.LegacyCore;
using Despegar.LegacyCore.Connector;
using Despegar.LegacyCore.ViewModel;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Model;
using Despegar.WP.UI.Product.Legacy;
using Despegar.WP.UI.Strings;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Despegar.WP.UI.Developer;
using Windows.UI.Xaml;
using Windows.UI.Popups;
using Despegar.WP.UI.Controls;
using System.ComponentModel;
using Despegar.WP.UI.Model.ViewModel;

namespace Despegar.WP.UI
{    
    public sealed partial class Home : Page
    {
        private ModalPopup loadingPopup = new ModalPopup(new Loading());
        private NavigationHelper navigationHelper;
        public List<Despegar.Core.Business.Configuration.Product> products;
        public Despegar.WP.UI.Model.HomeViewModel ViewModel { get; set; }

        public Home()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            ViewModel = new Despegar.WP.UI.Model.HomeViewModel(Navigator.Instance, GlobalConfiguration.CoreContext.GetConfigurationService());            
            ViewModel.PropertyChanged += Checkloading;

            this.DataContext = ViewModel;

            // Developer Tools
            this.CheckDeveloperTools();
            SetupMenuItems(GlobalConfiguration.Site);
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

        private async void SetupMenuItems(string country)
        {            
            products = await ViewModel.GetProducts(country);
            

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
                    ViewModel.NavigateToHotelsLegacy.Execute(AppResources.GetLegacyString("HomeProductHotelsUrl"));
                    break;
                case "Flights":
                    ViewModel.NavigateToFlights.Execute(null);
                    break;
                //case "MyDespegar":
                //    
                //    break;
                default:
                    var dialog = new MessageDialog("Proximamente estará disponible esta funcionalidad.", "Proximamente");
                    dialog.ShowAsync();
                    break;
            }
        }
        
    }
}