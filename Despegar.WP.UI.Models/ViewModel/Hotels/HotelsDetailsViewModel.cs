using Despegar.Core.Business.Hotels.BookingFields;
using Despegar.Core.Business.Hotels.HotelDetails;
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
    public class HotelsDetailsViewModel : ViewModelBase
    {
        public INavigator Navigator { get; set; }
        public IHotelService hotelService { get; set; }
        public HotelsCrossParameters CrossParameters { get; set; }

        private HotelDatails hotelDetail { get; set; }
        public HotelDatails HotelDetail
        {
            get
            {
                return hotelDetail;
            }

            set
            {
                hotelDetail = value;
                OnPropertyChanged();
            }
        }
        public ICollection<string> ImagesTestList { get; set; }

        public HotelsDetailsViewModel(INavigator navigator, IHotelService hotelService, IBugTracker t)
            : base(t)
        {
            this.Navigator = navigator;
            this.hotelService = hotelService;

        }

        public ICommand BuySuggestRoomCommand
        {
            get
            {
                return new RelayCommand(() => BuySuggestRoom());
            }
        }

        private void BuySuggestRoom()
        {
            if (CrossParameters != null && hotelDetail != null)
            {
                CrossParameters.BookRequest = new HotelsBookingFieldsRequest();
                CrossParameters.BookRequest.token = HotelDetail.token;
                CrossParameters.BookRequest.hotel_id = hotelDetail.id;
                CrossParameters.BookRequest.room_choices = new List<string>() { hotelDetail.suggested_room_choice };
                CrossParameters.BookRequest.mobile_identifier = GlobalConfiguration.UPAId;

                Navigator.GoTo(ViewModelPages.HotelsCheckout, CrossParameters);
            }
        }

        public async Task Init()
        {
            HotelDetail = await hotelService.GetHotelsDetail(CrossParameters.IdSelectedHotel, CrossParameters.SearchParameters.Checkin, CrossParameters.SearchParameters.Checkout, CrossParameters.SearchParameters.distribution);
        }
    }
}
