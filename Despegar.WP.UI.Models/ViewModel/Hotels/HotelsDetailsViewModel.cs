﻿using Despegar.Core.Business.Hotels;
using Despegar.Core.Business.Hotels.CustomUserReviews;
using Despegar.Core.Business.Hotels.HotelDetails;
using Despegar.Core.Business.Hotels.UserReviews;
using Despegar.Core.IService;
using Despegar.Core.Log;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Model.ViewModel.Controls.Maps;
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
        #region ** Public Interface **
        public INavigator Navigator { get; set; }
        public IHotelService hotelService { get; set; }
        public HotelsCrossParameters CrossParameters { get; set; }

        public HotelUserReviews HotelReviews { get; set;}

        private List<CustomReviewsItem> customReviews { get; set; }
        public List<CustomReviewsItem> CustomReviews
        {
            get
            {
                return customReviews;
            }
            set
            {
                customReviews = value;
                OnPropertyChanged();
            }
        }

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
                OnPropertyChanged("SuggestRoomPriceBase");
                OnPropertyChanged("SuggestRoomPriceBest");
            }
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

        private int? suggestRoomPriceBase { get; set; }
        public int? SuggestRoomPriceBase
        {
            get
            {
                return suggestRoomPriceBase;
            }

            set
            {
                suggestRoomPriceBase = value;
                OnPropertyChanged();
            }
        }

        private int? suggestRoomPriceBest { get; set; }
        public int? SuggestRoomPriceBest
        {
            get
            {
                return suggestRoomPriceBest;
            }

            set
            {
                suggestRoomPriceBest = value;
                OnPropertyChanged();
            }
        }


        private int goToPivot { get; set; }
        public int GoToPivot
        {
            get
            {
                return goToPivot; 
            }
            set
            {
                goToPivot = value;
                OnPropertyChanged();
            }
        }

        #endregion

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
            HotelReviews = await hotelService.GetHotelUserReviews(CrossParameters.IdSelectedHotel, 10, 0, "es");
            FormatReviews("es");

            //Gets suggest room price
            foreach (Roompack roomPack in hotelDetail.roompacks)
            {
                foreach (RoomAvailability room in roomPack.room_availabilities)
                {
                    if (room.choices.Contains(hotelDetail.suggested_room_choice))
                    {
                        SuggestRoomPriceBest = room.price.best;

                        if (SuggestRoomPriceBest != room.price.@base)
                            SuggestRoomPriceBase = room.price.@base;
                        else
                            SuggestRoomPriceBase = null;
                    }
                }
            }
        }

        private void FormatReviews(string p)
        {
            CustomReviews = new List<CustomReviewsItem>();
            foreach(Item item in HotelReviews.items)
            {
                CustomReviewsItem customItem = new CustomReviewsItem();
                if(p == "es")
                {
                    if (item.descriptions[0].bad != null)
                        customItem.bad = item.descriptions[0].bad.es;
                    if (item.descriptions[0].good != null)
                        customItem.good = item.descriptions[0].good.es;
                    if (item.descriptions[0].description != null)
                        customItem.description = item.descriptions[0].description.es;
                }
                if (p == "pt")
                {
                    if (item.descriptions[0].bad != null)
                        customItem.bad = item.descriptions[0].bad.pt;
                    if (item.descriptions[0].good != null)
                        customItem.good = item.descriptions[0].good.pt;
                    if (item.descriptions[0].description != null)
                        customItem.description = item.descriptions[0].description.pt;
                }
                customItem.country = "BusarPais";
                customItem.name = item.user.first_name + item.user.last_name;
                customItem.rating = item.qualifications.overall_rating.ToString("N2");
                CustomReviews.Add(customItem);
            }
        }



    }
}
