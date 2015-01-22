using Despegar.Core.Business.Common.Checkout;
using Despegar.Core.Business.Flight.BookingFields;
using Despegar.Core.Business.Flights;
using Despegar.WP.UI.Model.ViewModel.Flights;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Despegar.WP.UI.Product.Flights.Checkout.Controls
{
    public sealed partial class PaymentWithInterest : UserControl
    {
        private FlightsCheckoutViewModel ViewModel { get { return DataContext as FlightsCheckoutViewModel; } }

        public PaymentWithInterest()
        {
            this.InitializeComponent();
        }  

        private void OnRadioButton_Clicked(object sender, RoutedEventArgs e)
        {
            RadioButton a = (RadioButton)e.OriginalSource;
            ViewModel.SelectedInstallment = a.DataContext as InstallmentOption;
        }

    }
}
