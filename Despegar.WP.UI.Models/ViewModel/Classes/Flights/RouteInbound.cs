using Despegar.Core.Neo.Business.Flight.Itineraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model.ViewModel.Classes.Flights
{
    public class RouteInbound: Route
    {
        public RouteInbound( Route route)
        {
            base.arrival = route.arrival;
            base.choice = route.choice;
            base.departure = route.departure;
            base.duration = route.duration;
            base.from = route.from;
            base.layovers = route.layovers;
            base.segments = route.segments;
            base.to = route.to;
        }
    }
}
