using Despegar.Core.Neo.Business.Flight.Itineraries;
using Despegar.WP.UI.Model.ViewModel.Classes.Flights;

namespace Despegar.WP.UI.Model.Classes.Flights
{
    public class RoutesItems
    {
        public RouteOutbound outbound { get; set; }
        public RouteInbound inbound { get; set; }
        public int price { get; set; }

        public RoutesItems(Route inboundItem , Route outboundItem , int price )
        {
            inbound = new RouteInbound(inboundItem);
            outbound = new RouteOutbound(outboundItem);
            this.price = price;
        }
    }
}