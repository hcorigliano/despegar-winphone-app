using Despegar.Core.Business.Flight.Itineraries;
using Despegar.WP.UI.Model.ViewModel.Classes.Flights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model.Classes.Flights
{
    public class RoutesItems
    {
        public RouteOutbound outbound { get; set; }
        public RouteInbound inbound { get; set; }

        public RoutesItems( Route inboundItem , Route outboundItem )
        {
            inbound = new RouteInbound(inboundItem);
            outbound = new RouteOutbound(outboundItem);
        }
    }
}
