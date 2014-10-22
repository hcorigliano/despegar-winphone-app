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
        public Outbound outbound { get; set; }
        public Inbound inbound { get; set; }

        public RoutesItems( Inbound inboundItem, Outbound outboundItem )
        {
            inbound = inboundItem;
            outbound = outboundItem;
        }
    }
}
