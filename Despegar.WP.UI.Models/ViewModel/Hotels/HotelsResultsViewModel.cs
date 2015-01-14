using Despegar.Core.Business.Hotels.CitiesAvailability;
using Despegar.Core.IService;
using Despegar.Core.Log;
using Despegar.WP.UI.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model.ViewModel.Hotels
{

    public class HotelsResultsViewModel : ViewModelBase
    {
        public INavigator navigator { get; set; }
        public IHotelService hotelService { get; set; }
        public CitiesAvailability citiesAvailability { get; set; }
        public string reSearchTextRoomAndNights { get; set; }


        public HotelsResultsViewModel(INavigator navigator, IHotelService hotelService, IBugTracker t)
            : base(t)
        {
            this.navigator = navigator;
            this.hotelService = hotelService;
        }

        public void init()
        {
            
            if(citiesAvailability.searchDetails.Rooms == 1)
                reSearchTextRoomAndNights = citiesAvailability.searchDetails.Rooms.ToString() + " habitación - ";
            else
                reSearchTextRoomAndNights = citiesAvailability.searchDetails.Rooms.ToString() + " habitaciónes - ";

            if (citiesAvailability.searchDetails.Nights == 1)
                reSearchTextRoomAndNights += citiesAvailability.searchDetails.Nights.ToString() + " noche";
            else
                reSearchTextRoomAndNights += citiesAvailability.searchDetails.Nights.ToString() + " noches";
        }

    }


}
