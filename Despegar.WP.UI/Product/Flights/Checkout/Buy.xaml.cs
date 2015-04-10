using Despegar.WP.UI.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Despegar.WP.UI.Product.Flights.Checkout
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Buy : UserControl
    {
        public delegate void EventHandler(object sender, RoutedEventArgs e);
        public event EventHandler OnUserControlButtonClicked;

        public Buy()
        {
            this.InitializeComponent();
        }

        private async void AcceptConditions_Click(object sender, RoutedEventArgs e)
        {
            string uriToLaunch = String.Format("https://secure.despegar.com.{0}/book/flights/checkout/conditions/wp", GlobalConfiguration.Site.ToLowerInvariant());

            if (GlobalConfiguration.Site.ToLowerInvariant() == "br")
                uriToLaunch = "https://secure.decolar.com/book/flights/checkout/conditions/wp";

            var uri = new Uri(uriToLaunch);
            var success = await Windows.System.Launcher.LaunchUriAsync(uri);
        }
    }
}
