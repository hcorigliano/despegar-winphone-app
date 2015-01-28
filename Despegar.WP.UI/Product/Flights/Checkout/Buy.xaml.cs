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

        private void OnButtonClicked(object sender, RoutedEventArgs e)
        {
            bool chkToc = (AcceptConditionsCheckBox.IsChecked!=null) ? AcceptConditionsCheckBox.IsChecked.Value : false;
            if (OnUserControlButtonClicked != null & chkToc)
                OnUserControlButtonClicked(this, e);
            else
            {
                // TODO : Show messagge Error : Isn't checked TOC
            }
        }

        private async void AcceptConditions_Click(object sender, RoutedEventArgs e)
        {
            string uriToLaunch = @"https://secure.despegar.com.ar/book/flights/checkout/conditions/wp";
            var uri = new Uri(uriToLaunch);
            var success = await Windows.System.Launcher.LaunchUriAsync(uri);

            if (success)
            {
                // URI launched
            }
            else
            {
                // URI launch failed
            }

        }
    }
}
