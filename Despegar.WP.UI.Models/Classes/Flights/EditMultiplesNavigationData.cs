using Despegar.Core.Business.Flight.SearchBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model.Classes.Flights
{
    public class EditMultiplesNavigationData
    {
        public int SelectedSegmentIndex { get; set; }
        public FlightSearchModel SearchModel { get; set; }
    }
}
