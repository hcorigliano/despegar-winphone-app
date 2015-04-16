using Despegar.Core.Neo.Business.Flight.Itineraries;
using Despegar.Core.Neo.Business.Flight.SearchBox;
using Despegar.Core.Neo.Business.Hotels.CitiesAvailability;
using Despegar.Core.Neo.Business.Hotels.HotelDetails;
using Despegar.Core.Neo.Business.Hotels.SearchBox;
using Despegar.Core.Neo.Contract.API;
using Despegar.Core.Neo.InversionOfControl;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Model.ViewModel.Classes;
using Despegar.WP.UI.Model.ViewModel.Classes.Flights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Developer
{
    //Still in progress.
    public class QuickLinks
    {
        private static string ORIGIN_FLIGHT = "BUE";
        private static string DESTINATION_FLIGHT = "LAX";
        private static int IN_PLUS_DAYS = 2;
        private static int OUT_PLUS_DAYS = IN_PLUS_DAYS + 1; 


        public INavigator Navigator { get; set; }

        public async Task GoToFlightsCheckout()
        {
            Navigator = IoC.Resolve<INavigator>();

            DateTimeOffset Today = DateTime.Now;

            FlightSearchModel coreSearchModel = new FlightSearchModel();

            coreSearchModel.AdultsInFlights = 1;
            coreSearchModel.DepartureDate = Today.AddDays(IN_PLUS_DAYS);
            coreSearchModel.DestinationDate = Today.AddDays(OUT_PLUS_DAYS);
            coreSearchModel.DestinationFlight = DESTINATION_FLIGHT;
            coreSearchModel.OriginFlight = ORIGIN_FLIGHT;

            string extra = String.Format("{0} to {1}, [Passengers]: {2}",
                coreSearchModel.DepartureDate.ToString("yyyy-MM-dd"),
                coreSearchModel.DestinationDate != null ? coreSearchModel.DestinationDate.ToString("yyyy-MM-dd") : "-",
                "TotalAdults " + coreSearchModel.AdultsInFlights + " Child: " + coreSearchModel.ChildrenInFlights);

            FlightsItineraries  Itineraries = new FlightsItineraries();

            try
            {
                IMAPIFlights flightService = IoC.Resolve<IMAPIFlights>();
                Itineraries = await flightService.GetItineraries(coreSearchModel);
            }
            catch
            {
                //Do nothing.
            }

            if (Itineraries.items.Count() > 0)
            {
                FlightsCrossParameter FlightsCrossParameters = new FlightsCrossParameter();
                FlightsCrossParameters.FlightId = Itineraries.items[0].id;
                FlightsCrossParameters.Inbound = Itineraries.items[0].inbound[0];
                FlightsCrossParameters.Outbound = Itineraries.items[0].outbound[0];

                Navigator.GoTo(ViewModelPages.FlightsCheckout, FlightsCrossParameters);
            }

            
        }

        public void GoToFlightsResults()
        {
            Navigator = IoC.Resolve<INavigator>();

            DateTimeOffset Today = DateTime.Now;

            FlightSearchModel coreSearchModel = new FlightSearchModel();

            coreSearchModel.AdultsInFlights = 1;
            coreSearchModel.DepartureDate = Today.AddDays(IN_PLUS_DAYS);
            coreSearchModel.DestinationDate = Today.AddDays(OUT_PLUS_DAYS);
            coreSearchModel.DestinationFlight = DESTINATION_FLIGHT;
            coreSearchModel.OriginFlight = ORIGIN_FLIGHT;

            string extra = String.Format("{0} to {1}, [Passengers]: {2}",
                coreSearchModel.DepartureDate.ToString("yyyy-MM-dd"),
                coreSearchModel.DestinationDate != null ? coreSearchModel.DestinationDate.ToString("yyyy-MM-dd") : "-",
                "TotalAdults " + coreSearchModel.AdultsInFlights + " Child: " + coreSearchModel.ChildrenInFlights);

            Navigator.GoTo(ViewModelPages.FlightsResults, new GenericResultNavigationData() { SearchModel = coreSearchModel, FiltersApplied = false });
        }

        public async Task GoToHotelsCheckout()
        {
            HotelDatails HotelDetail = new HotelDatails();
            CitiesAvailability CitiesAvailability = new CitiesAvailability();
            HotelSearchModel SearchModel = new HotelSearchModel();
            IMAPIHotels hotelService = IoC.Resolve<IMAPIHotels>();

            DateTimeOffset Today = DateTime.Now;

            SearchModel.CheckinDate = Today.AddDays(IN_PLUS_DAYS);
            SearchModel.CheckoutDate = Today.AddDays(OUT_PLUS_DAYS);
            PassengersForRooms pfr = new PassengersForRooms();
            pfr.GeneralAdults = 1;
            SearchModel.Rooms.Add(pfr);
            SearchModel.DestinationCode = 982;
            SearchModel.Currency = "ars";
            SearchModel.Offset = 0;
            SearchModel.Limit = 20;

            try
            {
                CitiesAvailability = await hotelService.GetHotelsAvailability(SearchModel);
            }
            catch
            {
                //Do nothing
            }

            //Terminar
            //HotelDetail = await hotelService.GetHotelsDetail(CrossParameters.IdSelectedHotel, CrossParameters.SearchModel.DepartureDateFormatted, CrossParameters.SearchModel.DestinationDateFormatted, CrossParameters.SearchModel.DistributionString);

        }

    }

}
