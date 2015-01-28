using Despegar.Core.Business.Hotels.HotelDetails;
using Despegar.Core.IService;
using Despegar.Core.Log;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Model.ViewModel.Controls.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Despegar.WP.UI.Model.ViewModel.Hotels
{
    public class HotelsDetailsViewModel : ViewModelBase
    {
        public INavigator navigator { get; set; }
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
            this.navigator = navigator;
            this.hotelService = hotelService;
        }

        private CustomMapViewModel _customMap;

        public CustomMapViewModel CustomMap 
        {
            get {
                    if (_customMap == null) _customMap = new CustomMapViewModel();
                    
                    if (hotelDetail!=null && hotelDetail.hotel.geo_location!=null)
                    {
                        Classes.CustomPinPoint pinpoint = new Classes.CustomPinPoint() { Latitude = hotelDetail.hotel.geo_location.latitude, Longitude = hotelDetail.hotel.geo_location.longitude, Title=hotelDetail.hotel.name, Address = hotelDetail.hotel.address};
                        _customMap.Locations.Add(pinpoint);
                    }

                    return _customMap; 
                }
        }

        public async Task Init()
        {
            HotelDetail = await hotelService.GetHotelsDetail(CrossParameters.IdSelectedHotel, CrossParameters.SearchParameters.Checkin, CrossParameters.SearchParameters.Checkout, CrossParameters.SearchParameters.distribution);
        }
    }
}
