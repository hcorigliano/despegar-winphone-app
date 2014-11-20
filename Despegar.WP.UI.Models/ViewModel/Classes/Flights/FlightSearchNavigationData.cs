using Despegar.Core.Business.Flight.SearchBox;
using Despegar.WP.UI.Model.ViewModel.Flights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model.Classes.Flights
{
    public class FlightSearchNavigationData
    {
        public FlightSearchModel SearchModel { get; set; }
        public PassengersViewModel PassengerModel { get; set; }
        public bool NavigatedFromMultiples { get; set; }
    }
}
