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
using Despegar.WP.UI.Classes;
using Despegar.WP.UI.Model;
using Windows.UI.Popups;
using System.Threading.Tasks;
using Despegar.WP.UI.Model.Classes;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Despegar.WP.UI.Product.Flights
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FlightResults : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private FlightResultsModel flightResultModel = new FlightResultsModel();
        private FlightSearchModel flightSearchModel = new FlightSearchModel();

        public FlightResults()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            this.DataContext = flightResultModel;
            this.miniboxSearch.DataContext = flightSearchModel;
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
            PageParameters pageParameters = e.NavigationParameter as PageParameters;

            flightResultModel.Itineraries = pageParameters.Itineraries as FlightsItineraries;

            flightSearchModel = pageParameters.SearchModel as FlightSearchModel;

            this.miniboxSearch.DataContext = flightSearchModel;

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

        public T FindDescendant<T>(DependencyObject obj) where T : DependencyObject
        {
            // Check if this object is the specified type
            if (obj is T)
                return obj as T;

            // Check for children
            int childrenCount = VisualTreeHelper.GetChildrenCount(obj);
            if (childrenCount < 1)
                return null;

            // First check all the children
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child is T)
                    return child as T;
            }

            // Then check the childrens children
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = FindDescendant<T>(VisualTreeHelper.GetChild(obj, i));
                if (child != null && child is T)
                    return child as T;
            }

            return null;
        }

        private DependencyObject FindChildControl<T>(DependencyObject control, string ctrlName)
        {
            int childNumber = VisualTreeHelper.GetChildrenCount(control);
            for (int i = 0; i < childNumber; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(control, i);
                FrameworkElement fe = child as FrameworkElement;
                // Not a framework element or is null
                if (fe == null) return null;

                if (child is T && fe.Name == ctrlName)
                {
                    // Found the control so return
                    return child;
                }
                else
                {
                    // Not found it - search children
                    DependencyObject nextLevel = FindChildControl<T>(child, ctrlName);
                    if (nextLevel != null)
                        return nextLevel;
                }
            }
            return null;
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Grid grid = sender as Grid;
            if (grid!=null)
            {
                PagesManager.GoTo(typeof(FlightDetail), grid.DataContext);
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem currentSelectedListBoxItem;
            if (lbFlights.SelectedIndex == -1)
                return;

            //currentSelectedListBoxItem = this.lbFlights.ItemContainerGenerator.ContainerFromIndex(lbFlights.SelectedIndex) as ListBoxItem;

            currentSelectedListBoxItem = this.lbFlights.ContainerFromIndex(lbFlights.SelectedIndex) as ListBoxItem;

            if (currentSelectedListBoxItem == null)
                return;

            // Iterate whole listbox tree and search for this items
            ItemsControl itemsControl = FindDescendant<ItemsControl>(currentSelectedListBoxItem);

            if (itemsControl == null) 
                return;
            itemsControl.Visibility = SetVisualEffect(itemsControl.Visibility);

        }

        private void ListBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ListBox listbox = sender as ListBox;
            ListBoxItem listboxitem;
            int index = 0;

            if (listbox == null)
                return;
            
            index = listbox.SelectedIndex;

            if (index == -1) return;

            listboxitem = listbox.ContainerFromIndex(index) as ListBoxItem;
            if (listboxitem == null) 
                return;

            //ItemsControl itemsControl = FindDescendant<ItemsControl>(listboxitem);
            ItemsControl itemsControl = FindChildControl<ItemsControl>(listboxitem, "RoutesItemControl") as ItemsControl;

            if (itemsControl == null) 
                return;

            itemsControl.Visibility = SetVisualEffect(itemsControl.Visibility);
        }

        private Visibility SetVisualEffect(Visibility visibility)
        {
            switch (visibility){
                case  Visibility.Collapsed:
                    return Visibility.Visible;
                case Visibility.Visible:
                    return Visibility.Collapsed;
            }
            return visibility;
        }


        public void GoBack() 
        {
            if (navigationHelper.CanGoBack())
            {
                navigationHelper.GoBack();
            }
        }

        private void miniboxSearch_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GoBack();
        }
    }
}
