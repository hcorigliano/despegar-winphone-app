using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Despegar.Core.Business.Flight.Itineraries;
using Despegar.Core.Business.Enums;
using Despegar.WP.UI.Model;

namespace Despegar.Core.Business.Flight
{
    public class FlightSearchModel : BusinessModelBase
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

        //auxiliar
        public string MultipleDates { get; set; }

        public FlightSearchModel()
        {
            this.InitializeModel();
        }

        public override void InitializeModel()
        {
            base.InitializeModel();
            //TODO uncomment following code for advance search
            //this.AdvanceSearch = new AdvanceSearchModel();
            this.AdultsInFlights = 1;
            this.ChildrenInFlights = 0;
            this.InfantsInFlights = 0;

            this.OriginFlight = string.Empty;
            this.DestinationFlight = string.Empty;

            this.DepartureDate = DateTime.Today;
            this.DestinationDate = DateTime.Today;

            //TODO these values are going to be handle next interation
            this.LimitResult = 20;
            this.Offset = 0;
        }

        public override void Validate()
        {
            IsValid();
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
                    serviceUrl = ""; 
                    // TODO  
                    //coreSearchModel.MultipleDates = String.Join(",", segmentMultipleStackPanel.Children.Select(x => ((FlightsSection)x).DateControlContainerMultipleSection.DateDeparture.Date.ToString("yyyy-MM-dd")).ToList());
                    //coreSearchModel.OriginFlight = String.Join(",", segmentMultipleStackPanel.Children.Select(x => ((FlightsSection)x).AirportsContainerMultipleSection.AirportOrigin).ToList());
                    //coreSearchModel.DestinationFlight = String.Join(",", segmentMultipleStackPanel.Children.Select(x => ((FlightsSection)x).AirportsContainerMultipleSection.AirportDestiny).ToList()); 
                    break;
            }

            return serviceUrl;
        }

        // TODO!!
        public bool IsValid()
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
