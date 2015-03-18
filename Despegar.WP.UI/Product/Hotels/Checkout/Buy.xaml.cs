using Despegar.WP.UI.Model.ViewModel.Hotels;
using Despegar.WP.UI.Controls;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace Despegar.WP.UI.Product.Hotels.Checkout
{
    public sealed partial class Buy : UserControl
    {
        private HotelsCheckoutViewModel ViewModel { get { return DataContext as HotelsCheckoutViewModel; } }

        public Buy()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog dialog = new MessageDialog(ViewModel.PaymentAlertMessage, "Error");
            await dialog.ShowSafelyAsync();
        }


        private void OnButtonClicked(object sender, RoutedEventArgs e)
        {
            //bool chkToc = (AcceptConditionsCheckBox.IsChecked != null) ? AcceptConditionsCheckBox.IsChecked.Value : false;
            //if (OnUserControlButtonClicked != null & chkToc)
            //    OnUserControlButtonClicked(this, e);
            //else
            //{
            //    // TODO : Show messagge Error : Isn't checked TOC
            //}
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
