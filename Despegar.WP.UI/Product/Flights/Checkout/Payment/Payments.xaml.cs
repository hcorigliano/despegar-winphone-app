using Despegar.Core.Business.Flight.BookingFields;
using Despegar.WP.UI.Model.Classes.Flights.Checkout;
using Despegar.WP.UI.Model.ViewModel;
using Despegar.WP.UI.Model.ViewModel.Flights;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Despegar.WP.UI.Product.Flights.Checkout.Payment
{
    public sealed partial class Payments : UserControl
    {
        private FlightsCheckoutViewModel ViewModel { get { return DataContext as FlightsCheckoutViewModel; } }

        public Payments()
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