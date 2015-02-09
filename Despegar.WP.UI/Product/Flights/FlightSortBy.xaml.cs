using Despegar.Core.Neo.InversionOfControl;
using Despegar.WP.UI.Model.ViewModel.Flights;
using Windows.Phone.UI.Input;
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
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            ViewModel = IoC.Resolve<FlightOrderByViewModel>();
            ViewModel.OnNavigated(e.Parameter);
            this.DataContext = ViewModel;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            ViewModel.BugTracker.LeaveBreadcrumb("Flight search orderby - Back button pressed");
            ViewModel.Navigator.GoBack();
            e.Handled = true;
        }

    }
}