using Despegar.Core.Neo.Business.Flight.SearchBox;
using Despegar.WP.UI.Model.ViewModel.Flights;

namespace Despegar.WP.UI.Model.Classes.Flights
{
    public class EditMultiplesNavigationData
    {
        public int SelectedSegmentIndex { get; set; }
        public FlightSearchModel SearchModel { get; set; }
        public PassengersViewModel PassengerModel { get; set; }
    }
}