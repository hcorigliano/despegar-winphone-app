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
        }

        public string Destination
        {
            get { return coreSearchModel.DestinationHotel; }
            set
            {
                coreSearchModel.DestinationHotel = value;
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
                
            }
            return distribution;
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

        private async void SearchHotels()
        {
            
            
            HotelsCrossParameters hotelCrossParameters = new HotelsCrossParameters();
            hotelCrossParameters.SearchParameters.distribution = "2";
            hotelCrossParameters.SearchParameters.Checkin = "2015-03-01";
            hotelCrossParameters.SearchParameters.Checkout = "2015-03-05";
            hotelCrossParameters.SearchParameters.currency = "ars";
            hotelCrossParameters.SearchParameters.destinationNumber = 4451; //982

            //int child = hotelCrossParameters.SearchParameters.Childs ;
            Navigator.GoTo(ViewModelPages.HotelsResults, hotelCrossParameters);

            //CitiesAvailability cities = await hotelService.GetHotelsAvailability("2015-03-01", "2015-03-05", 982, "2", "ars", 0, 30, "",""); //982 , "2015-03-01" , "2015-03-05" , "2" , "ars" , 0 , 30 , "" , "" );
            //cities.searchDetails =  new HotelsSearchParameters();
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
