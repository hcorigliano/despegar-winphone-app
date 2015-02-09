using Despegar.Core.Neo.InversionOfControl;
using Despegar.WP.UI.Model.ViewModel.Flights;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Despegar.WP.UI.Product.Flights
{

    public sealed partial class FlightSortBy : Page
    {
        public FlightOrderByViewModel ViewModel { get; set; }

        public FlightSortBy()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = IoC.Resolve<FlightOrderByViewModel>();
            ViewModel.OnNavigated(e.Parameter);
            this.DataContext = ViewModel;
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Navigator.GoBack();
        }

        private void AppBarCancelButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO Restore first state
            ViewModel.Navigator.GoBack();
        }

    }
}