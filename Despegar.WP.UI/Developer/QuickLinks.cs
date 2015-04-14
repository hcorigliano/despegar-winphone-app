using Despegar.Core.Neo.Business.Flight.SearchBox;
using Despegar.Core.Neo.InversionOfControl;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Model.ViewModel.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Developer
{
    public class QuickLinks
    {
        private static string ORIGIN_FLIGHT = "BUE";
        private static string DESTINATION_FLIGHT = "LAX";


        public INavigator Navigator { get; set; }
        private FlightSearchModel coreSearchModel;

        public void GoToFlightsCheckout()
        {
            Navigator = IoC.Resolve<INavigator>();

            DateTimeOffset Today = DateTime.Now;

            coreSearchModel = new FlightSearchModel();

            coreSearchModel.AdultsInFlights = 1;
            coreSearchModel.DepartureDate = Today.AddDays(1);
            coreSearchModel.DestinationDate = Today.AddDays(2);
            coreSearchModel.DestinationFlight = DESTINATION_FLIGHT;
            coreSearchModel.OriginFlight = ORIGIN_FLIGHT;

            string extra = String.Format("{0} to {1}, [Passengers]: {2}",
                coreSearchModel.DepartureDate.ToString("yyyy-MM-dd"),
                coreSearchModel.DestinationDate != null ? coreSearchModel.DestinationDate.ToString("yyyy-MM-dd") : "-",
                "TotalAdults " + coreSearchModel.AdultsInFlights + " Child: " + coreSearchModel.ChildrenInFlights);

            Navigator.GoTo(ViewModelPages.FlightsResults, new GenericResultNavigationData() { SearchModel = coreSearchModel, FiltersApplied = false });


        }


    }


}
