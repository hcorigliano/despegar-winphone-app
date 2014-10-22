using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
//using Despegar.Analytics;
//using System.Windows.Media.Imaging;
using Windows.UI.Xaml.Controls;
using Despegar.LegacyCore.ViewModel;
using Despegar.WP.UI.Classes;
using Windows.UI.Xaml;
using Despegar.WP.UI;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;

namespace Despegar.WP.UI.Product.Legacy
{
    public partial class HotelsThanks : Page
    {
        public HotelsThanksViewModel ThanksViewModel { get; set; }

        public HotelsThanks()
        {
            InitializeComponent();

            #if DECOLAR
            MainLogo.Source = new BitmapImage(new Uri("/Assets/Image/decolar-logo.png", UriKind.RelativeOrAbsolute));
            #endif            
            
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
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            // do nothing
        }

        private void BackToFlow_Click(object sender, RoutedEventArgs e)
        {
            PagesManager.ClearStack();
            PagesManager.GoTo(typeof(Home), null);
        }
       
    }
}