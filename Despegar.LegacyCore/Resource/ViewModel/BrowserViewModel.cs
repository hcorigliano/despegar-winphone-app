using Despegar.LegacyCore.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Despegar.LegacyCore.Resource;

namespace Despegar.LegacyCore.ViewModel
{
    public class BrowserViewModel
    {

        public BrowserViewModel()
        {
            Logger.Info("[vm:browser] Browser ViewModel initialized");
        }

        public string GetPageByUrl(string url)
        {
            string product = "";

            if (url.EndsWith(Properties.HomeProductHotelsUrl))
                product = "HotelsHome";

            else if (url.Contains(Properties.HomeProductHotelsUrl + "search"))
                product = "HotelsSearch";

            else if (url.Contains(Properties.HomeProductHotelsUrl + "detail"))
                product = "HotelsDetail";

            else if (url.Contains("hotels/checkout/conditions"))
                product = "HotelsTermsAndConditions";

            else if (url.EndsWith(Properties.HomeProductFlightsUrl))
                product = "FlightsHome";

            else if (url.Contains(Properties.HomeProductFlightsUrl + "roundtrip"))
                product = "FlightsSearch";

            else if (url.Contains(Properties.HomeProductFlightsUrl + "oneway"))
                product = "FlightsSearch";

            else if (url.Contains(Properties.HomeProductFlightsUrl + "multiple-destinations"))
                product = "FlightsSearch";

            else if (url.Contains(Properties.HomeProductFlightsUrl + "detail"))
                product = "FlightsDetail";

            else if (url.Contains("flights/checkout/conditions"))
                product = "FlightsTermsAndConditions";

            else if (url.EndsWith(Properties.HomeProductCarsUrl))
                product = "CarsHome";

            else if (url.Contains(Properties.HomeProductCarsUrl + "result"))
                product = "CarsSearch";

            else if (url.EndsWith(Properties.HomeProductSelfserviceUrl))
                product = "SelfServiceHome";

            else if (url.EndsWith(Properties.HomeProductFlightTrackerUrl))
                product = "FlightTrackerHome";

            else if (url.Contains("tracker/detail"))
                product = "FlightTrackerDetail";

            else product = url;

            return product;
        }
    }
}
