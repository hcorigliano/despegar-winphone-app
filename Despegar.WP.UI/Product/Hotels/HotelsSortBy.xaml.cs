using Despegar.Core.Neo.InversionOfControl;
using Despegar.WP.UI.Model.ViewModel.Hotels;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Despegar.WP.UI.Product.Hotels
{
    public sealed partial class HotelsSortBy : Page
    {
        public HotelsSortByViewModel ViewModel { get; set; }

        public HotelsSortBy()
        {
            this.InitializeComponent();
        }
       
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            ViewModel = IoC.Resolve<HotelsSortByViewModel>();
            ViewModel.OnNavigated(e.Parameter);
            this.DataContext = ViewModel;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            ViewModel.BugTracker.LeaveBreadcrumb("Hotels search orderby - Back button pressed");
            ViewModel.Navigator.GoBack();
            e.Handled = true;
        }
    }
}