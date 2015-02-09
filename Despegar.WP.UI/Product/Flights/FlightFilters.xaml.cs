using Despegar.Core.Neo.InversionOfControl;
using Despegar.WP.UI.Model.ViewModel.Flights;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Despegar.WP.UI.Product.Flights
{
    public sealed partial class FlightFilters : Page
    {
        public FlightFiltersViewModel ViewModel { get; set; }

        public FlightFilters()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            ViewModel = IoC.Resolve<FlightFiltersViewModel>();
            ViewModel.OnNavigated(e.Parameter);
            this.DataContext = ViewModel;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {                          
            ViewModel.BugTracker.LeaveBreadcrumb("Flight search filters - Back button pressed");
            ViewModel.Navigator.GoBack();
            e.Handled = true;
        }
    }
}