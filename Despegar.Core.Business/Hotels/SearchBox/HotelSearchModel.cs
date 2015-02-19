using Despegar.Core.Business.CustomErrors;
using Despegar.WP.UI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Business.Hotels.SearchBox
{
    public class HotelSearchModel : BusinessModelBase
    {

        public int DestinationCode { get; set; }
        public string DestinationHotelText { get; set; }
        public int AdultsInFlights { get; set; }
        public int ChildrenInFlights { get; set; }
        public DateTimeOffset DepartureDate { get; set; }
        public DateTimeOffset DestinationDate { get; set; }
        public int EmissionAnticipationDay { get; set; }
        public int LastAvailableHours { get; set; }
        private DateTime DateBoundary { get; set; }
        public CustomError SearchErrors { get; set; }

        public HotelSearchModel()
        {
            this.AdultsInFlights = 1;
            this.ChildrenInFlights = 0;
            this.DestinationCode = 0;
            this.DestinationHotelText = string.Empty;

            this.DepartureDate = DateTime.Today.AddDays(EmissionAnticipationDay);
            this.DestinationDate = DateTime.Today.AddDays(EmissionAnticipationDay);
        }

        public override bool Validate()
        {
            return IsValid;
        }

        public override bool IsValid 
        { 
            get{

                if (String.IsNullOrEmpty(DestinationHotelText))
                {
                    // No origin
                    SearchErrors = new CustomError("Debe que seleccionar destino.", "FLIGHT_SEARCH_NO_ORIGIN_ERROR_MESSAGE", "CommonValidations");
                    return false;
                }

                if (AdultsInFlights <= 0)
                {
                    SearchErrors = new CustomError("Tiene que haber al menos un adulto.", "FLIGHT_SEARCH_MIN_ADULTS_ERROR_MESSAGE", "CommonValidations");
                    // Adult obligatory
                    return false;
                }
                if (DestinationDate <= DepartureDate)
                {
                    SearchErrors = new CustomError("fecha partida menor a llegada.", "FLIGHT_SEARCH_DESTINATIONDATE_SMALLER_THAN_DEPARTURE_ERROR_MESSAGE", "isValid");
                    return false;
                }

                if (DepartureDate < DateBoundary)
                {
                    // Cannot fly in the past.
                    SearchErrors = new CustomError(string.Empty, "FLIGHT_SEARCH_DEPARTUREDATE_SMALLER_THAN_ERROR_MESSAGE", "isValid", true, DateBoundary.ToString("dd-MM-yyyy"));
                    return false;
                }

                if (DestinationDate < DateBoundary)
                {
                    // Cannot fly in the past.
                    SearchErrors = new CustomError(string.Empty, "FLIGHT_SEARCH_DEPARTUREDATE_SMALLER_THAN_ERROR_MESSAGE", "isValid", true, DateBoundary.ToString("dd-MM-yyyy"));
                    return false;
                }

                return true;
            
            }
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
            this.DestinationDate = daysToAdd.AddDays(1);

            DateBoundary = daysToAdd;
        }


    }
}
