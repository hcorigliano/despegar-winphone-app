using Despegar.Core.Neo.InversionOfControl;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Model.ViewModel.Flights;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace Despegar.WP.UI.Product.Flights
{
    public sealed partial class FlightThanks : Page
    {
        private FlightThanksViewModel ViewModel;

        public FlightThanks()
        {
            this.InitializeComponent();           
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;            
#if !DEBUG
                GoogleAnalyticContainer ga = new GoogleAnalyticContainer();
                ga.Tracker = GoogleAnalytics.EasyTracker.GetTracker();
                ga.SendView("FlightThanks");
#endif
            ViewModel = Despegar.Core.Neo.InversionOfControl.IoC.Resolve<FlightThanksViewModel>();
            ViewModel.OnNavigated(e.Parameter);
            this.DataContext = ViewModel;

            if (ViewModel.flightCrossParameters.Inbound.choice == -1)
            {
                FlightsSegmentReturnTextBlock.Visibility = Visibility.Collapsed;
                FlightsSegmentReturnSrc.Visibility = Visibility.Collapsed;
            }
            if (ViewModel.flightCrossParameters.MultipleRoutes == null)
                FlightsSegmentGoSrcMultiple.Visibility = Visibility.Collapsed;
            else
                FlightsSegmentGoSrc.Visibility = Visibility.Collapsed;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            ViewModel.NavigateToHomeCommand.Execute(null);        
        }
    }
}
