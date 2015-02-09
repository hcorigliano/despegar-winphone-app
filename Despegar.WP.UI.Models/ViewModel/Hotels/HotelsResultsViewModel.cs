using Despegar.Core.Business;
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

    public class HotelsResultsViewModel : ViewModelBase
    {
        #region private
        private IHotelService hotelService { get; set; }
        #endregion

        #region public
        public INavigator Navigator { get; set; }
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
        
        private bool backButtonIsTapEnable { get; set; }
        public bool BackButtonIsTapEnable
        {
            get
            {
                return backButtonIsTapEnable;
                //return CrossParameters.SearchParameters.offset != 0;

            }
            set
            {
                backButtonIsTapEnable =  value;
                OnPropertyChanged();
            }
        }

        private bool forwardButtonIsTapEnable { get; set; }
        public bool ForwardButtonIsTapEnable
        {
            get
            {
                return forwardButtonIsTapEnable;
            }
            set
            {
                forwardButtonIsTapEnable = value;
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

        public HotelsResultsViewModel(INavigator navigator, IHotelService hotelService, IBugTracker t): base(t)
        {
            this.Navigator = navigator;
            this.hotelService = hotelService;
            PropertyChanged += LockUnlockAppBar;
        }

        public ICommand ShowNextPageCommand
        {
            get
            {
                return new RelayCommand(() => ShowNextPage());
            }
        }

        public ICommand ShowPreviousPageCommand
        {
            get
            {
                return new RelayCommand(() => ShowPreviousPage());
            }
        }

        private void LockUnlockAppBar(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsLoading")
            {
                if ((sender as ViewModelBase).IsLoading)
                {
                    FilterButtonIsTapEnable = false;
                    OrderButtonIsTapEnable = false;
                    RefreshIcons();
                }
                else
                {
                    FilterButtonIsTapEnable = true;
                    OrderButtonIsTapEnable = true;
                    RefreshIcons();
                }
            }
        }

        private void RefreshIcons()
        {
            if (!IsLoading)
            {
                BackButtonIsTapEnable = CrossParameters.SearchParameters.offset != 0;
                if (CitiesAvailability != null)
                    ForwardButtonIsTapEnable = (CitiesAvailability.paging.offset + ITEMS_FOR_EACH_PAGE) < CitiesAvailability.paging.total;
            }
            else
            {
                BackButtonIsTapEnable = false;
                ForwardButtonIsTapEnable = false;
            }
        }

        public async Task Search()
        {
            IsLoading = true;
            if (CrossParameters.SearchParameters.latitude == 0)
            {
                CitiesAvailability = await hotelService.GetHotelsAvailability(CrossParameters.SearchParameters.Checkin, CrossParameters.SearchParameters.Checkout, CrossParameters.SearchParameters.destinationNumber, CrossParameters.SearchParameters.distribution, CrossParameters.SearchParameters.currency, CrossParameters.SearchParameters.offset, CrossParameters.SearchParameters.offset + 30, CrossParameters.SearchParameters.extraParameters);
            }
            else
            {
                CitiesAvailability = await hotelService.GetHotelsAvailabilityByGeo(CrossParameters.SearchParameters.Checkin, CrossParameters.SearchParameters.Checkout, CrossParameters.SearchParameters.distribution, CrossParameters.SearchParameters.latitude, CrossParameters.SearchParameters.longitude);
            }
            //RefreshIcons();
            IsLoading = false;
        }

        public async Task ShowNextPage()
        {
            if (!IsLoading)
            {
                CrossParameters.SearchParameters.offset += ITEMS_FOR_EACH_PAGE;
                await Search();
            }
        }

        public async Task ShowPreviousPage()
        {
            if(!IsLoading && CrossParameters.SearchParameters.offset != 0)
            {
                CrossParameters.SearchParameters.offset -= ITEMS_FOR_EACH_PAGE;
                await Search();
            }
        }

        public async Task SearchAgaing()
        {
            CrossParameters.SearchParameters.extraParameters = "";

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
                        CrossParameters.SearchParameters.extraParameters += facet.criteria + "=" + valueString + "&";
                    }
                }
            }

            foreach (Value value in CitiesAvailability.sorting.values)
            {
                if (value.selected)
                    CrossParameters.SearchParameters.extraParameters += "order_by=" + value.value;
            }


            if (CrossParameters.SearchParameters.extraParameters != "")
            {
                CrossParameters.SearchParameters.offset = 0;
                await Search();
            }

            CitiesAvailability.SearchStatus = SearchStates.FirstSearch; 

        }

        public void GoToDetails()
        {
            Navigator.GoTo(ViewModelPages.HotelsDetails, CrossParameters);
        }

    }
}
