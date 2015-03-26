using Despegar.WP.UI.Model.ViewModel.Hotels;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Despegar.WP.UI.Controls;


namespace Despegar.WP.UI.Product.Hotels.Checkout.Controls
{
    public sealed partial class BuyDetailsControl : UserControl
    {
        private HotelsCheckoutViewModel ViewModel { get { return DataContext as HotelsCheckoutViewModel; } }

        public BuyDetailsControl()
        {
            this.InitializeComponent();
        }


        private async void Grid_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            MessageDialog dialog = new MessageDialog(ViewModel.ItemSelected.price_destination.message);
            await dialog.ShowSafelyAsync();
        }
    }
}
