using Despegar.Core.Business.Enums;
using Despegar.Core.Business.Flight;
using Despegar.Core.Business.Flight.CitiesAutocomplete;
using Despegar.Core.Business.Flight.Itineraries;
using Despegar.Core.Business.Flight.SearchBox;
using Despegar.Core.IService;
using Despegar.WP.UI.Model.Classes;
using Despegar.WP.UI.Model.Classes.Flights;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Models.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace Despegar.WP.UI.Model.ViewModel.Flights
{
    public class FlightSearchViewModel : ViewModelBase
    {
        
        private INavigator Navigator { get; set; }
        private IFlightService flightService { get; set; }
        private FlightSearchModel coreSearchModel;
        public PassengersViewModel PassengersViewModel { get; set; }

        #region ** Exposed Properties **

        public ObservableCollection<FlightMultipleSegment> MultipleSegments
        {
            get { return new ObservableCollection<FlightMultipleSegment>(coreSearchModel.MultipleSegments); }            
        }

        public string Origin 
        { 
            get {
                return coreSearchModel.OriginFlight;
            }
            set {
                coreSearchModel.OriginFlight = value;
                OnPropertyChanged();
            }
        }

        public string Destination 
        {
            get { return coreSearchModel.DestinationFlight; }
            set
            {
                coreSearchModel.DestinationFlight = value;
                OnPropertyChanged();
            } 
        }

        public DateTimeOffset FromDate
        {
            get { return coreSearchModel.DepartureDate; }
            set
            {
                coreSearchModel.DepartureDate = value;

                if(coreSearchModel.PageMode == FlightSearchPages.RoundTrip)
                    ToDate = value; 

                OnPropertyChanged();
            }
        }

        public DateTimeOffset ToDate
        {
            get { return coreSearchModel.DestinationDate; }
            set
            {
                coreSearchModel.DestinationDate = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddSegmentMultipleCommand
        {
            get
            {
                return new RelayCommand(() => { coreSearchModel.AddMultipleSegment(); OnPropertyChanged("MultipleSegments"); });
            }
        }

        public ICommand RemoveSegmentMultipleCommand
        {
            get
            {
                return new RelayCommand(() => { coreSearchModel.RemoveMultipleSegment(); OnPropertyChanged("MultipleSegments"); });
            }
        }

        public ICommand SearchTwoWayCommand
        {
            get
            {
                return new RelayCommand(() => SearchTwoWay());
            }
        }

        public ICommand SearchOneWayCommand
        {
            get
            {
                return new RelayCommand(() => SearchOneWay());
            }
        }

        public ICommand SearchMultiplesCommand
        {
            get
            {
                return new RelayCommand(() => SearchMultiples());
            }
        }

        public ICommand EditMultipleSegment
        {
            get
            {
                // TODO navigate to 
                return new RelayCommand<ItemClickEventArgs>((x) => 
                  {   int segmentIndex = (x.ClickedItem as FlightMultipleSegment).Index;
                  Navigator.GoTo(ViewModelPages.FlightsMultiplEdit, new EditMultiplesNavigationData (){ SelectedSegmentIndex = segmentIndex, SearchModel = coreSearchModel });
                });
            }
        }

        #endregion

        public FlightSearchViewModel(INavigator navigator, IFlightService flightService)
        {
            this.Navigator = navigator;
            this.flightService = flightService;
            this.coreSearchModel = new FlightSearchModel();
            this.PassengersViewModel = new PassengersViewModel();
        }

        public async Task<CitiesAutocomplete> GetCitiesAutocomplete(string cityString) 
        {
            return await flightService.GetCitiesAutocomplete(cityString);
        }

        private void SearchTwoWay()
        {
            coreSearchModel.PageMode = FlightSearchPages.RoundTrip;
            UpdatePassengers();

            DoSearch();            
        }

        private void SearchOneWay()
        { 
            coreSearchModel.PageMode = FlightSearchPages.OneWay;
            UpdatePassengers();
            FromDate = DateTimeOffset.MinValue;

            DoSearch();
        }

        private void SearchMultiples()
        {
            coreSearchModel.PageMode = FlightSearchPages.Multiple;
            UpdatePassengers();
            FromDate = DateTimeOffset.MaxValue; 
            ToDate = DateTimeOffset.MinValue;

            DoSearch();
        }

        private async void DoSearch()
        {
            if (coreSearchModel.IsValid)
            {
                FlightsItineraries intineraries = await flightService.GetItineraries(coreSearchModel);
                //TODO handle error with exceptions.

                var pageParameters = new PageParameters();
                pageParameters.Itineraries = intineraries;
                pageParameters.SearchModel = coreSearchModel;

                Navigator.GoTo(ViewModelPages.FlightsResults, pageParameters);
            }
            else
            {
                // Error messages
            }
        }

        private void UpdatePassengers()
        {
            coreSearchModel.AdultsInFlights = PassengersViewModel.Adults;
            coreSearchModel.ChildrenInFlights = PassengersViewModel.Children;
            coreSearchModel.InfantsInFlights = PassengersViewModel.Infants;
        }
    }
}