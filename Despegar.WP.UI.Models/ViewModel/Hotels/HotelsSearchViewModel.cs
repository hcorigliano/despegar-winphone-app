using Despegar.Core.Business.Hotels;
using Despegar.Core.Business.Hotels.CitiesAvailability;
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
        public INavigator navigator { get; set; }
        public IHotelService hotelService { get; set; }


        public HotelsSearchViewModel(INavigator navigator, IHotelService hotelService, IBugTracker t) : base(t)
        {
            this.navigator = navigator;
            this.hotelService = hotelService;
        }

        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(() => SearchHotels());
            }
        }

        public ICommand GotoPage
        {
            get
            {
                return new RelayCommand(() => navigator.GoTo(ViewModelPages.HotelsDetails, null));
            }
        }


        private async void SearchHotels()
        {
            CitiesAvailability cities = await hotelService.GetHotelsAvailability("2015-03-01", "2015-03-05", 982, "2", "ars", 0, 30, ""); //982 , "2015-03-01" , "2015-03-05" , "2" , "ars" , 0 , 30 , "" , "" );
            cities.searchDetails =  new SearchDetails();
            cities.searchDetails.Adults = 1;
            cities.searchDetails.Checkin = "2015-03-01";
            cities.searchDetails.Checkout = "2015-03-05";
            cities.searchDetails.Childs = 3;
            cities.searchDetails.Rooms = 2;
            navigator.GoTo(ViewModelPages.HotelsResults, cities);
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
