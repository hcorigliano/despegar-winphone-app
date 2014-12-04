using Despegar.Core.Business.Configuration;
using Despegar.Core.Business.Flight.BookingFields;
using Despegar.Core.IService;
using Despegar.WP.UI.Model;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    }
}