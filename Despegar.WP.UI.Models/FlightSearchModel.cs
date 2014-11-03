using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Despegar.WP.UI.Model.Classes;
using Despegar.WP.UI.Model.Enums;

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

            return flightSearchBoxModel.GetItineraries(this.OriginFlight, this.DestinationFlight, DateConverterDeparture(this.DepartureDate), this.AdultsInFlights, DateConverterDestination(this.DestinationDate), this.ChildrenInFlights, this.InfantsInFlights, this.Offset, this.LimitResult, "", "", "", "");
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
    }
}
