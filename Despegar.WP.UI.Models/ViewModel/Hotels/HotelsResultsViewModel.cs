using Despegar.Core.Neo.Business.Hotels.CitiesAvailability;
using Despegar.Core.Neo.Contract.API;
using Despegar.Core.Neo.Contract.Log;
using Despegar.Core.Neo.Exceptions;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Models.Classes;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Despegar.WP.UI.Model.ViewModel.Hotels
{
    public class HotelsResultsViewModel : ViewModelBase
    {
        private IMAPIHotels hotelService { get; set; }

        #region  *** Public Interface ***
        public const int ITEMS_FOR_EACH_PAGE = 30;
        public HotelsCrossParameters CrossParameters { get; set; }

        private CitiesAvailability citiesAvailability { get; set; }
        public CitiesAvailability CitiesAvailability
        {
            get { return citiesAvailability; }
            set
            {
                citiesAvailability = value;
                OnPropertyChanged();
            }
        }
        
        private bool previousPageButtonIsTapEnable { get; set; }
        public bool PreviousPageIsTapEnable
        {
            get
            {
                return previousPageButtonIsTapEnable;
                //return CrossParameters.SearchParameters.offset != 0;

            }
            set
            {
                previousPageButtonIsTapEnable =  value;
                OnPropertyChanged();
            }
        }

        private bool nextPageButtonIsTapEnable { get; set; }
        public bool NextPageButtonIsTapEnable
        {
            get
            {
                return nextPageButtonIsTapEnable;
            }
            set
            {
                nextPageButtonIsTapEnable = value;
                OnPropertyChanged();
            }
        }

        private bool filterButtonIsTapEnable { get; set; }
        public bool FilterButtonIsTapEnable
        {
            get
            {
                return filterButtonIsTapEnable;
                //return CrossParameters.SearchParameters.offset != 0;
            }
            set
            {
                filterButtonIsTapEnable = value;
                OnPropertyChanged();
            }
        }

        private bool orderButtonIsTapEnable { get; set; }
        public bool OrderButtonIsTapEnable
        {
            get
            {
                return orderButtonIsTapEnable;
            }
            set
            {
                orderButtonIsTapEnable = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public HotelsResultsViewModel(INavigator navigator, IMAPIHotels hotelService, IBugTracker t)
            : base(navigator, t)
        {
            this.hotelService = hotelService;
        }

        public ICommand ShowNextPageCommand
        {
            get
            {
                return new RelayCommand(async  () => await ShowNextPage());
            }
        }

        public ICommand ShowPreviousPageCommand
        {
            get
            {
                return new RelayCommand(async () => await ShowPreviousPage());
            }
        }

        private void LockUnlockAppBar(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsLoading")
            {
                RefreshIcons();
            }
        }

        private void RefreshIcons()
        {
            if (!IsLoading)
            {
                FilterButtonIsTapEnable = true;
                OrderButtonIsTapEnable = true;
                PreviousPageIsTapEnable = CrossParameters.SearchModel.offset != 0;

                if (CitiesAvailability != null)
                    NextPageButtonIsTapEnable = (CitiesAvailability.paging.offset + ITEMS_FOR_EACH_PAGE) < CitiesAvailability.paging.total;
            }
            else
            {
                PreviousPageIsTapEnable = false;
                NextPageButtonIsTapEnable = false;
                FilterButtonIsTapEnable = false;
                OrderButtonIsTapEnable = false;
            }
        }

        public async Task Search()
        {
            IsLoading = true;

            try
            {
                if (CrossParameters.SearchModel.latitude == 0)
                    CitiesAvailability = await hotelService.GetHotelsAvailability(CrossParameters.SearchModel.Checkin, CrossParameters.SearchModel.Checkout, CrossParameters.SearchModel.destinationNumber, CrossParameters.SearchModel.distribution, CrossParameters.SearchModel.currency, CrossParameters.SearchModel.offset, CrossParameters.SearchModel.offset + 30, CrossParameters.SearchModel.extraParameters);
                else
                    CitiesAvailability = await hotelService.GetHotelsAvailabilityByGeo(CrossParameters.SearchModel.Checkin, CrossParameters.SearchModel.Checkout, CrossParameters.SearchModel.distribution, CrossParameters.SearchModel.latitude, CrossParameters.SearchModel.longitude);
            }
            catch (APIErrorException e)
            {
                // Custom error?
                OnViewModelError("SEARCH_ERROR", e.ErrorData.code);
            }
            catch (Exception ) {
                OnViewModelError("UNKNOWN_ERROR");
            }

            //RefreshIcons();
            IsLoading = false;
        }

        public async Task ShowNextPage()
        {
            if (!IsLoading)
            {
                CrossParameters.SearchModel.offset += ITEMS_FOR_EACH_PAGE;
                await Search();
            }
        }

        public async Task ShowPreviousPage()
        {
            if(!IsLoading && CrossParameters.SearchModel.offset != 0)
            {
                CrossParameters.SearchModel.offset -= ITEMS_FOR_EACH_PAGE;
                await Search();
            }
        }

        public async Task SearchAgain()
        {
            CrossParameters.SearchModel.extraParameters = "";

            foreach (Facet facet in CitiesAvailability.facets)
            {
                string valueString = "";

                if (facet.values != null)
                {
                    valueString = "";
                    foreach (Value value in facet.values)
                    {
                        if (value.selected)
                        {
                            if (valueString == "")
                                valueString += value.value;
                            else
                                valueString += "," + value.value;
                        }
                    }

                    if (valueString != "")
                    {
                        CrossParameters.SearchModel.extraParameters += facet.criteria + "=" + valueString + "&";
                    }
                }
            }

            foreach (Value value in CitiesAvailability.sorting.values)
            {
                if (value.selected)
                    CrossParameters.SearchModel.extraParameters += "order_by=" + value.value;
            }


            if (CrossParameters.SearchModel.extraParameters != "")
            {
                CrossParameters.SearchModel.offset = 0;
                await Search();
            }

            //CitiesAvailability.SearchStatus = SearchStates.FirstSearch; 
        }

        public void GoToDetails()
        {
            Navigator.GoTo(ViewModelPages.HotelsDetails, CrossParameters);
        }

        public override void OnNavigated(object navigationParams)
        {
            CrossParameters = navigationParams as HotelsCrossParameters;
            PropertyChanged += LockUnlockAppBar;
        }
    }
}