using Despegar.Core.Neo.Business.Configuration;
using Despegar.Core.Neo.Business.Hotels;
using Despegar.Core.Neo.Business.Hotels.CustomUserReviews;
using Despegar.Core.Neo.Business.Hotels.HotelDetails;
using Despegar.Core.Neo.Business.Hotels.UserReviews;
using Despegar.Core.Neo.Business.Hotels.UserReviews.V1;
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
        private HotelsCrossParameters CrossParameters { get; set; }

        #region ** Public Interface **
        public HotelUserReviews HotelReviews { get; set;}
        public HotelUserReviewsV1 HotelReviewsV1 { get; set; }

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

        public HotelsDetailsViewModel(INavigator navigator, IMAPIHotels hotelService, IAPIv3 hotelReviews, IAPIv1 hReviewV1 ,IMAPICross crossService, IBugTracker t)
            : base(navigator, t)
        {
            this.hotelService = hotelService;
            this.userReviewService = hotelReviews;
            this.crossService = crossService;
            this.userReviewServiceV1 = hReviewV1;

        }

        public override void OnNavigated(object navigationParams)
        {
            CrossParameters = navigationParams as HotelsCrossParameters;
        }
    
        public async Task Init()
        {
            IsLoading = true;

           HotelDetail = await hotelService.GetHotelsDetail(CrossParameters.IdSelectedHotel, CrossParameters.SearchModel.DepartureDateFormatted, CrossParameters.SearchModel.DestinationDateFormatted, CrossParameters.SearchModel.DistributionString);
           
           //HotelReviews = await userReviewService.GetHotelUserReviews(CrossParameters.IdSelectedHotel, 10, 0, "es","despegar");

           HotelReviewsV1 = await userReviewServiceV1.GetHotelUserReviews(CrossParameters.IdSelectedHotel, 10, 0, "es", "despegar");
           CompleteReviewsWithV1Response();

           //FormatReviews(GlobalConfiguration.Language);

            
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

            //TEST
            



            HotelDistance = Convert.ToInt32(CrossParameters.HotelsExtraData.Distance);


            // Get suggest room price
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

            IsLoading = false;
        }

        private async void CompleteReviewsWithV1Response()
        {
            ResourceLoader manager = new ResourceLoader();
            CustomReviews = new List<CustomReviewsItem>();
            Countries countries = await crossService.GetCountries();

            foreach (Review review in HotelReviewsV1.reviews)
            {
                CustomReviewsItem reviewItem = new CustomReviewsItem();

                reviewItem.description = (review.comments.FirstOrDefault() != null) ? review.comments.FirstOrDefault().description : String.Empty;
                reviewItem.bad = (review.comments.FirstOrDefault() != null) ? review.comments.FirstOrDefault().bad : String.Empty;
                reviewItem.good = (review.comments.FirstOrDefault() != null) ? review.comments.FirstOrDefault().good : String.Empty;
                reviewItem.name = (review.user != null) ? review.user.name : String.Empty;
                    
                reviewItem.name = (String.IsNullOrWhiteSpace(reviewItem.name)) ? manager.GetString("Page_Hotels_Anonymous") : reviewItem.name;

                reviewItem.countryCode = (review.user != null) ? review.user.country : String.Empty;

                var country = countries.countries.Where(x => x.id == reviewItem.countryCode).FirstOrDefault();
                reviewItem.country = (country != null)? country.name : String.Empty;

                reviewItem.rating = (review.scores != null) ? (review.scores.avgRecommend/10).ToString() : "0";

                customReviews.Add(reviewItem);
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
                    room_choices = new List<string>() { hotelDetail.suggested_room_choice },
                    mobile_identifier = GlobalConfiguration.UPAId
                };

                Navigator.GoTo(ViewModelPages.HotelsCheckout, CrossParameters);
            }
        }

        private object BuySelectedRoomCommand()
        {
            //CUAL MIERDA ES EL CUARTO
            RoomAvailability room =  HotelDetail.roompacks[0].room_availabilities.First(x => x.selectedRoom);

            //CUal mierda es la cama 
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
                    mobile_identifier = GlobalConfiguration.UPAId
                };

                Navigator.GoTo(ViewModelPages.HotelsCheckout, CrossParameters);
            }

            //TODO: Buy
            int test = 1;
            return null;
        }

        private  async void FormatReviews(string p)
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

                if (item.user != null)
                {
                    if (item.user.city_id > 0) { 
                        customItem.country = await this.GetCountry(item.user.city_id.ToString()); 
                        //customItem.country = String.Empty;
                    }else
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
