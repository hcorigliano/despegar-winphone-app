using Despegar.Core.Neo.InversionOfControl;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Model;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Despegar.WP.UI.Product.Flights
{    
    public sealed partial class FlightDetail : Page
    {
        public FlightDetailsViewModel ViewModel { get; set; }

        public FlightDetail()
        {
            this.InitializeComponent();            
            this.CheckDeveloperTools();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            if (e.NavigationMode == NavigationMode.New || e.NavigationMode == NavigationMode.Back) 
            {             
                ViewModel = IoC.Resolve<FlightDetailsViewModel>();
                ViewModel.OnNavigated(e.Parameter);
                this.DataContext = ViewModel;

                // Check Search type
                if (!ViewModel.IsTwoWaySearch)
                {
                    // Remove "Return" pivot item
                    MainPivot.Items.RemoveAt(2);
                }

                if (ViewModel.FlightsCrossParameters.MultipleRoutes != null)
                {
                    // Remove "Go" pivot item (Only multiple)
                    MainPivot.Items.RemoveAt(1);
                }
                else { 
                    MainPivot.Items.RemoveAt(0);
                }

            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            ViewModel.BugTracker.LeaveBreadcrumb("Flight detail orderby - Back button pressed");
            ViewModel.Navigator.GoBack();
            e.Handled = true;
        }

    }
}