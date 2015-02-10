using Despegar.Core.Neo.Business.Hotels.SearchBox;
using Despegar.Core.Neo.Contract.API;
using Despegar.Core.Neo.Contract.Log;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Models.Classes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Geolocation;

namespace Despegar.WP.UI.Model.ViewModel.Hotels
{
    public class HotelsSearchViewModel : ViewModelBase
    {
        public IMAPIHotels hotelService { get; set; }
        private HotelSearchModel coreSearchModel;
        public RoomsViewModel RoomsViewModel { get; set; }
        private Geolocator geolocator = null;       

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

        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(() => SearchHotels());
            }
        }

        public ICommand GetPositionCommand
        {
            get
            {
                return new RelayCommand(() => GetPosition());
            }
        }

        private string checkin;
        public string Checkin
        {
            get { return checkin; }
            set
            {
                checkin = value;
                OnPropertyChanged();
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
                for (int i = 1; i <= 8; i++)
                    options.Add(i);

                return options;
            }
        }

        public HotelsSearchViewModel(INavigator navigator,  IBugTracker t, IMAPIHotels hotelService ) : base(navigator,t)
        {
            this.hotelService = hotelService;
            this.coreSearchModel = new HotelSearchModel();
            this.RoomsViewModel = new RoomsViewModel(); // what
            this.geolocator = new Geolocator();  // Dependency?

            this.coreSearchModel.EmissionAnticipationDay = GlobalConfiguration.GetEmissionAnticipationDayForHotels();
            this.coreSearchModel.LastAvailableHours = GlobalConfiguration.GetLastAvailableHoursForHotels();
            this.DestinationType = string.Empty;            
            coreSearchModel.UpdateSearchDays();
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

        private async void GetPosition()
        {
            try
            {
                Geoposition pos = await geolocator.GetGeopositionAsync();

                HotelsCrossParameters hotelCrossParameters = new HotelsCrossParameters();
                hotelCrossParameters.SearchParameters.distribution = "2";
                hotelCrossParameters.SearchParameters.Checkin = DateTime.Now.ToString("yyyy-MM-dd");
                hotelCrossParameters.SearchParameters.Checkout = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                hotelCrossParameters.SearchParameters.latitude = pos.Coordinate.Point.Position.Latitude;
                hotelCrossParameters.SearchParameters.longitude = pos.Coordinate.Point.Position.Longitude;

                Navigator.GoTo(ViewModelPages.HotelsResults, hotelCrossParameters);


            }
            catch (System.UnauthorizedAccessException)
            {
                
                throw;
            }
            catch (TaskCanceledException)
            {

            }

        }

        private async void SearchHotels()
        {
            UpdatePassengers();

            if (coreSearchModel.IsValid)
            {
               // IsLoading = true;
                BugTracker.LeaveBreadcrumb("Hotel search performed");

                HotelsCrossParameters hotelCrossParameters = new HotelsCrossParameters();
                hotelCrossParameters.SearchParameters.distribution = BuildDistributionString();
                hotelCrossParameters.SearchParameters.Checkin = coreSearchModel.DepartureDate.Date.ToString("yyyy-MM-dd");
                hotelCrossParameters.SearchParameters.Checkout = coreSearchModel.DestinationDate.Date.ToString("yyyy-MM-dd");
               
                if (coreSearchModel.DestinationCode != 0)
                {
                    hotelCrossParameters.SearchParameters.destinationNumber = coreSearchModel.DestinationCode;
                }
                else
                {
                    Geoposition pos = await geolocator.GetGeopositionAsync();
                    hotelCrossParameters.SearchParameters.latitude = pos.Coordinate.Point.Position.Latitude;
                    hotelCrossParameters.SearchParameters.longitude = pos.Coordinate.Point.Position.Longitude;
                }

                if (this.DestinationType == "city" || this.DestinationType == "geo" )
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

    }
}
