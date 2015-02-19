using Despegar.Core.Neo.InversionOfControl;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Model.ViewModel.Flights;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Despegar.WP.UI.Product.Flights
{
    public sealed partial class FlightMultipleEdit : Page
    {
        public MultipleEditionViewModel ViewModel { get; set; }

        public FlightMultipleEdit()
        {
            this.InitializeComponent();
            this.CheckDeveloperTools();                        
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {              
            ViewModel.BugTracker.LeaveBreadcrumb("Flight Multiple View - Back button pressed");
            ViewModel.Navigator.GoBack();
            e.Handled = true;                
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
           HardwareButtons.BackPressed += HardwareButtons_BackPressed;

           ViewModel = IoC.Resolve<MultipleEditionViewModel>();
           ViewModel.OnNavigated(e.Parameter);
           this.DataContext = ViewModel;

           MainPivotControl.ItemsSource = ViewModel.Segments;
           MainPivotControl.SelectedIndex = ViewModel.SelectedNavigationIndex - 1;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }
    }
}