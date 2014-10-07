using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Despegar.Core.ViewModel;
using Despegar.Core.Util;
//using Despegar.Analytics;
using System.Windows.Media.Imaging;

namespace Despegar.View
{
    public partial class HotelsThanks : PhoneApplicationPage
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
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Logger.Info(String.Format("[view:hotel:thanks] Hotel Thanks page navigated for", this.ThanksViewModel.ToString()));
            //Track.View("HotelsThanksPage");
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            // do nothing
        }

        private void BackToFlow_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Home.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}