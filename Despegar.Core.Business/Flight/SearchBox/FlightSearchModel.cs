using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Despegar.Core.Business.Flight.Itineraries;
using Despegar.Core.Business.Enums;
using Despegar.WP.UI.Model;


namespace Despegar.Core.Business.Flight.SearchBox
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
        public int AdultsInFlights { get; set; }
        public int ChildrenInFlights { get; set; }
        public int InfantsInFlights { get; set; }
        public int LimitResult { get; set; }
        public int Offset { get; set; }
        public List<FlightMultipleSegment> MultipleSegments { get; set; }

        public List<Facet> FacetsSearch { get; set; }
        private Value3 OldValue { get; set; }
        public bool HasNewSortingSearch { get; set; }
        public string SortingCriteriaSearch { get; set; }
        public SearchStates SearchStatus { get; set; }


        public FlightSearchModel()
        {
            //TODO uncomment following code for advance search
            //this.AdvanceSearch = new AdvanceSearchModel();
            this.AdultsInFlights = 1;
            this.ChildrenInFlights = 0;
            this.InfantsInFlights = 0;

            this.OriginFlight = string.Empty;
            this.DestinationFlight = string.Empty;

            this.DepartureDate = DateTime.Today.AddDays(2);
            this.DestinationDate = DateTime.Today.AddDays(2);

            this.MultipleSegments = new List<FlightMultipleSegment>();
            AddMultipleSegment();
            AddMultipleSegment();

            // TODO these values are going to be handle next interation
            this.LimitResult = 20;
            this.Offset = 0;
        }

        public void AddMultipleSegment()
        {
            if (this.MultipleSegments.Count < MAX_MULTIPLE_SEGMENTS)
                this.MultipleSegments.Add(new FlightMultipleSegment() { Index = this.MultipleSegments.Count + 1, DepartureDate = DateTime.Today, AirportOrigin = "", AirportDestination = "" });            
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

        /// <summary>
        /// Builds the flights query string for the service
        /// </summary>
        /// <returns></returns>
        public string GetQueryUrl()
        {
            string serviceUrl = String.Empty;

            switch (this.PageMode)
            {
                case FlightSearchPages.RoundTrip:
                    serviceUrl = String.Format(ServiceURL.GetServiceURL(ServiceKey.FlightItineraries), this.OriginFlight, this.DestinationFlight, this.DepartureDate.ToString("yyyy-MM-dd"), this.AdultsInFlights, this.DestinationDate.ToString("yyyy-MM-dd"), this.ChildrenInFlights, this.InfantsInFlights, this.Offset, this.LimitResult, "", "", "", "");
                    break;
                case FlightSearchPages.OneWay:
                    serviceUrl = String.Format(ServiceURL.GetServiceURL(ServiceKey.FlightItineraries), this.OriginFlight, this.DestinationFlight, this.DepartureDate.ToString("yyyy-MM-dd"), this.AdultsInFlights, "", this.ChildrenInFlights, this.InfantsInFlights, this.Offset, this.LimitResult, "", "", "", "");
                    break;
                case FlightSearchPages.Multiple:
                    string vectorizedDates =   String.Join(",", MultipleSegments.Select(x => x.DepartureDate.Date.ToString("yyyy-MM-dd") ));
                    string vectorizedOrigins = String.Join(",", MultipleSegments.Select(x => x.AirportOrigin));
                    string vectorizedDestinations = String.Join(",", MultipleSegments.Select(x => x.AirportDestination)); 

                    serviceUrl = String.Format(ServiceURL.GetServiceURL(ServiceKey.FlightItineraries), vectorizedOrigins, vectorizedDestinations,vectorizedDates, this.AdultsInFlights, "", this.ChildrenInFlights, this.InfantsInFlights, this.Offset, this.LimitResult, "", "", "", "");
                    break;
            }

            return serviceUrl;
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

                        if (DepartureDate < DateTime.Today)
                            // Cannot fly in the past.
                            return false;

                        if (DestinationDate < DateTime.Today)
                            // Cannot fly in the past.
                            return false;
                        break;

                    case FlightSearchPages.OneWay:
                        if (!CommonValidations())
                            return false;

                        if (DepartureDate < DateTime.Today)
                            // Cannot fly in the past.
                            return false;
                        break;

                    case FlightSearchPages.Multiple:
                        if (MultipleSegments.Count > MAX_MULTIPLE_SEGMENTS)
                            // Maximum exceeded
                            return false;

                        foreach (var segment in MultipleSegments)
                        {
                            if (segment.DepartureDate < DateTime.Today)
                                // Cannot fly in the past.
                                return false;

                            if (String.IsNullOrEmpty(segment.AirportOrigin) || String.IsNullOrEmpty(segment.AirportDestination))
                                // No destinations
                                return false;
                        }
                        break;
                }

                return true;
            }
        }

        private bool CommonValidations() 
        {
            if (String.IsNullOrEmpty(OriginFlight) || String.IsNullOrEmpty(DestinationFlight))
                // No destinations
                return false;

            if (AdultsInFlights <= 0)
                // Adult obligatory
                return false;

            if (AdultsInFlights < InfantsInFlights)
                // An adult foreach infant
                return false;

            return true;
        }


        public string FacetsCodes
        {
            get
            {
                List<String> facetListNames = new List<string>();
                if (FacetsSearch == null) return String.Empty;

                foreach (Facet facet in FacetsSearch)
                {
                    var elements = facet.values.Where(x => x.selected == true);

                    var response = (from value in elements
                                    select value.value);

                    if (response != null)
                    {
                        facetListNames.AddRange(response.ToList());
                    }
                }

                if (facetListNames.Count == 0) return String.Empty;

                return String.Join(",", facetListNames);
            }
        }

        public Value3 SortingValuesSearch
        {
            get
            {
                if (OldValue == null) return new Value3 { label = String.Empty, type = String.Empty, value = String.Empty };
                return OldValue;
            }
            set
            {
                HasNewSortingSearch = !(value.label == SortingValuesSearch.label);
                OldValue = value;
            }
        }

    }
}
