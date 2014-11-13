using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Despegar.WP.UI.Model.Classes;
using Despegar.WP.UI.Model.Enums;
using Despegar.Core.Business.Flight.Itineraries;

namespace Despegar.WP.UI.Model
{
    public class FlightSearchModel : AppModelBase , Interfaces.IValidateInterface ,Interfaces.IInitializeModelInterface
    {

        public DateTimeOffset DepartureDate { get; set; }
        public DateTimeOffset DestinationDate { get; set; }
        public string OriginFlight { get; set; }
        public string DestinationFlight { get; set; }
        public FlightSearchPages PageMode { get; set; }
        public int AdultsInFlights { get; set; }
        public int ChildrenInFlights { get; set; }
        public int InfantsInFlights { get; set; }
        
        public int LimitResult { get; set; }
        public int Offset { get; set; }

        public int TotalPassangers {
                get {
                    
                    return AdultsInFlights+ChildrenInFlights+InfantsInFlights;
                }
            }
        public int TotalFlights { get; set; }

        public List<Facet> FacetsSearch { get; set; }
        private Value3 OldValue { get; set; }
        public Value3 SortingValuesSearch { 
            get
            {
                if (OldValue == null) return new Value3 { label=String.Empty , type = String.Empty , value = String.Empty };
                return OldValue;
            } 
            set
            {
                HasNewSortingSearch = !(value.label == SortingValuesSearch.label);
                OldValue = value;
            } 
        }
        public bool HasNewSortingSearch { get; set;}

        public string SortingCriteriaSearch { get; set; }

        public SearchStates SearchStatus { get; set; }

        //auxiliar
        public string MultipleDates { get; set; }

        //public AdvanceSearchModel AdvanceSearch { get; set; }

        public FlightSearchModel()
        {
            this.InitializeModel();
        }

        public new void InitializeModel()
        {
            base.InitializeModel();
            //TODO uncomment following code for advance search
            //this.AdvanceSearch = new AdvanceSearchModel();

            this.OriginFlight = string.Empty;
            this.DestinationFlight = string.Empty;

            //TODO these values are going to be handle next interation
            this.LimitResult = 20;
            this.Offset = 0;
        }

        public new void Validate()
        {
            //throw new NotImplementedException();
        }


        public Task<Core.Business.Flight.Itineraries.FlightsItineraries> Search()
        {
            FlightsSearchBoxModel flightSearchBoxModel = new FlightsSearchBoxModel();
            //flightSearchBoxModel.GetItineraries("BUE", "LAX", "2014-12-11", 1, "2014-12-13", 0, 0, 0, 10, "", "", "", "");

            return flightSearchBoxModel.GetItineraries(this.OriginFlight, this.DestinationFlight, DateConverterDeparture(this.DepartureDate), this.AdultsInFlights, DateConverterDestination(this.DestinationDate), this.ChildrenInFlights, this.InfantsInFlights, this.Offset, this.LimitResult, SortingValuesSearch.value, SortingValuesSearch.type, "", this.FacetsCodes);
        }


        private string DateConverterDeparture(DateTimeOffset datetime)
        {
            switch (this.PageMode)
            {
                case FlightSearchPages.RoundTrip:
                case FlightSearchPages.OneWay:
                    return datetime.ToString("yyyy-MM-dd");
                case FlightSearchPages.Multiple:
                    return this.MultipleDates;
                default:
                    return String.Empty;
            }
        }

        private string DateConverterDestination(DateTimeOffset datetime)
        {
            switch (this.PageMode)
            {
                case FlightSearchPages.RoundTrip:
                    return datetime.ToString("yyyy-MM-dd");
                
                case FlightSearchPages.Multiple:
                case FlightSearchPages.OneWay:
                default:
                    return String.Empty;
            }
        }

        public bool isValid()
        {

            switch (this.PageMode)
            {
                case FlightSearchPages.RoundTrip:
                    {
                        if (String.IsNullOrEmpty(OriginFlight)) return false;

                        if (PageMode == FlightSearchPages.RoundTrip)
                        {
                            if (String.IsNullOrEmpty(DestinationFlight)) return false;
                        }

                        if (AdultsInFlights <= 0) return false;
                    }
                    break;

                case FlightSearchPages.OneWay:
                case FlightSearchPages.Multiple:
                    return true;

                default:
                    return false;
            }

            return true;
        }

        public bool HasFacetsSelected
        {
            get
            {
                if (FacetsSearch != null)
                {
                    return FacetsSearch.Count > 0;
                }
                return false;
            }
        }

        public string FacetsCodes
        {
            get
            {
                // TODO create array separatated "," for webservice call

                // var element = from x in FacetsSearch
                // var codes = from s in  FacetsSearch 
                //            select  new stri}

                // for each facet check the list of value on true
                // get the string array then create a array of separeted by comma

                List<String> facetListNames = new List<string>();
                if (FacetsSearch == null) return String.Empty;

                foreach (Facet facet in FacetsSearch)
                {
                    var elements = facet.values.Where(x=>x.selected==true);

                    var response = (from value in elements
                                     select value.value);

                    if (response != null)
                    {
                        facetListNames.AddRange(response.ToList());
                    }
                }

                if (facetListNames.Count == 0) return String.Empty;

                return String.Join(",",facetListNames);
            }
        }
    }
}
