using Despegar.Core.Neo.Business;
using Despegar.Core.Neo.Business.Flight.Itineraries;
using Despegar.Core.Neo.Business.Flight.SearchBox;
using Despegar.Core.Neo.Contract.API;
using Despegar.Core.Neo.Contract.Log;
using Despegar.WP.UI.Model.Classes;
using Despegar.WP.UI.Model.Classes.Flights;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Model.ViewModel.Classes;
using Despegar.WP.UI.Model.ViewModel.Classes.Flights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Despegar.WP.UI.Model.ViewModel.Flights
{
    public class FlightResultsViewModel : ViewModelBase
    {
        public const int ITEMS_FOR_EACH_PAGE = 15;

        #region #Control Buttons#

        private bool previousPageButtonIsTapEnable { get; set; }
        public bool PreviousPageIsTapEnable
        {
            get
            {
                return previousPageButtonIsTapEnable;

            }
            set
            {
                previousPageButtonIsTapEnable = value;
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

        public async Task ShowNextPage()
        {
            if (!IsLoading)
            {
                FlightSearchModel.Offset += ITEMS_FOR_EACH_PAGE;
                await LoadResults();
            }
        }

        public async Task ShowPreviousPage()
        {
            if (!IsLoading && FlightSearchModel.Offset != 0)
            {
                FlightSearchModel.Offset -= ITEMS_FOR_EACH_PAGE;
                await LoadResults();
            }
        }


        public ICommand NavigateToFiltersCommand
        {
            get
            {
                return new RelayCommand(() => { Navigator.GoTo(Model.Interfaces.ViewModelPages.FlightsFilters, new GenericResultNavigationData() { SearchModel = FlightSearchModel }); });
            }
        }

        public ICommand NavigateToOrderByCommand
        {
            get
            {
                return new RelayCommand(() => { Navigator.GoTo(Model.Interfaces.ViewModelPages.FlightsOrderBy, new GenericResultNavigationData() { SearchModel = FlightSearchModel }); });
            }
        }

#endregion

        private IMAPIFlights flightService;
        private IGoogleAnalytics analyticsService;

        private FlightsItineraries itineraries;
        public FlightsItineraries Itineraries
        {
            get { return itineraries; }
            set
            {
                itineraries = value;
            }
        }
        
        //TODO: create the correct class for cheapest_price
        //public IObservable<object> cheapest_price { get; set; }

        public FlightsCrossParameter FlightCrossParameters { get; set; }
        public FlightSearchModel FlightSearchModel { get; set; }
        
        // This items represents flights inbounds and outbounds
        private List<BindableItem> _items;
        public List<BindableItem> Items {
            get { return _items; }
            set {
                _items = value;
                OnPropertyChanged();
            }
        }

        private List<Facet> facets;
        public List<Facet> Facets 
        {
            get
            {
                return facets;
            }
            set 
            {
                  facets = value;
                  OnPropertyChanged();
            }
        }

        private Sorting sorting;
        public Sorting Sorting {
            get { return sorting; }
            set
            {
                sorting = value;
                OnPropertyChanged();
            } 
        }
        

        // TODO: se usa???
        public string SelectedCurrency
        {
            get
            {
                if (this.Itineraries.currencies == null) 
                      return String.Empty;

                var _cur = this.Itineraries.currencies.values.FirstOrDefault(x => x.selected == true);

                if (_cur == null)
                    return String.Empty;

                return ((Value)_cur).label;
            }
        }

        public FlightResultsViewModel(INavigator navigator, IMAPIFlights flightService, IBugTracker t, IGoogleAnalytics analyticsService) : base(navigator, t)
        {
            this.flightService = flightService;
            this.analyticsService = analyticsService;
            Items = new List<BindableItem>();
            FlightCrossParameters = new FlightsCrossParameter();
        }

        public override void OnNavigated(object navigationParams)
        {
            BugTracker.LeaveBreadcrumb("Flight Results View");
            analyticsService.SendView("FlightResults");

            GenericResultNavigationData pageParameters = navigationParams as GenericResultNavigationData;

            // Obtain Search parameters
            FlightSearchModel = pageParameters.SearchModel as FlightSearchModel;

            // Remover la pantall de filtros/order del navigation stack
            if (pageParameters.FiltersApplied)
            {
                Navigator.RemoveBackEntry(); // Filters page
                Navigator.RemoveBackEntry(); // Old results page
            }
        }

        public async Task LoadResults()
        {
            BugTracker.LeaveBreadcrumb("FlightsResults Load Results");
            IsLoading = true;

            DisableButtons();

            PreviousPageIsTapEnable = false;
            NextPageButtonIsTapEnable = false;

            try
            {
                Itineraries = await flightService.GetItineraries(FlightSearchModel);

                if (Itineraries.items.Count == 0)
                {
                    OnViewModelError("LOAD_RESULTS_NO_ITEMS");
                    IsLoading = false;
                    return;
                }

                FlightSearchModel.Facets = Itineraries.facets;
                FlightSearchModel.Sorting = Itineraries.sorting;
                FlightSearchModel.TotalFlights = itineraries.total;


                if (Itineraries.items != null)
                    this.Items = (Itineraries.items.Select(il => new BindableItem(il))).ToList();
            }
            catch (Exception)
            {
                // Will not filter the results, but it will keep the last list status
                OnViewModelError("LOAD_RESULTS_FAILED");
            }

            EnableButtons();

            IsLoading = false;
        }

        private void DisableButtons()
        {
            PreviousPageIsTapEnable = false;
            NextPageButtonIsTapEnable = false;
            OrderButtonIsTapEnable = false;
            FilterButtonIsTapEnable = false;
        }

        private void EnableButtons()
        {
            if (FlightSearchModel != null)
            {
                if (FlightSearchModel.Facets != null)
                    FilterButtonIsTapEnable = FlightSearchModel.Facets.Count > 0;
                else
                    FilterButtonIsTapEnable = false;

                OrderButtonIsTapEnable = true;

                PreviousPageIsTapEnable = FlightSearchModel.Offset != 0;

                if (Itineraries != null && Itineraries.paging != null)
                    NextPageButtonIsTapEnable = (Itineraries.paging.offset + ITEMS_FOR_EACH_PAGE) < Itineraries.paging.total;
                else
                    NextPageButtonIsTapEnable = false;
            }
        }

        /// <summary>
        /// Hace Rebúsqueda
        /// </summary>
        public void MiniboxSearch()
        {
            BugTracker.LeaveBreadcrumb("Flight Result Minibox Hit");
            Navigator.GoBack();
        }

        public void ResetPagination()
        {
            FlightSearchModel.Offset = 0;
        }

        public void RefreshMiniBox()
        {
            FlightSearchModel.PropertyChangedMiniBox();
        }
    }
}