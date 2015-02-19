using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Despegar.Core.Neo.Business.Flight.Itineraries;
using Despegar.Core.Neo.Business.Enums;
using Despegar.WP.UI.Model;
using Despegar.Core.Neo.Business.CustomErrors;
using Despegar.Core.Neo.API;

namespace Despegar.Core.Neo.Business.Flight.SearchBox
{
    public class FlightSearchModel : BusinessModelBase
    {
        public const int MAX_MULTIPLE_SEGMENTS = 6;
        public const int MIN_MULTIPLE_SEGMENTS = 2;
        public FlightSearchPages PageMode { get; set; }
        public DateTimeOffset DepartureDate { get; set; }
        public DateTimeOffset DestinationDate { get; set; }
        public string OriginFlight { get; set; }
        public string DestinationFlight { get; set; }
        public string OriginFlightText { get; set; }
        public string DestinationFlightText { get; set; }
        public int AdultsInFlights { get; set; }
        public int ChildrenInFlights { get; set; }
        public int InfantsInFlights { get; set; }
        public int LimitResult { get; set; }
        public int Offset { get; set; }
        public List<FlightMultipleSegment> MultipleSegments { get; set; }

        /// <summary>
        /// Loaded after calling GetItineraries
        /// </summary>
        public List<Facet> Facets { get; set; }

        /// <summary>
        /// Loaded after calling GetItineraries
        /// </summary>
        public Sorting Sorting { get; set; }

        private SortingOption selectedSortingOption;
        public SortingOption SelectedSortingOption
        {
            get
            {
                var nullValue = new SortingOption { label = String.Empty, type = String.Empty, value = String.Empty };

                if (Sorting != null)
                {
                    var item = Sorting.values.FirstOrDefault(x => x.selected);
                    return item != null ? item : nullValue;
                }

                return nullValue;
            }           
        }

        public int TotalPassangers 
        {
            get
            {
                return AdultsInFlights + ChildrenInFlights + InfantsInFlights;
            }
        }

        public CustomError SearchErrors { get; set; }

        public int EmissionAnticipationDay { get; set; }

        public int LastAvailableHours { get; set; }

        private DateTime DateBoundary { get; set; }

        public FlightSearchModel()
        {
            //TODO uncomment following code for advance search
            //this.AdvanceSearch = new AdvanceSearchModel();
            this.AdultsInFlights = 1;
            this.ChildrenInFlights = 0;
            this.InfantsInFlights = 0;

            this.OriginFlight = string.Empty;
            this.DestinationFlight = string.Empty;
            this.OriginFlightText = string.Empty;
            this.DestinationFlightText = string.Empty;

            this.DepartureDate = DateTime.Today.AddDays(EmissionAnticipationDay);
            this.DestinationDate = DateTime.Today.AddDays(EmissionAnticipationDay);

            this.MultipleSegments = new List<FlightMultipleSegment>();
            AddMultipleSegment();
            AddMultipleSegment();

            // TODO these values are going to be handle next interation
            this.LimitResult = 200;
            this.Offset = 0;

            //this.SearchErrors = new List<CustomError>();
        }

        public void AddMultipleSegment()
        {
            if (this.MultipleSegments.Count < MAX_MULTIPLE_SEGMENTS)
                this.MultipleSegments.Add(new FlightMultipleSegment() { Index = this.MultipleSegments.Count + 1, DepartureDate = DateTime.Today, AirportDestination=String.Empty, AirportOrigin=String.Empty, AirportOriginText = String.Empty, AirportDestinationText = String.Empty});            
        }

        public void RemoveMultipleSegment()
        {
            if (this.MultipleSegments.Count > MIN_MULTIPLE_SEGMENTS)
                this.MultipleSegments.Remove(MultipleSegments.Last());
        }

        public override bool Validate()
        {
            return IsValid;
        }

     
        public void UpdateSearchDays()
        {
            DateTime daysToAdd;
            DateTime daysToCompare = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, LastAvailableHours, 0, 0);

            if (DateTime.Compare(DateTime.Now, daysToCompare) > 0)
            {
                daysToAdd = DateTime.Today.AddDays(EmissionAnticipationDay + 1);
            }
            else
            {
                daysToAdd = DateTime.Today.AddDays(EmissionAnticipationDay);
            }

            this.DepartureDate = daysToAdd;
            this.DestinationDate = daysToAdd;

            DateBoundary = daysToAdd;
        }

        // TODO, Use Errors List!!
        public override bool IsValid
        {
            get
            {
                switch (this.PageMode)
                {
                    case FlightSearchPages.RoundTrip:

                        if (!CommonValidations())
                            return false;

                        if (DestinationDate < DepartureDate)
                        {
                            SearchErrors = new CustomError("fecha partida menor a llegada.", "FLIGHT_SEARCH_DESTINATIONDATE_SMALLER_THAN_DEPARTURE_ERROR_MESSAGE", "isValid");
                            return false;
                        }

                        if (DepartureDate < DateBoundary)
                        {
                            // Cannot fly in the past.
                            SearchErrors = new CustomError(string.Empty, "FLIGHT_SEARCH_DEPARTUREDATE_SMALLER_THAN_ERROR_MESSAGE", "isValid",true,DateBoundary.ToString("dd-MM-yyyy"));
                            return false;
                        }

                        if (DestinationDate < DateBoundary)
                        {
                            // Cannot fly in the past.
                            SearchErrors = new CustomError(string.Empty, "FLIGHT_SEARCH_DEPARTUREDATE_SMALLER_THAN_ERROR_MESSAGE", "isValid", true, DateBoundary.ToString("dd-MM-yyyy"));
                            return false;
                        }

                        if (DateTime.Today.AddDays(329.0) < DestinationDate)
                        {
                            SearchErrors = new CustomError("La fecha no puede ser superior a la fecha ", "FLIGHT_SEARCH_DESTINATIONDATE_GREATER_THAN_MAXDAY_ERROR_MESSAGE", "isValid", true, DateTime.Today.AddDays(329.0).ToString("dd-MM-yyyy"));
                            return false;
                        }
                        break;

                    case FlightSearchPages.OneWay:
                        if (!CommonValidations())
                        {
                            return false;
                        }

                        if (DepartureDate < DateBoundary)
                        {
                            // Cannot fly in the past.
                            //SearchErrors = new CustomError("Fecha desde tiene que ser mayor a la de hoy.", "FLIGHT_SEARCH_DEPARTUREDATE_SMALLER_THAN_TODAY_ERROR_MESSAGE", "isValid");
                            SearchErrors = new CustomError(string.Empty, "FLIGHT_SEARCH_DEPARTUREDATE_SMALLER_THAN_ERROR_MESSAGE", "isValid", true, DateBoundary.ToString("dd-MM-yyyy"));
                            return false;
                        }

                        break;

                    case FlightSearchPages.Multiple:
                        if (MultipleSegments.Count > MAX_MULTIPLE_SEGMENTS)
                        {
                            // Maximum exceeded
                            SearchErrors = new CustomError("Cantidad de segmentos máximo excedido.", "FLIGHT_SEARCH_SEGMENT_MAX_ERROR_MESSAGE", "isValid");
                            return false;
                        }

                        foreach (var segment in MultipleSegments)
                        {
                            if (segment.DepartureDate < DateBoundary)
                            {
                                // Cannot fly in the past.
                                SearchErrors = new CustomError(string.Empty, "FLIGHT_SEARCH_DEPARTUREDATE_SMALLER_THAN_ERROR_MESSAGE", "isValid", true, DateBoundary.ToString("dd-MM-yyyy"));
                                return false;
                            }

                            if (String.IsNullOrEmpty(segment.AirportOrigin) || String.IsNullOrEmpty(segment.AirportDestination))
                            {
                                // No destinations
                                SearchErrors = new CustomError("Seleccione destino para el segmento.", "FLIGHT_SEARCH_AIRPORT_NO_SELECTED_ERROR_MESSAGE", "isValid");
                                return false;
                            }
                        }
                        break;
                }

                return true;
            }
        }

        private bool CommonValidations() 
        {
            if (String.IsNullOrEmpty(OriginFlight))
            {
                // No origin
                SearchErrors = new CustomError("Debe que seleccionar origen.", "FLIGHT_SEARCH_NO_ORIGIN_ERROR_MESSAGE", "CommonValidations");
                return false;
            }

            if (String.IsNullOrEmpty(DestinationFlight))
            {
                // No destinations
                SearchErrors = new CustomError("Debe que seleccionar destino.", "FLIGHT_SEARCH_NO_DESTINY_ERROR_MESSAGE", "CommonValidations");
                return false;
            }

            if (AdultsInFlights <= 0)
            {
                SearchErrors = new CustomError("Tiene que haber al menos un adulto.", "FLIGHT_SEARCH_MIN_ADULTS_ERROR_MESSAGE", "CommonValidations");
                // Adult obligatory
                return false;
            }

            if (AdultsInFlights < InfantsInFlights)
            {
                SearchErrors = new CustomError("Tiene que haber un adulto por cada infante.", "FLIGHT_SEARCH_ADULT_INFANT_MATCH_ERROR_MESSAGE", "CommonValidations");
                // An adult foreach infant
                return false;
            }

            return true;
        }

        /// <summary>
        /// Converts the selected Facets into querystring paramaters for MAPI
        /// </summary>
        public string FormattedFacetsCodes
        {
            get
            {
                List<String> facetListNames = new List<string>();
                if (Facets == null) 
                    return String.Empty;

                foreach (Facet facet in Facets)
                {
                    var response = facet.values.Where(x => x.selected == true).Select(x => x.value).ToList();

                    if (response != null)
                    {
                        String parameters = String.Join(",", response.ToList());
                        facetListNames.Add(facet.criteria + "=" + parameters);
                    }
                }

                if (facetListNames.Count == 0) 
                    return String.Empty;

                return String.Join("&", facetListNames);
            }
        }

    }
}
