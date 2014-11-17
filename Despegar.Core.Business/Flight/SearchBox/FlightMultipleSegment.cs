using Despegar.WP.UI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace Despegar.Core.Business.Flight.SearchBox
{
    public class FlightMultipleSegment
    {
        public int Index { get; set; }
        public DateTimeOffset DepartureDate { get; set; }
        public string AirportOrigin { get; set; }
        public string AirportDestination { get; set; }        
    }
}