using Despegar.Core.Business.Enums;
using Despegar.Core.Business.Flight;
using Despegar.Core.Business.Flight.CitiesAutocomplete;
using Despegar.Core.Business.Flight.Itineraries;
using Despegar.Core.IService;
using Despegar.WP.UI.Model.Classes;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Despegar.WP.UI.Model.ViewModel.Flights
{
    public class FlightSearchViewModel : ViewModelBase
    {
        private const int MAX_MULTIPLE_SEGMENTS = 6;
        private INavigator Navigator { get; set; }
        private IFlightService flightService { get; set; }
        private FlightSearchModel coreSearchModel;
        public PassengersViewModel PassengersViewModel { get; set; }

        #region ** Exposed Properties **

        private int numberOfSegments = 0;

        public bool IsLoading;

        public int NumberOfSegments
        {
            get
            { 
                return numberOfSegments; 
            }
            set
            {
                if (value <= MAX_MULTIPLE_SEGMENTS)
                {
                    numberOfSegments = value;
                    OnPropertyChanged("NumberOfSegments");
                }
                else
                    throw new InvalidOperationException("[ViewModel:Flighst:SearchBox] Cannot add more than " + MAX_MULTIPLE_SEGMENTS  + " segments");
            }
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
                return new RelayCommand(() => { NumberOfSegments++; });
            }
        }

        public ICommand RemoveSegmentMultipleCommand
        {
            get
            {
                return new RelayCommand(() => { NumberOfSegments--; });
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
            coreSearchModel.DestinationDate = DateTimeOffset.MinValue; // TODO HACER EN EL SET PageMode del COREMODEL

            DoSearch();
        }

        private void SearchMultiples()
        {            
            coreSearchModel.PageMode = FlightSearchPages.Multiple;
            UpdatePassengers();
            coreSearchModel.DepartureDate = DateTimeOffset.MaxValue; // TODO HACER EN EL SET PageMode del COREMODEL
            coreSearchModel.DestinationDate = DateTimeOffset.MinValue; // TODO HACER EN EL SET PageMode del COREMODEL

            DoSearch();
        }

        private async void DoSearch()
        {
            if (coreSearchModel.IsValid())
            {
                IsLoading = true;
                FlightsItineraries intineraries = await flightService.GetItineraries(coreSearchModel);
                //TODO handle error with exceptions.

                IsLoading = false;
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