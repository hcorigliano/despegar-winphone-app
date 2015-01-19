using Despegar.WP.UI.Model.ViewModel.Flights;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Despegar.WP.UI.Product.Flights.Checkout.Passegers
{
    public sealed partial class Passengers : UserControl
    {
        //public FlightsCheckoutModel ViewModel { get { return DataContext as FlightsCheckoutModel; } }
        
        public Passengers()
        {
            this.InitializeComponent();
            //nationality.Text = ViewModel.Countries.FirstOrDefault(x => x.id == ((Passenger)this.DataContext).nationality.value).id;
        }

        private void NationalitySelection_GotFocus(object sender, RoutedEventArgs e)
        {
            ((FlightsCheckoutViewModel)DataContext).NationalityIsOpen = true;
        }
    }
}