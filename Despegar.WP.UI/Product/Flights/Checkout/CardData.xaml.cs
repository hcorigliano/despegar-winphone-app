using Despegar.WP.UI.Model.ViewModel.Flights;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Despegar.WP.UI.Product.Flights.Checkout
{
    public sealed partial class CardData : UserControl
    {       
        private FlightsCheckoutViewModel ViewModel { get { return DataContext as FlightsCheckoutViewModel; } }
        
        public CardData()
        {
            this.InitializeComponent();
        }

        private void FillExpiration(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CoreBookingFields.form.payment.card.expiration.CoreValue = null;

            if (YearCombo.SelectedValue != null && MonthCombo.SelectedValue != null)            
                ViewModel.CoreBookingFields.form.payment.card.expiration.CoreValue = YearCombo.SelectedValue.ToString() + "-" + MonthCombo.SelectedValue.ToString();            
        }
      
        private void Voucher_LostFocus(object sender, RoutedEventArgs e)
        {
            ViewModel.ValidateVoucher();
        }
    }
}