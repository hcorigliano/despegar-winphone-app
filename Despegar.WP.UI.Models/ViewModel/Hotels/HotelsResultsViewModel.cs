using Despegar.Core.Neo.Business.Hotels.CitiesAvailability;
using Despegar.Core.Neo.Business.Hotels.SearchBox;
using Despegar.Core.Neo.Contract.API;
using Despegar.Core.Neo.Contract.Log;
using Despegar.Core.Neo.Exceptions;
using Despegar.WP.UI.Model.Classes;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Model.ViewModel.Classes;
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
        public HotelSearchModel SearchModel { get; set; }

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

        public ICommand ShowNextPageCommand
        {
            get
            {
                return new RelayCommand(async () => await ShowNextPage());
            }
        }

        public ICommand ShowPreviousPageCommand
        {
            get
            {
                return new RelayCommand(async () => await ShowPreviousPage());
            }
        }

        public ICommand FilterCommand
        {
            get
            {
                return new RelayCommand(() => Navigator.GoTo(ViewModelPages.HotelsFilter, new GenericResultNavigationData(){ SearchModel = SearchModel } ));
            }
        }

        public ICommand OrderByCommand
        {
            get
            {
                return new RelayCommand(() => Navigator.GoTo(ViewModelPages.HotelsOrderBy, new GenericResultNavigationData() { SearchModel = SearchModel }));
            }
        }

        #endregion

        public HotelsResultsViewModel(INavigator navigator, IMAPIHotels hotelService, IBugTracker t)
            : base(navigator, t)
        {
            this.hotelService = hotelService;
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
            if (IsLoading)
            {
                PreviousPageIsTapEnable = false;
                NextPageButtonIsTapEnable = false;
                FilterButtonIsTapEnable = false;
                OrderButtonIsTapEnable = false;               
            }
            else
            {
                FilterButtonIsTapEnable = SearchModel.Facets.Count > 0;
                OrderButtonIsTapEnable = true;
                PreviousPageIsTapEnable = SearchModel.Offset != 0;

                if (CitiesAvailability != null)
                    NextPageButtonIsTapEnable = (CitiesAvailability.paging.offset + ITEMS_FOR_EACH_PAGE) < CitiesAvailability.paging.total;
                else
                    NextPageButtonIsTapEnable = false;
            }
        }

        public async Task LoadResults()
        {            
            IsLoading = true;          
          
            try
            {
               CitiesAvailability = await hotelService.GetHotelsAvailability(SearchModel);

               if (CitiesAvailability.items.Count == 0)
                   OnViewModelError("SEARCH_NO_RESULTS");
               else {
                   // Set filering
                   SearchModel.Facets = CitiesAvailability.facets;
                   SearchModel.Sortings = CitiesAvailability.sorting;
               }
            }
            catch (APIErrorException e)
            {
                // Custom error?
                OnViewModelError("SEARCH_ERROR", e.ErrorData.code);
            }
            catch (Exception ) {
                OnViewModelError("UNKNOWN_ERROR");
            }
            
            IsLoading = false;
        }

        public async Task ShowNextPage()
        {
            if (!IsLoading)
            {
                SearchModel.Offset += ITEMS_FOR_EACH_PAGE;
                await LoadResults();
            }
        }

        public async Task ShowPreviousPage()
        {
            if(!IsLoading && SearchModel.Offset != 0)
            {
                SearchModel.Offset -= ITEMS_FOR_EACH_PAGE;
                await LoadResults();
            }
        }
        
        public override void OnNavigated(object navigationParams)
        {
            BugTracker.LeaveBreadcrumb("Hotels Results View");
            GenericResultNavigationData pageParameters = navigationParams as GenericResultNavigationData;
            PropertyChanged += LockUnlockAppBar;

            // Reset paging (a new Search has been performed)
            SearchModel = (HotelSearchModel)pageParameters.SearchModel;
            SearchModel.Offset = 0;

            // Remover la pantall de filtros/order del navigation stack
            if (pageParameters.FiltersApplied)
            {
                Navigator.RemoveBackEntry(); // Filters page
                Navigator.RemoveBackEntry(); // Old results page
            }
        }

        public void GoToDetails(HotelItem hotelItem, int selectedIndex)
        {
            var param = new HotelsCrossParameters()
            {
                SelectedHotel = hotelItem,
                SearchModel = this.SearchModel, 
                IdSelectedHotel = hotelItem.id,
                UPA_SelectedItemIndex = selectedIndex,
            };
            param.HotelsExtraData.Distance = hotelItem.distance;

            Navigator.GoTo(ViewModelPages.HotelsDetails, param);
        }
    }
}