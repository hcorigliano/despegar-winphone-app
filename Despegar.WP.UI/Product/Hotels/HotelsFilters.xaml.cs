using Despegar.Core.Neo.InversionOfControl;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Model.ViewModel.Hotels;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Despegar.WP.UI.Product.Hotels
{
    public sealed partial class HotelsFilters : Page
    {
        public HotelsFiltersViewModel ViewModel { get; set; }

        public HotelsFilters()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            ViewModel = IoC.Resolve<HotelsFiltersViewModel>();
            ViewModel.OnNavigated(e.Parameter);
            this.DataContext = ViewModel;
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            ViewModel.BugTracker.LeaveBreadcrumb("Hotels search filters - Back button pressed");
            ViewModel.Navigator.GoBack();
            e.Handled = true;
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Navigator.Instance.GoBack();
        }

        private void AppBarCancelButton_Click(object sender, RoutedEventArgs e)
        {
            Navigator.Instance.GoBack();
        }
    }
}