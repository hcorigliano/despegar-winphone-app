using Despegar.Core.Neo.Business.CustomErrors;
using Despegar.Core.Neo.Business.Hotels.CitiesAvailability;
using Despegar.WP.UI.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Business.Hotels.SearchBox
{
    public class HotelSearchModel : BusinessModelBase
    {
        public string Currency { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
        public string Order { get; set; }       
        public int DestinationCode { get; set; }
        public string DestinationHotelText { get; set; }
        public DateTimeOffset CheckinDate { get; set; }
        public DateTimeOffset CheckoutDate { get; set; }
        public int EmissionAnticipationDay { get; set; }
        public int LastAvailableHours { get; set; }
        private DateTime DateBoundary { get; set; }
        public CustomError SearchErrors { get; set; }

        // Filters formatting
        public string ExtraParameters
        {
            get
            {
                string extraParameters = String.Empty;

                // Facets
                foreach (Facet facet in Facets)
                {
                    if (facet.values != null)
                    {
                        string valueString = "";

                        foreach (Value value in facet.values)
                        {
                            if (value.selected)
                            {
                                if (valueString == "")
                                    valueString += value.value;
                                else
                                    valueString += "," + value.value;
                            }
                        }

                        if (valueString != String.Empty)
                            extraParameters += facet.criteria + "=" + valueString + "&";
                    }
                }

                // Sortings
                if (Sortings.values != null) 
                { 
                    foreach (Value value in Sortings.values)
                    {
                        if (value.selected)
                            extraParameters += "order_by=" + value.value;
                    }
                }

                return extraParameters;
            }
        }

        // Rooms and Passengers Model
        public ObservableCollection<PassengersForRooms> Rooms { get; set; }
        public IEnumerable<int> RoomQuantityOptions
        {
            get
            {
                return Enumerable.Range(1, 4);
            }
        }
        private int selectedRoomsQuantityOption;
        public int SelectedRoomsQuantityOption
        {
            get { return selectedRoomsQuantityOption; }
            set
            {
                if (selectedRoomsQuantityOption != value)
                {
                    if (Rooms.Count < value)
                    {
                        // More rooms!
                        for (int i = Rooms.Count; i < value; i++)
                            Rooms.Add(new PassengersForRooms() { Index = i + 1 });
                    }
                    else
                    {
                        // Less rooms!
                        for (int i = Rooms.Count - 1; i >= value; i--)
                            Rooms.RemoveAt(i);
                    }
                    selectedRoomsQuantityOption = value;
                    OnPropertyChanged();
                }
            }
        }

        public int TotalAdults
        {
            get
            {
                return Rooms.Sum(x => x.GeneralAdults);
            }
        }

        public int TotalMinors
        {
            get
            {
                return Rooms.Sum(x => x.GeneralMinors);
            }
        }

        public int Nights
        {
            get
            {
                if (CheckinDate != null && CheckoutDate != null)
                {
                    return (CheckoutDate - CheckinDate).Days;
                }

                return 0;
            }
        }
               
        /// <summary>
        /// Geo Data
        /// </summary>
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        /// <summary>
        /// Selected facets
        /// </summary>
        public List<Facet> Facets { get; set; }
        public Sorting Sortings { get; set; }

        public string DistributionString 
        { 
           get 
           {
              string distribution = String.Empty;

              foreach (PassengersForRooms room in this.Rooms)
              {
                  distribution += room.GeneralAdults.ToString();
                  foreach (HotelsMinorsAge minor in room.MinorsAge)
                  {
                      distribution += "-" + minor.SelectedAge.ToString();
                  }
                  distribution += "!";
              }

              return distribution.Remove(distribution.Length - 1);
          } 
        }

        public string DepartureDateFormatted
        {
            get
            {
                return this.CheckinDate.Date.ToString("yyyy-MM-dd");
            }
        }

        public string DestinationDateFormatted
        {
            get
            {
                return this.CheckoutDate.Date.ToString("yyyy-MM-dd");
            }
        }

        public HotelSearchModel()
        {            
            this.DestinationCode = 0;
            this.DestinationHotelText = string.Empty;

            this.CheckinDate = DateTime.Today.AddDays(EmissionAnticipationDay);
            this.CheckoutDate = DateTime.Today.AddDays(EmissionAnticipationDay);
            this.Facets = new List<Facet>();
            this.Sortings = new Sorting();
            this.Rooms = new ObservableCollection<PassengersForRooms>();
            this.SelectedRoomsQuantityOption = 1;
            this.Rooms[0].GeneralAdults = 1;
        }

        public override bool Validate()
        {
            return IsValid;
        }

        public override bool IsValid 
        { 
            get
            {
                if (String.IsNullOrEmpty(DestinationHotelText) && DestinationCode != -1)
                {
                    // No origin
                    SearchErrors = new CustomError(String.Empty, "HOTEL_SEARCH_NO_DESTINATION_ERROR_MESSAGE", "CommonValidations");
                    return false;
                }

                if (CheckoutDate <= CheckinDate)
                {
                    SearchErrors = new CustomError(String.Empty, "HOTEL_SEARCH_CHECKOUT_DATE_SMALLER_THAN_CHECKIN_ERROR_MESSAGE", "isValid");
                    return false;
                }

                if (CheckinDate < DateBoundary)
                {
                    // Cannot fly in the past.
                    SearchErrors = new CustomError(String.Empty, "HOTEL_SEARCH_CHECKOUT_SMALLER_THAN_CI_ERROR_MESSAGE", "isValid", true, DateBoundary.ToString("dd-MM-yyyy"));
                    return false;
                }

                if (CheckoutDate < DateBoundary)
                {
                    // Cannot fly in the past.
                    SearchErrors = new CustomError(String.Empty, "HOTEL_SEARCH_CHECKOUT_SMALLER_THAN_CI_ERROR_MESSAGE", "isValid", true, DateBoundary.ToString("dd-MM-yyyy"));
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
            this.CheckinDate = daysToAdd;
            this.CheckoutDate = daysToAdd.AddDays(1);

            DateBoundary = daysToAdd;
        }

        public void NotifyPropertiesChanged()
        {
            OnPropertyChanged("DepartureDateFormatted");
            OnPropertyChanged("DestinationDateFormatted");
            OnPropertyChanged("Nights");
            OnPropertyChanged("SelectedRoomsQuantityOption");
            OnPropertyChanged("TotalAdults");
            OnPropertyChanged("TotalMinors");
        }
        
    }
}