using Despegar.WP.UI.BugSense;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Model;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Model.ViewModel.Classes.Flights;
using Despegar.WP.UI.Model.ViewModel.Flights;
using System;
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
            BugTracker.Instance.LeaveBreadcrumb("Flight Thanks View");
            BugTracker.Instance.LogEvent("Flight Purchase " + GlobalConfiguration.Site);

            HardwareButtons.BackPressed += HardwareButtons_BackPressed;            
#if !DEBUG
                GoogleAnalyticContainer ga = new GoogleAnalyticContainer();
                ga.Tracker = GoogleAnalytics.EasyTracker.GetTracker();
                ga.SendView("FlightThanks");
#endif

            FlightsCrossParameter flightsCrossParameters = e.Parameter as FlightsCrossParameter;

            ViewModel = new FlightThanksViewModel(Navigator.Instance, BugTracker.Instance);
            ViewModel.flightCrossParameters = flightsCrossParameters;

            DataContext = ViewModel;
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
