using Despegar.Core.Neo.Business.Flight;
using Despegar.Core.Neo.Business.Flight.Itineraries;
using Despegar.Core.Neo.Business.Flight.SearchBox;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Model;
using Despegar.WP.UI.Model.Classes;
using Despegar.WP.UI.Model.ViewModel.Flights;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Collections.Generic;
using Despegar.WP.UI.Model.ViewModel.Classes.Flights;
using Despegar.WP.UI.Model.Classes.Flights;
using Windows.UI.Xaml.Media.Imaging;
using Despegar.WP.UI.BugSense;
using Despegar.Core.Neo.Business.Flight.SearchBox;
using Despegar.Core.Neo.InversionOfControl;
using Despegar.Core.Neo.Business;
using Despegar.Core.Neo.Business.Enums;

namespace Despegar.WP.UI.Product.Flights
{
    public sealed partial class FlightResults : Page
    {
        private FlightResultsViewModel ViewModel;        

        public FlightResults()
        {
            this.InitializeComponent();
            this.CheckDeveloperTools();
            //Google Analytics
#if !DEBUG
                GoogleAnalyticContainer ga = new GoogleAnalyticContainer();
                ga.Tracker = GoogleAnalytics.EasyTracker.GetTracker();
                ga.SendView("FlightResults");
#endif
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (e.Parameter != null)
            {
                // TODO: Improve
                if (e.Parameter.GetType() == typeof(List<Facet>) || e.Parameter.GetType() == typeof(Sorting))
                    ViewModel.FlightSearchModel.SearchStatus = SearchStates.SearchAgain;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = IoC.Resolve<FlightResultsViewModel>();
            ViewModel.OnNavigated(e.Parameter);
            this.DataContext = ViewModel;
            this.miniboxSearch.DataContext = ViewModel.FlightSearchModel;            
        }

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
            Button button = sender as Button;
            if (button != null)
            {
                // TODO: Encapsular esto
                ViewModel.FlightCrossParameters.Inbound = ((RoutesItems)button.DataContext).inbound;
                ViewModel.FlightCrossParameters.Outbound = ((RoutesItems)button.DataContext).outbound;
                ViewModel.FlightCrossParameters.price = ((RoutesItems)button.DataContext).price;

                ViewModel.Navigator.GoTo(Model.Interfaces.ViewModelPages.FlightsDetails, ViewModel.FlightCrossParameters);                
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem currentSelectedListBoxItem;
            if (lbFlights.SelectedIndex == -1)
                return;

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
            ListView listview = sender as ListView;
            ListViewItem listviewitem;
            int index = 0;

            if (listview == null)
                return;

            index = listview.SelectedIndex;

            if (index == -1) return;

            listviewitem = listview.ContainerFromIndex(index) as ListViewItem;
            if (listviewitem == null)
                return;

            //ItemsControl itemsControl = FindDescendant<ItemsControl>(listboxitem);
            ItemsControl itemsControl = FindChildControl<ItemsControl>(listviewitem, "RoutesItemControl") as ItemsControl;

            if (itemsControl == null)
                return;

            ViewModel.FlightCrossParameters.FlightId = ((BindableItem)listview.SelectedItem).id;

            var list = itemsControl.DataContext as BindableItem;
            //itemsControl.DataContext.RoutesCustom
            if (list != null && ViewModel.FlightSearchModel.PageMode == FlightSearchPages.Multiple)
            {
                ViewModel.FlightCrossParameters.MultipleRoutes = list.RoutesCustom;
            }
            else { ViewModel.FlightCrossParameters.MultipleRoutes = null; }

            itemsControl.Visibility = SetVisualEffect(itemsControl.Visibility);
        }

        private Visibility SetVisualEffect(Visibility visibility)
        {
            switch (visibility)
            {
                case Visibility.Collapsed:
                    return Visibility.Visible;
                case Visibility.Visible:
                    return Visibility.Collapsed;
            }
            return visibility;
        }

        private void miniboxSearch_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.BugTracker.LeaveBreadcrumb("Flight Result Minibox Hit");
            ViewModel.Navigator.GoBack();
        }

        private void appBarFilter_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Navigator.GoTo(Model.Interfaces.ViewModelPages.FlightsFilters, null);
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            ((Image)sender).Source = new BitmapImage(new Uri("ms-appx:/Assets/Icon/Airlines/ag_default@2x.png", UriKind.Absolute));
        }
    }
}