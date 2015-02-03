using Despegar.Core.Business.Hotels;
using Despegar.Core.Business.Hotels.CitiesAvailability;
using Despegar.Core.Business.Hotels.SearchBox;
using Despegar.Core.IService;
using Despegar.Core.Log;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Despegar.WP.UI.Model.ViewModel.Hotels
{
    public class HotelsSearchViewModel : ViewModelBase
    {
        public INavigator Navigator { get; set; }
        public IHotelService hotelService { get; set; }
        private HotelSearchModel coreSearchModel;
        public RoomsViewModel RoomsViewModel { get; set; }

        public HotelsSearchViewModel(INavigator navigator, IHotelService hotelService, IBugTracker t) : base(t)
        {
            this.Navigator = navigator;
            this.hotelService = hotelService;
            this.coreSearchModel = new HotelSearchModel();
            this.RoomsViewModel = new RoomsViewModel(t);
            this.coreSearchModel.EmissionAnticipationDay = GlobalConfiguration.GetEmissionAnticipationDayForHotels();
            this.coreSearchModel.LastAvailableHours = GlobalConfiguration.GetLastAvailableHoursForHotels();
            this.DestinationType = string.Empty;
            coreSearchModel.UpdateSearchDays();
        }

        public int DestinationCode
        {
            get { return coreSearchModel.DestinationCode; }
            set
            {
                coreSearchModel.DestinationCode = value;
                OnPropertyChanged();
            }
        }


        public string DestinationType{get;set;}

        public string DestinationText
        {
            get { return coreSearchModel.DestinationHotelText; }
            set
            {
                coreSearchModel.DestinationHotelText = value;
                OnPropertyChanged();
            }
        }

        public DateTimeOffset FromDate
        {
            get { return coreSearchModel.DepartureDate; }
            set
            {
                coreSearchModel.DepartureDate = value;
                OnPropertyChanged();
        }
        }

        public DateTimeOffset ToDate
        {
            get { return coreSearchModel.DestinationDate; }
            set
            {
                coreSearchModel.DestinationDate = value;
                OnPropertyChanged();
            }
        }

        private string BuildDistributionString()
        {
            string distribution = String.Empty;

            foreach (PassengersForRooms room in this.RoomsViewModel.RoomsDetailList)
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

        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(() => SearchHotels());
            }
        }

        /// <summary>
        /// Returns the available options for Adults passengers
        /// </summary>
        public IEnumerable<int> RoomOptions
        {
            get
            {
                List<int> options = new List<int>();

                // 1 is the Minimum Adult count
                for (int i = 1; i <= 8 ; i++)
                    options.Add(i);

                return options;
            }
        }

        private void SearchHotels()
        {

            UpdatePassengers();

            if (coreSearchModel.IsValid)
            {
               // IsLoading = true;
                Tracker.LeaveBreadcrumb("Flight search performed");

                HotelsCrossParameters hotelCrossParameters = new HotelsCrossParameters();
                hotelCrossParameters.SearchParameters.distribution = BuildDistributionString();
                hotelCrossParameters.SearchParameters.Checkin = coreSearchModel.DepartureDate.Date.ToString("yyyy-MM-dd");
                hotelCrossParameters.SearchParameters.Checkout = coreSearchModel.DestinationDate.Date.ToString("yyyy-MM-dd");
                hotelCrossParameters.SearchParameters.destinationNumber = coreSearchModel.DestinationCode;

                if (this.DestinationType == "city")
                {
                    Navigator.GoTo(ViewModelPages.HotelsResults, hotelCrossParameters);
                }
                else
                {
                    Navigator.GoTo(ViewModelPages.HotelsDetails, hotelCrossParameters);
                }
            }
            else
            {
                OnViewModelError("SEARCH_INVALID_WITH_MESSAGE", coreSearchModel.SearchErrors);
            }
        }


        private void UpdatePassengers()
        {
            coreSearchModel.AdultsInFlights = RoomsViewModel.Adults;
            coreSearchModel.ChildrenInFlights = RoomsViewModel.Minors;
        }

        private string _Checkin;
        public string Checkin
        {
            get { return  _Checkin; }
            set
            {
                _Checkin = value;
                OnPropertyChanged();
            }
        }



    }
}
