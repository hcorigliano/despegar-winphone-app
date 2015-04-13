using Despegar.Core.Neo.Business.Hotels.CitiesAvailability;
using Despegar.Core.Neo.Business.Hotels.SearchBox;
using Despegar.Core.Neo.Contract.API;
using Despegar.Core.Neo.Contract.Log;
using Despegar.WP.UI.Model.Classes;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Model.ViewModel.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Resources;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;


namespace Despegar.WP.UI.Model.ViewModel.Hotels
{
    public class HotelsSearchViewModel : ViewModelBase
    {
        public IMAPIHotels hotelService { get; set; }
        private HotelSearchModel coreSearchModel;
        private Geolocator geolocator = null;
        private IGoogleAnalytics analyticsService;
        private const int RESULTS_PAGE_SIZE = 30;

        public string DestinationType { get; set; }
        public int DestinationCode
        {
            get { return coreSearchModel.DestinationCode; }
            set
            {
                coreSearchModel.DestinationCode = value;
                OnPropertyChanged();
            }
        }
        public string DestinationText
        {
            get { return coreSearchModel.DestinationHotelText; }
            set
            {
                coreSearchModel.DestinationHotelText = value;
                OnPropertyChanged();
            }
        }
        public DateTimeOffset CheckinDate
        {
            get { return coreSearchModel.CheckinDate; }
            set
            {
                coreSearchModel.CheckinDate = value;
                if (DateTimeOffset.Compare(CheckinDate, CheckoutDate) >= 0 )
                    CheckoutDate = value.AddDays(1);
                
                OnPropertyChanged();
            }
        }
        public DateTimeOffset CheckoutDate
        {
            get { return coreSearchModel.CheckoutDate; }
            set
            {
                coreSearchModel.CheckoutDate = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Rooms & Passengers
        /// </summary>
        public IEnumerable<int> RoomQuantityOptions
        {
            get
            {
                return coreSearchModel.RoomQuantityOptions;
            }
        }
        public int SelectedRoomsQuantityOption 
        {
            get { return coreSearchModel.SelectedRoomsQuantityOption; }
            set { coreSearchModel.SelectedRoomsQuantityOption = value; }
        }
        public ObservableCollection<PassengersForRooms> Rooms
        {
            get { return coreSearchModel.Rooms; }           
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
                return new RelayCommand(() => SearchTodayHotels());
            }
        }

        public HotelsSearchViewModel(INavigator navigator, IBugTracker t, IMAPIHotels hotelService, IGoogleAnalytics analyticsService)
            : base(navigator, t)
        {
            this.hotelService = hotelService;
            this.coreSearchModel = new HotelSearchModel();
            this.coreSearchModel.Limit = RESULTS_PAGE_SIZE;
            this.geolocator = new Geolocator();  // Dependency?

            this.coreSearchModel.EmissionAnticipationDay = GlobalConfiguration.GetEmissionAnticipationDayForHotels();
            this.coreSearchModel.LastAvailableHours = GlobalConfiguration.GetLastAvailableHoursForHotels();
            this.DestinationType = string.Empty;
            this.analyticsService = analyticsService;
            coreSearchModel.UpdateSearchDays();
        }

        private async void SearchTodayHotels()
        {
            if (geolocator.LocationStatus != PositionStatus.Disabled)
            {
                CheckinDate = DateTime.Now;
                CheckoutDate = DateTime.Now.AddDays(1);
                coreSearchModel.CheckinDate = CheckinDate;
                coreSearchModel.CheckoutDate = CheckoutDate;
                coreSearchModel.SelectedRoomsQuantityOption = 1;
                coreSearchModel.DestinationCode = -1; // TODO: mejorar a algo como IsGeoSearch = true

                coreSearchModel.Rooms[0].GeneralAdults = 1;
                coreSearchModel.Rooms[0].GeneralMinors = 0;

                await SearchHotels();
            }
            else
            {
                ResourceLoader manager = new ResourceLoader();
                MessageDialog dialog = new MessageDialog(manager.GetString("Hotel_Gps_Error"), "Error");
                await dialog.ShowAsync();
                return;
            }

        }

        private async Task SearchHotels()
        {
            if (coreSearchModel.IsValid)
            {
               // IsLoading = true;
                BugTracker.LeaveBreadcrumb("Hotel search performed");                
               
                //Reset facet and sorting
                coreSearchModel.Facets = new List<Facet>();
                coreSearchModel.Sortings = new Sorting();

                if (coreSearchModel.DestinationCode == -1)                
                {
                  
                    // Geolocation  search
                    this.DestinationType = "geo";

                    // TODO: Test this better
                    try
                    {
                        IsLoading = true;
                        Geoposition pos = await geolocator.GetGeopositionAsync();
                        coreSearchModel.Latitude = pos.Coordinate.Point.Position.Latitude;
                        coreSearchModel.Longitude = pos.Coordinate.Point.Position.Longitude;
                    }
                    catch (System.UnauthorizedAccessException)
                    {
                        // TODO: Pedirle que active el gps?

                        throw;
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    IsLoading = false;    

                }

                if (this.DestinationType == "city" || this.DestinationType == "geo")
                {
                    // Location search
                    Navigator.GoTo(ViewModelPages.HotelsResults, new GenericResultNavigationData() { SearchModel = coreSearchModel, FiltersApplied = false });
                }
                else
                {
                    // The user searched directly for a specific hotel
                    HotelsCrossParameters hotelCrossParameters = new HotelsCrossParameters();
                    hotelCrossParameters.SearchModel = this.coreSearchModel;
                    hotelCrossParameters.IdSelectedHotel = coreSearchModel.DestinationCode.ToString();
                    Navigator.GoTo(ViewModelPages.HotelsDetails, hotelCrossParameters);
                }
            }
            else
            {
                OnViewModelError("SEARCH_INVALID_WITH_MESSAGE", coreSearchModel.SearchErrors);
            }
        }

        public override void OnNavigated(object navigationParams)
        {
            analyticsService.SendView("HotelsCheckout");
        }
    }
}