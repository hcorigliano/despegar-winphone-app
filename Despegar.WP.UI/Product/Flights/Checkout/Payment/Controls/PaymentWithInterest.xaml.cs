using Despegar.Core.Business.Flight.BookingFields;
using Despegar.WP.UI.Model.ViewModel;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Despegar.WP.UI.Product.Flights.Checkout.Payment.Controls
{
    public sealed partial class PaymentWithInterest : UserControl
    {
        private FlightsCheckoutViewModel ViewModel { get { return DataContext as FlightsCheckoutViewModel; } }

        public PaymentWithInterest()
        {
            this.InitializeComponent();
        }  

        private void Image_Load_Failed(object sender, ExceptionRoutedEventArgs e)
        {                         
            //((Image)sender).Source = new BitmapImage(new Uri("/Assets/Icon/CreditCard/GRL.png", UriKind.Relative));
        }

        private void OnRadioButton_Clicked(object sender, RoutedEventArgs e)
        {
            RadioButton a = (RadioButton)e.OriginalSource;
            ViewModel.SelectedInstallment =  new List<PaymentDetail> { a.DataContext as PaymentDetail };
        }

    }
}
