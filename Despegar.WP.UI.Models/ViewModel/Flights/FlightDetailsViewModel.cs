using Despegar.Core.Neo.Contract.Log;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Model.ViewModel;
using Despegar.WP.UI.Model.ViewModel.Classes;
using Despegar.WP.UI.Model.ViewModel.Classes.Flights;
using System.Windows.Input;

namespace Despegar.WP.UI.Model
{
    public class FlightDetailsViewModel : ViewModelBase
    {
        public FlightsCrossParameter FlightsCrossParameters { get; set; }
        private IGoogleAnalytics analyticsService;

        /// <summary>
        /// Inbound + Outbound Initialization
        /// </summary>
        /// <param name="outBound"></param>
        /// <param name="route2"></param>
        public FlightDetailsViewModel(INavigator navigator, IBugTracker t, IGoogleAnalytics analyticsService) : base(navigator,t)
        {
            this.analyticsService = analyticsService;
        }

        public bool IsTwoWaySearch
        {
            get { return this.FlightsCrossParameters.Inbound.segments != null; }
        }

        public ICommand BuyCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    // Todo send product data
                    Navigator.GoTo(ViewModelPages.FlightsCheckout, FlightsCrossParameters);
                });
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Navigator.GoBack();
                });
            }
        }

        public override void OnNavigated(object navigationParams)
        {
            BugTracker.LeaveBreadcrumb("Flight Detail View");
            analyticsService.SendView("FlightDetails");

            FlightsCrossParameter routes = navigationParams as FlightsCrossParameter;

            if (routes != null)
            {
                // Multiples are inserted as an Outbound collection of Routes
                FlightsCrossParameters = routes;
            }
        }
    }
}