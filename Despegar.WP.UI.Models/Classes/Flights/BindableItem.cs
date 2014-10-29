using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Despegar.Core.Business.Flight.Itineraries;

namespace Despegar.WP.UI.Model.Classes.Flights
{
    public class BindableItem : Despegar.Core.Business.Flight.Itineraries.Item
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
            base.inbound =  (item.inbound == null) ? new List<Inbound>() : item.inbound ;
            base.outbound = (item.outbound == null) ? new List<Outbound>() : item.outbound;
            base.price = item.price;
            base.validating_carrier = item.validating_carrier;
            RoutesCustom = new List<RoutesItems>();

            LinkFlightRoutes();
        }


        public void LinkFlightRoutes()
        {
            if (RoutesCustom.Count == 0)
            {
                if (inbound.Count == 0 && outbound.Count>0)
                {
                    foreach (Outbound outboundItem in outbound)
                    {
                        RoutesCustom.Add(new RoutesItems(null, outboundItem));
                    }
                }

                foreach (Inbound inboundItem in inbound)
                {
                    foreach (Outbound outboundItem in outbound)
                    {
                        RoutesCustom.Add(new RoutesItems(inboundItem, outboundItem));
                    }
                }

            }
        }
    }
}
