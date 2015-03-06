using Despegar.Core.Neo.Contract.Log;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Model.ViewModel.Classes;
using Despegar.WP.UI.Model.ViewModel.Classes.Flights;
using System.Windows.Input;

namespace Despegar.WP.UI.Model.ViewModel.Flights
{
    public class FlightThanksViewModel : ViewModelBase
    {
        public FlightsCrossParameter flightCrossParameters { get; set; }

        public FlightThanksViewModel(INavigator nav, IBugTracker t)
            : base(nav,t) 
        {
        }

        public bool IsETicketed { get { return flightCrossParameters.BookingResponse.eticket_token != null; } }

        public override void OnNavigated(object navigationParams)
        {
             BugTracker.LeaveBreadcrumb("Flight Thanks View");
             BugTracker.LogEvent("Flight Purchase " + GlobalConfiguration.Site);
             this.flightCrossParameters = navigationParams as FlightsCrossParameter;
        }

        public ICommand NavigateToHomeCommand
        { 
            get
            {
                return new RelayCommand(() => { this.NavigateToHome(); });
            }
        }

        private void NavigateToHome()
        {
            Navigator.GoTo(ViewModelPages.Home, new HomeParameters() { ClearStack = true });            
        }
        
    }
}
