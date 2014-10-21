﻿using Despegar.LegacyCore;
using Despegar.LegacyCore.ViewModel;
using Despegar.WP.UI.Classes;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Product.Legacy;
using Despegar.WP.UI.Strings;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Despegar.WP.UI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Home : Page
    {        
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();                

        public Home()
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
            //TODO cast element for calling the correct instances of object
            TextBlock text = e.ClickedItem as TextBlock;

            switch (text.Name) 
            {
                case "FlightsOption": 
                    PagesManager.GoTo(typeof(Product.Flights.FlightSearch),e);
                    break;
                case "HotelsOption":
                    LoadBrowser(AppResources.GetLegacyString("HomeProductHotelsUrl"), e);
                    break;
                default:
                    throw new InvalidOperationException();
            }

            //delete the following lines
            //AppEmbbed.PivotPage pp = new PivotPage();
            //PagesManager.GoTo(typeof(), e);
        }

        private void LoadBrowser(string relativePath, ItemClickEventArgs e)
        {
            ApplicationConfig.Instance.ResetBrowsingPages(new Uri(HomeViewModel.Domain + relativePath));
            //ApplicationConfig.Instance.ResetBrowsingPages(new Uri("http://m.despegar.com.ar/hoteles/detail/297146/2014-10-31/2014-11-01/2/ARS"));
            PagesManager.GoTo(typeof(Browser), e);
        }
    }
}