using Despegar.Core.Neo.Business.Configuration;
using Despegar.Core.Neo.Business.Hotels;
using Despegar.Core.Neo.Business.Hotels.CustomUserReviews;
using Despegar.Core.Neo.Business.Hotels.HotelDetails;
using Despegar.Core.Neo.Business.Hotels.UserReviews;
using Despegar.Core.Neo.Business.Hotels.UserReviews.V1;
using Despegar.Core.Neo.Contract.API;
using Despegar.Core.Neo.Contract.Log;
using Despegar.Core.Neo.Exceptions;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Model.ViewModel.Classes;
using Despegar.WP.UI.Model.ViewModel.Controls.Maps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Resources;
using Windows.Storage;


namespace Despegar.WP.UI.Model.ViewModel.Hotels
{
    public class HotelsDetailsViewModel : ViewModelBase
    {
        private IMAPICross crossService { get; set; }
        private IMAPIHotels hotelService { get; set; }
        private IAPIv3 userReviewService { get; set; }
        private IAPIv1 userReviewServiceV1 { get; set; }
        private IGoogleAnalytics analyticsService;
        private HotelsCrossParameters CrossParameters { get; set; }

        #region ** Public Interface **
        public int RoomsQuantity
        {
            get
            {
                return CrossParameters.SearchModel.Rooms.Count();
            }
        }

        public HotelUserReviews HotelReviews { get; set; }
        public HotelUserReviewsV1 HotelReviewsV1 { get; set; }

        private ObservableCollection<CustomReviewsItem> customReviews { get; set; }
        public ObservableCollection<CustomReviewsItem> CustomReviews
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
            get
            {
                if (_customMap == null) _customMap = new CustomMapViewModel();

                if (hotelDetail != null && hotelDetail.hotel.geo_location != null)
                {
                    Classes.CustomPinPoint pinpoint = new Classes.CustomPinPoint() { Latitude = hotelDetail.hotel.geo_location.latitude, Longitude = hotelDetail.hotel.geo_location.longitude, Title = hotelDetail.hotel.name, Address = hotelDetail.hotel.address };
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

                if (hotelDetail.hotel.amenities != null)
                {
                    int maxToTake = (hotelDetail.hotel.amenities.Count() < 5) ? hotelDetail.hotel.amenities.Count() : 4;
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

        public HotelsDetailsViewModel(INavigator navigator, IMAPIHotels hotelService, IAPIv3 hotelReviews, IAPIv1 hReviewV1 ,IMAPICross crossService, IBugTracker t, IGoogleAnalytics analyticsService)
            : base(navigator, t)
        {
            this.hotelService = hotelService;
            this.userReviewService = hotelReviews;
            this.crossService = crossService;
            this.userReviewServiceV1 = hReviewV1;
            this.analyticsService = analyticsService;

        }

        public override void OnNavigated(object navigationParams)
        {
            CrossParameters = navigationParams as HotelsCrossParameters;
            analyticsService.SendView("HotelsDetails");

        }

        public async Task Init()
        {
            IsLoading = true;
            try
            {
                HotelDetail = await hotelService.GetHotelsDetail(CrossParameters.IdSelectedHotel, CrossParameters.SearchModel.DepartureDateFormatted, CrossParameters.SearchModel.DestinationDateFormatted, CrossParameters.SearchModel.DistributionString);

                //HotelReviews = await userReviewService.GetHotelUserReviews(CrossParameters.IdSelectedHotel, 10, 0, "es","despegar");

                HotelReviewsV1 = await userReviewServiceV1.GetHotelUserReviews(CrossParameters.IdSelectedHotel, true, 1, 10, true);
                CompleteReviewsWithV1Response();

                //FormatReviews(GlobalConfiguration.Language);

                foreach (Roompack roompack in HotelDetail.roompacks)
                {
                    if (roompack.rooms[0].pictures == null)
                    {
                        roompack.rooms[0].pictures = new List<string>();
                        roompack.rooms[0].pictures.Add(HotelDetail.hotel.main_picture);

                    }
                    foreach (RoomAvailability room in roompack.room_availabilities)
                    {
                        room.buySelectedRoom = this.BuySelectRoomCommand;
                    }
                }

                HotelDistance = Convert.ToInt32(CrossParameters.HotelsExtraData.Distance);

                // Get suggest room price and some more things

                if (hotelDetail.roompacks.Count > 0)
                {
                    Roompack tempRoompack = new Roompack();
                    foreach (Roompack roomPack in hotelDetail.roompacks)
                    {
                        tempRoompack = roomPack;
                        foreach (RoomAvailability room in roomPack.room_availabilities)
                        {
                            if (room.choices.Contains(hotelDetail.suggested_room_choice))
                            {
                                hotelDetail.list_suggested_room_choice = room.choices; //Transforma el suggest en una lista completa la cual es necesaria para hacer el booking.

                                SuggestRoomPriceBest = room.price.best;

                                if (SuggestRoomPriceBest != room.price.@base)
                                    SuggestRoomPriceBase = room.price.@base;
                                else
                                    SuggestRoomPriceBase = null;

                                CrossParameters.RoomPackSelected = tempRoompack; //Toma el roompack seleccionado.

                                if (tempRoompack.rooms[0].bed_options != null && tempRoompack.rooms[0].bed_options.Count > 0)
                                    CrossParameters.BedSelected = tempRoompack.rooms[0].bed_options[0];

                                break;
                            }
                        }
                    }
                }
                else
                {
                    OnViewModelError("NO_AVAILABILITY");
                }
            }
            catch (APIErrorException e)
            {
                // Custom error?
                OnViewModelError("SEARCH_ERROR", e.ErrorData.code);
            }
            catch (Exception)
            {
                OnViewModelError("INIT_FAILED");
            }

            IsLoading = false;
        }

        private async void CompleteReviewsWithV1Response()
        {
            ResourceLoader manager = new ResourceLoader();
            CustomReviews = new ObservableCollection<CustomReviewsItem>();
            try
            {
                Countries countries = await crossService.GetCountries();


                foreach (Review review in HotelReviewsV1.reviews)
                {
                    CustomReviewsItem reviewItem = new CustomReviewsItem();

                    //reviewItem.description = (review.comments.FirstOrDefault() != null) ? review.comments.FirstOrDefault().description : String.Empty;
                    //reviewItem.bad = (review.comments.FirstOrDefault() != null) ? review.comments.FirstOrDefault().bad : String.Empty;
                    //reviewItem.good = (review.comments.FirstOrDefault() != null) ? review.comments.FirstOrDefault().good : String.Empty;

                    reviewItem.description = review.comments.description;
                    reviewItem.bad = review.comments.bad;
                    reviewItem.good = review.comments.good;

                    reviewItem.name = (review.user != null) ? review.user.name : String.Empty;

                    reviewItem.name = (String.IsNullOrWhiteSpace(reviewItem.name)) ? manager.GetString("Page_Hotels_Anonymous") : reviewItem.name;

                    reviewItem.countryCode = (review.user != null) ? review.user.country : String.Empty;

                    if (countries != null)
                    {
                        var country = countries.countries.Where(x => x.id == reviewItem.countryCode).FirstOrDefault();
                        reviewItem.country = (country != null) ? country.name : String.Empty;
                    }

                    //reviewItem.rating = (review.scores != null) ? (review.scores.avgRecommend/10).ToString() : "0";

                    reviewItem.rating = (review.averageScore / 10).ToString("N2");

                    customReviews.Add(reviewItem);
                }
            }
            catch
            {
                //Do Nothing
            }
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
            RoomAvailability room = new RoomAvailability();

            //Refactor?
            foreach (Roompack rp in HotelDetail.roompacks)
            {
                foreach (RoomAvailability ra in rp.room_availabilities)
                {
                    if (ra.selectedRoom)
                    {
                        room = ra;
                        ra.selectedRoom = false;
                    }
                }

                foreach (Room roomBed in rp.rooms)
                {
                    foreach (BedOption bo in roomBed.bed_options)
                    {
                        if (bo.Selected)
                        {
                            CrossParameters.BedSelected = bo;
                            bo.Selected = false;
                        }
                    }
                    //CrossParameters.BedSelected = roomBed.bed_options.FirstOrDefault(x => x.Selected);
                }
            }

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

                CrossParameters.RoomPackSelected = HotelDetail.roompacks[0];  //En selected siempre es el primero, por que si fuera mas de uno entonces no se mostraria la seleccion de cuartos.

                Navigator.GoTo(ViewModelPages.HotelsCheckout, CrossParameters);
            }
        }

        private async void FormatReviews(string p)
        {
            CustomReviews = new ObservableCollection<CustomReviewsItem>();
            foreach (Item item in HotelReviews.items)
            {
                CustomReviewsItem customItem = new CustomReviewsItem();
                if (p.ToLower().Equals("es"))
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

                if (item.user != null)
                {
                    if (item.user.city_id > 0)
                    {
                        customItem.country = await this.GetCountry(item.user.city_id.ToString());
                        //customItem.country = String.Empty;
                    }
                    else
                    {
                        customItem.country = String.Empty;
                    }


                    if (item.user.first_name == null && item.user.last_name == null)
                    {
                        ResourceLoader manager = new ResourceLoader();
                        customItem.name = manager.GetString("Page_Hotels_Anonymous");
                    }
                    else
                    {
                        customItem.name = (String.IsNullOrEmpty(item.user.first_name) ? String.Empty : item.user.first_name) + " " + (String.IsNullOrEmpty(item.user.last_name) ? String.Empty : item.user.last_name);
                    }
                }

                double tempRating = item.qualifications.overall_rating / 10;
                customItem.rating = tempRating.ToString("N2");
                CustomReviews.Add(customItem);
            }
        }

        private async Task<string> GetCountry(string cityId)
        {

            Despegar.Core.Neo.Business.Configuration.City city = await crossService.GetCity(cityId);

            return (city != null) ? city.country_name : String.Empty;
        }

    }
}