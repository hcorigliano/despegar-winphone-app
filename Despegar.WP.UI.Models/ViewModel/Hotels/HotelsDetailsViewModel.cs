﻿using Despegar.Core.Neo.Business.Hotels;
using Despegar.Core.Neo.Business.Hotels.CustomUserReviews;
using Despegar.Core.Neo.Business.Hotels.HotelDetails;
using Despegar.Core.Neo.Business.Hotels.UserReviews;
using Despegar.Core.Neo.Contract.API;
using Despegar.Core.Neo.Contract.Log;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Model.ViewModel.Classes;
using Despegar.WP.UI.Model.ViewModel.Controls.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Despegar.WP.UI.Model.ViewModel.Hotels
{
    public class HotelsDetailsViewModel : ViewModelBase
    {
        private IMAPIHotels hotelService { get; set; }
        private IAPIv3 userReviewService { get; set; }
        private HotelsCrossParameters CrossParameters { get; set; }

        #region ** Public Interface **
        public int RoomsQuantity
        {
            get
            {
                return CrossParameters.SearchModel.Rooms.Count();
            }
        }

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

        private List<Amenity> _amenitiesShortList;
        public List<Amenity> AmenitiesShortList
        {
            get
            {
                if (_amenitiesShortList == null) _amenitiesShortList = new List<Amenity>();

                if (hotelDetail == null || hotelDetail.hotel == null) return null;

                if(hotelDetail.hotel.amenities!=null)
                {
                    int maxToTake = (hotelDetail.hotel.amenities.Count()<5)? hotelDetail.hotel.amenities.Count(): 4;
                    var firstFourElements = hotelDetail.hotel.amenities.Take(maxToTake);
                    _amenitiesShortList.AddRange(firstFourElements);
                }
                return _amenitiesShortList;
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

        public ICommand BuySuggestRoomCommand
        {
            get
            {
                return new RelayCommand(() => BuySuggestRoom());
            }
        }

        public ICommand BuySelectRoomCommand
        {
            get
            {
                return new RelayCommand(() => BuySelectedRoomCommand());
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

        public int HotelDistance { get; set; }

        #endregion

        public HotelsDetailsViewModel(INavigator navigator, IMAPIHotels hotelService, IAPIv3 hotelReviews, IBugTracker t)
            : base(navigator, t)
        {
            this.hotelService = hotelService;
            this.userReviewService = hotelReviews;
        }

        public override void OnNavigated(object navigationParams)
        {
            CrossParameters = navigationParams as HotelsCrossParameters;
        }
    
        public async Task Init()
        {
            IsLoading = true;

           HotelDetail = await hotelService.GetHotelsDetail(CrossParameters.IdSelectedHotel, CrossParameters.SearchModel.DepartureDateFormatted, CrossParameters.SearchModel.DestinationDateFormatted, CrossParameters.SearchModel.DistributionString);
           
           HotelReviews = await userReviewService.GetHotelUserReviews(CrossParameters.IdSelectedHotel, 10, 0, "es");
           FormatReviews(GlobalConfiguration.Language);

            
            foreach (Roompack roompack in HotelDetail.roompacks)
            {
                if (roompack.rooms[0].pictures == null)
                {
                    roompack.rooms[0].pictures = new List<string>(); 
                    roompack.rooms[0].pictures.Add(HotelDetail.hotel.main_picture); 
                    
                }
                foreach(RoomAvailability room in roompack.room_availabilities)
                {
                    room.buySelectedRoom = this.BuySelectRoomCommand;
                }
            }

            HotelDistance = Convert.ToInt32(CrossParameters.HotelsExtraData.Distance);

            // Get suggest room price
            foreach (Roompack roomPack in hotelDetail.roompacks)
            {
                foreach (RoomAvailability room in roomPack.room_availabilities)
                {
                    if (room.choices.Contains(hotelDetail.suggested_room_choice))
                    {
                        hotelDetail.list_suggested_room_choice = room.choices;

                        SuggestRoomPriceBest = room.price.best;

                        if (SuggestRoomPriceBest != room.price.@base)
                            SuggestRoomPriceBase = room.price.@base;
                        else
                            SuggestRoomPriceBase = null;
                    }
                }
            }

            IsLoading = false;
        }

        private void BuySuggestRoom()
        {
            if (CrossParameters != null && hotelDetail != null)
            {
                CrossParameters.BookRequest = new HotelsBookingFieldsRequest()
                {
                    token = HotelDetail.token,
                    hotel_id = hotelDetail.id,
                    room_choices = hotelDetail.list_suggested_room_choice,
                    mobile_identifier = GlobalConfiguration.UPAId,
                    SelectedItemIndex = CrossParameters.UPA_SelectedItemIndex
                };

                Navigator.GoTo(ViewModelPages.HotelsCheckout, CrossParameters);
            }
        }

        private void BuySelectedRoomCommand()
        {            
            RoomAvailability room =  HotelDetail.roompacks[0].room_availabilities.First(x => x.selectedRoom);

            BedOption bedOption = new BedOption();
            foreach(Room roomBed in HotelDetail.roompacks[0].rooms)
            {
                bedOption = roomBed.bed_options.FirstOrDefault(x=>x.Selected);
            }

            CrossParameters.BedSelected = bedOption;

            if (CrossParameters != null && hotelDetail != null)
            {
                CrossParameters.BookRequest = new HotelsBookingFieldsRequest()
                {
                    token = HotelDetail.token,
                    hotel_id = hotelDetail.id,
                    room_choices = room.choices,
                    mobile_identifier = GlobalConfiguration.UPAId,  
                    SelectedItemIndex = CrossParameters.UPA_SelectedItemIndex
                };

                Navigator.GoTo(ViewModelPages.HotelsCheckout, CrossParameters);
            }
        }

        private void FormatReviews(string p)
        {
            CustomReviews = new List<CustomReviewsItem>();
            foreach(Item item in HotelReviews.items)
            {
                CustomReviewsItem customItem = new CustomReviewsItem();
                if(p.ToLower().Equals("es"))
                {
                    if (item.descriptions[0].bad != null)
                        customItem.bad = item.descriptions[0].bad.es;
                    if (item.descriptions[0].good != null)
                        customItem.good = item.descriptions[0].good.es;
                    if (item.descriptions[0].description != null)
                        customItem.description = item.descriptions[0].description.es;
                }
                if (p.ToLower().Equals("pt"))
                {
                    if (item.descriptions[0].bad != null)
                        customItem.bad = item.descriptions[0].bad.pt;
                    if (item.descriptions[0].good != null)
                        customItem.good = item.descriptions[0].good.pt;
                    if (item.descriptions[0].description != null)
                        customItem.description = item.descriptions[0].description.pt;
                }
                customItem.country = "BusarPais";
                if (item.user != null)
                {
                    customItem.name = (String.IsNullOrEmpty(item.user.first_name) ? String.Empty : item.user.first_name) + " " + (String.IsNullOrEmpty(item.user.last_name) ? String.Empty : item.user.last_name);
                }
                customItem.rating = item.qualifications.overall_rating.ToString("N2");
                CustomReviews.Add(customItem);
            }
        }

    }
}