using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Despegar.LegacyCore.Util;
using Despegar.LegacyCore.Service;
using Despegar.LegacyCore.Connector.Domain.API;

namespace Despegar.LegacyCore.Model
{
    public class FlightsAvailabilityModel
    {

        public FlightsAvailabilityModel()
        {
            Logger.Info("[model:flight:detail] Flights Availability model created");
        }

        public void SetParamsByUrl(Uri uri)
        {
            string[] opts = uri.LocalPath.Split('/');

            Ticket    = opts[opts.Length - 3] + "!" + opts[opts.Length - 2];
            Itinerary = opts[opts.Length - 1];
            //MiscCurrency Curr = await CurrenciesModel.Get(ApplicationConfig.Instance.Country);
            //Currency = Curr.id;

            ApplicationConfig.Instance.BrowsingPages.Pop();
        }

        public async Task<FlightAvailability> GetAvailability()
        {
            FlightAvailability avail = await APIFlightsService.Availability(Ticket, Itinerary);
            MiscCurrency Curr = await CurrenciesModel.GetById(avail.meta.currencyCode);
            if (Curr != null) Currency = Curr.symbol;
            else Currency = "";
            return avail;
        }

        public string Ticket { set; get; }

        public string Itinerary { get; set; }

        public string Currency { get; set; }
    }
}
