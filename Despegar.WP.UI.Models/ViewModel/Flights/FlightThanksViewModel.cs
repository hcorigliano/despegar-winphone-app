using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Model.ViewModel.Classes;
using Despegar.WP.UI.Model.ViewModel.Classes.Flights;
using Despegar.WP.UI.Models.Classes;
using System.Windows.Input;

namespace Despegar.WP.UI.Model.ViewModel.Flights
{
    public class FlightThanksViewModel : ViewModelBase
    {
        private INavigator navigator;
        public FlightsCrossParameter FlightParameters { get; set; }

        public FlightThanksViewModel(INavigator nav, IBugTracker t) : base(t) 
        {
            this.navigator = nav;
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
            navigator.GoTo(ViewModelPages.Home, new HomeParameters() { ClearStack = true });            
        }
       
    }
}
