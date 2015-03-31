using Despegar.Core.Neo.Business.Enums;
using Despegar.Core.Neo.InversionOfControl;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Controls;
using Despegar.WP.UI.Model.Classes.Flights;
using Despegar.WP.UI.Model.Common;
using Despegar.WP.UI.Model.ViewModel;
using Despegar.WP.UI.Model.ViewModel.Classes;
using Despegar.WP.UI.Model.ViewModel.Flights;
using System;
using System.ComponentModel;
using System.Net.NetworkInformation;
using Windows.ApplicationModel.Resources;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Despegar.WP.UI.Product.Flights
{
    public sealed partial class FlightResults : Page
    {
        private ModalPopup loadingPopup = new ModalPopup(new Loading());
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

        # region ** ERROR HANDLING **
        private async void ErrorHandler(object sender, ViewModelErrorArgs e)
        {
            ResourceLoader manager = new ResourceLoader();
            MessageDialog dialog;

            switch (e.ErrorCode)
            {
                case "LOAD_RESULTS_NO_ITEMS":
                    dialog = new MessageDialog(manager.GetString("Flight_results_ERROR_NO_ITEMS"), "Error");
                    await dialog.ShowSafelyAsync();
                    break;
                case "LOAD_RESULTS_FAILED":
                    if (NetworkInterface.GetIsNetworkAvailable())
                    {
                        dialog = new MessageDialog(manager.GetString("Flights_Search_ERROR_SEARCH_FAILED"), manager.GetString("Flights_Search_ERROR_SEARCH_FAILED_TITLE"));
                    } else {
                        // Internet error
                        dialog = new MessageDialog(manager.GetString("Generic_ERROR_SEARCH_FAILED_NO_INTERNET"), manager.GetString("Generic_ERROR_SEARCH_FAILED_NO_INTERNET_TITLE"));
                    }

                    await dialog.ShowSafelyAsync();
                    break;   
            }

            // Return to SearchBox
            ViewModel.Navigator.GoBack();
        }
        #endregion

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            var param = e.Parameter as GenericResultNavigationData;
            BottomAppBar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            if (e.NavigationMode == NavigationMode.New)
            {
                // First Search
                ViewModel = IoC.Resolve<FlightResultsViewModel>();
                ViewModel.ViewModelError += ErrorHandler;
                ViewModel.PropertyChanged += Checkloading;
                ViewModel.OnNavigated(e.Parameter);
                ViewModel.ResetPagination();
                await ViewModel.LoadResults();
                
                this.DataContext = ViewModel;
            }

            // not working
            BottomAppBar.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (ViewModel != null)
            {
                if (ViewModel.IsLoading)
                {
                    e.Handled = true;
                }
                else
                {
                    ViewModel.BugTracker.LeaveBreadcrumb("Flight search Results - Back button pressed");
                    ViewModel.Navigator.GoBack();                    
                }
            }

            e.Handled = true;
        }

        private T FindDescendant<T>(DependencyObject obj) where T : DependencyObject
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
                var routeItem =  (RoutesItems)button.DataContext;

                ViewModel.FlightCrossParameters.Inbound = routeItem.inbound;
                ViewModel.FlightCrossParameters.Outbound = routeItem.outbound;
                ViewModel.FlightCrossParameters.price = routeItem.price;


                //ViewModel.Itineraries.items.IndexOf(routeItem);

                // UPA Tracking
                ViewModel.FlightCrossParameters.UPA_SelectedItemIndex = lbFlights.SelectedIndex;

                ViewModel.Navigator.GoTo(Model.Interfaces.ViewModelPages.FlightsDetails, ViewModel.FlightCrossParameters);                
            }

            e.Handled = true;
        }

        private void ListBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ListView listview = sender as ListView;
            int index = 0;

            if (listview == null)
                return;

            index = listview.SelectedIndex;

            if (index == -1) 
                return;

            ListViewItem listviewitem = listview.ContainerFromIndex(index) as ListViewItem;
            if (listviewitem == null)
                return;

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
            else 
            {
                ViewModel.FlightCrossParameters.MultipleRoutes = null; 
            }


            if (itemsControl.Visibility == Visibility.Visible) 
            {
                itemsControl.Visibility = Visibility.Collapsed;
                return;
            }

            // Close all
            int counter = 0;
            foreach (var i in listview.Items)
            {
                ListViewItem container = (ListViewItem)listview.ContainerFromIndex(counter);
                if (container != null)
                {
                    ItemsControl control = FindChildControl<ItemsControl>(container, "RoutesItemControl") as ItemsControl;
                    if (control != null)                   
                        control.Visibility = Visibility.Collapsed;                                            
                }

                counter++;
            }
                
            // Open selected
            itemsControl.Visibility = itemsControl.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        private void miniboxSearch_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.MiniboxSearch();
        }
        
        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            ((Image)sender).Source = new BitmapImage(new Uri("ms-appx:/Assets/Icon/Airlines/ag_default@2x.png", UriKind.Absolute));
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
    }
}