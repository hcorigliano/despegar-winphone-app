using Despegar.Core.Business.Flight.Itineraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model.Classes.Flights
{
    public class RoutesItems
    {
        public Route outbound { get; set; }
        public Route inbound { get; set; }

        public RoutesItems( Route inboundItem, Route outboundItem )
        {
            inbound = inboundItem;
            outbound = outboundItem;
        }
    }
}
