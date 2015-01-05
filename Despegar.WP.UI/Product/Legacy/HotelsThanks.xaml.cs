using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using Windows.UI.Xaml.Controls;
using Despegar.LegacyCore.ViewModel;
using Windows.UI.Xaml;
using Despegar.WP.UI;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.BugSense;
using Despegar.WP.UI.Model;

namespace Despegar.WP.UI.Product.Legacy
{
    public partial class HotelsThanks : Page
    {
        public HotelsThanksViewModel ThanksViewModel { get; set; }

        public HotelsThanks()
        {
            InitializeComponent();

            #if DECOLAR
            MainLogo.Source = new BitmapImage(new Uri("ms-appx:/Product/Legacy/Assets/Image/decolar-logo.png", UriKind.Absolute));
            #endif            
            
            BugTracker.Instance.LogEvent("Hotels Purchase " + GlobalConfiguration.Site);

            ThanksViewModel = new HotelsThanksViewModel();
            HotelsThanksView.DataContext = ThanksViewModel;
            StackPepe.DataContext = ThanksViewModel.AvailabilityModel;
        }
        
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {        
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            BugTracker.Instance.LeaveBreadcrumb("Hotel Thanks");
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            // do nothing
        }

        private void BackToFlow_Click(object sender, RoutedEventArgs e)
        {
            OldPagesManager.ClearStack();
            OldPagesManager.GoTo(typeof(Home), null);
        }
       
    }
}