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
        private IMAPIFlights flightService;

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

        public FlightResultsViewModel(INavigator navigator, IMAPIFlights flightService, IBugTracker t) : base(navigator, t)
        {
            this.flightService = flightService;
            Items = new List<BindableItem>();
            FlightCrossParameters = new FlightsCrossParameter();
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

        public override void OnNavigated(object navigationParams)
        {
            BugTracker.LeaveBreadcrumb("Flight Results View");
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
            try
            {
                Itineraries = await flightService.GetItineraries(FlightSearchModel);

                if (Itineraries.items.Count == 0)
                {
                    OnViewModelError("LOAD_RESULTS_NO_ITEMS");
                    return;
                }

                FlightSearchModel.Facets = Itineraries.facets;
                FlightSearchModel.Sorting = Itineraries.sorting;


                if (Itineraries.items != null)
                    this.Items = (Itineraries.items.Select(il => new BindableItem(il))).ToList();
            }
            catch (Exception)
            {
                // Will not filter the results, but it will keep the last list status
                OnViewModelError("LOAD_RESULTS_FAILED");
            }

            IsLoading = false;
        }

        /// <summary>
        /// Hace Rebúsqueda
        /// </summary>
        public void MiniboxSearch()
        {
            BugTracker.LeaveBreadcrumb("Flight Result Minibox Hit");
            Navigator.GoBack();
        } 
        
    }
}