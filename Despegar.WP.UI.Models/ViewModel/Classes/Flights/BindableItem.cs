using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Despegar.WP.UI.Model.Common;
using Despegar.Core.Neo.Business.Flight.Itineraries;

namespace Despegar.WP.UI.Model.Classes.Flights
{
    public class BindableItem : Item
    {
        public List<RoutesItems> RoutesCustom { get; set; }

        public BindableItem(Item item)
        {
            base.id = item.id;
            base.airline = item.airline;
            base.currency = item.currency;
            base.destination_country_code = item.destination_country_code;
            base.destination_type = item.destination_type;
            base.final_price = item.final_price;
            base.inbound =  (item.inbound == null) ? new List<Route>() : item.inbound ;
            base.outbound = (item.outbound == null) ? new List<Route>() : item.outbound;
            base.price = item.price;
            base.validating_carrier = item.validating_carrier;
            base.routes = (item.routes == null) ? new List<Route>() : item.routes;

            RoutesCustom = new List<RoutesItems>();

            LinkFlightRoutes();
        }


        public void LinkFlightRoutes()
        {

            if (inbound.Count == 0 && outbound.Count == 0)
            {
                outbound = routes;
            }

            if (RoutesCustom.Count == 0)
            {
                if (inbound.Count == 0 && outbound.Count>0)
                {
                    foreach (Route outboundItem in outbound)
                    {
                        //route inbound is created with choice -1 for UI animation
                        RoutesCustom.Add(new RoutesItems(new Route { choice = -1 }, outboundItem , price));
                    }
                }

                foreach (Route inboundItem in inbound)
                {
                    foreach (Route outboundItem in outbound)
                    {
                        RoutesCustom.Add(new RoutesItems(inboundItem, outboundItem , price));
                    }
                }

            }
        }

    }
}
