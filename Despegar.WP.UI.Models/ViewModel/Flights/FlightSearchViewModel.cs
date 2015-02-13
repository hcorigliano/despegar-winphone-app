using Despegar.Core.Business.Configuration;
using Despegar.Core.Business.Enums;
using Despegar.Core.Business.Flight;
using Despegar.Core.Business.Flight.CitiesAutocomplete;
using Despegar.Core.Business.Flight.Itineraries;
using Despegar.Core.Business.Flight.SearchBox;
using Despegar.Core.IService;
using Despegar.Core.Log;
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
        public INavigator Navigator { get; set; }
        public PassengersViewModel PassengersViewModel { get; set; }
        private IFlightService flightService { get; set; }
        private FlightSearchModel coreSearchModel;

        #region ** Exposed Properties **

        public ObservableCollection<FlightMultipleSegment> MultipleSegments
        {
            get { return new ObservableCollection<FlightMultipleSegment>(coreSearchModel.MultipleSegments); }
        }

        public void AddMultipleSegmentsMock(List<FlightMultipleSegment> value)
        {
            if (coreSearchModel != null && value != null)
            {
                coreSearchModel.MultipleSegments.Clear();
                coreSearchModel.MultipleSegments.AddRange(value);
            }
        }

        public string Origin
        {
            get
            {
                return coreSearchModel.OriginFlight;
            }
            set
            {
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

        public string OriginText
        {
            get
            {
                return coreSearchModel.OriginFlightText;
            }
            set
            {
                coreSearchModel.OriginFlightText = value;
                OnPropertyChanged();
            }
        }

        public string DestinationText
        {
            get { return coreSearchModel.DestinationFlightText; }
            set
            {
                coreSearchModel.DestinationFlightText = value;
                OnPropertyChanged();
            }
        }

        public DateTimeOffset FromDate
        {
            get { return coreSearchModel.DepartureDate; }
            set
            {
                coreSearchModel.DepartureDate = value;

                if (coreSearchModel.PageMode == FlightSearchPages.RoundTrip)
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

        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(() => Search());
            }
        }

        public ICommand EditMultipleSegment
        {
            get
            {
                // TODO navigate to 
                return new RelayCommand<ItemClickEventArgs>((x) =>
                  {
                      int segmentIndex = (x.ClickedItem as FlightMultipleSegment).Index;
                      Navigator.GoTo(ViewModelPages.FlightsMultiplEdit, new EditMultiplesNavigationData() { SelectedSegmentIndex = segmentIndex, SearchModel = coreSearchModel, PassengerModel = PassengersViewModel });
                  });
            }
        }

        #endregion

        public FlightSearchViewModel(INavigator navigator, IFlightService flightService, IBugTracker t) : base(t)
        {
            this.Navigator = navigator;
            this.flightService = flightService;
            this.coreSearchModel = new FlightSearchModel();
            this.PassengersViewModel = new PassengersViewModel(t);

            GetParameterSearchfromConfiguration(this.coreSearchModel);

            coreSearchModel.UpdateSearchDays();
        }

        private void GetParameterSearchfromConfiguration(FlightSearchModel model)
        {
            Configuration conf = GlobalConfiguration.CoreContext.GetConfiguration();
            string site = GlobalConfiguration.Site;
            

            if (site == "BR" || site == "MX") 
            {
                model.EmissionAnticipationDay = 0;
                model.LastAvailableHours = 19;
            } else {
                var site2return = conf.sites.Where(s => s.code == site).FirstOrDefault();
                if (site2return == null)
                    return;

                var _s = site2return.products.Where(p => p.name == "flights").FirstOrDefault();
                if (_s == null)
                    return;

                model.EmissionAnticipationDay = _s.emission_anticipation_days;
                int last = 0;

                try
                {
                    last = Convert.ToInt32(_s.last_available_hour);
                }
                catch (Exception)
                {
                    last = 0;
                }

                model.LastAvailableHours = last;
            }


        }

        private async void Search()
        {
            coreSearchModel.SearchStatus = Core.Business.SearchStates.FirstSearch;
            UpdatePassengers();

            if (coreSearchModel.IsValid)
            {
                IsLoading = true;
                
                string airports = String.Format("{0} - {1}", 
                    coreSearchModel.OriginFlightText, coreSearchModel.DestinationFlightText);

                string extra = String.Format("{0} to {1}, [Passengers]: {2}",
                    coreSearchModel.DepartureDate.ToString("yyyy-MM-dd"),
                    coreSearchModel.DestinationDate != null ? coreSearchModel.DestinationDate.ToString("yyyy-MM-dd") : "-",
                    "Adults " + coreSearchModel.AdultsInFlights + " Child: " + coreSearchModel.ChildrenInFlights);

                Tracker.LeaveBreadcrumb("Flight Search performed");
                Tracker.SetExtraData("LastFlightAirports", airports);
                Tracker.SetExtraData("LastFlightExtra", extra);                

                try
                {

                    FlightsItineraries intineraries = await flightService.GetItineraries(coreSearchModel);

                    if (intineraries.items.Count != 0)
                    {
                        var pageParameters = new PageParameters();
                        pageParameters.Itineraries = intineraries;
                        pageParameters.SearchModel = coreSearchModel;

                        Navigator.GoTo(ViewModelPages.FlightsResults, pageParameters);

                    }
                    else
                    {

                        var msg = new MessageDialog("Lo sentimos, no hemos encontrado ningún resultado para su búsqueda.Por favor, inténtelo nuevamente modificando alguno de los criterios de búsqueda. ");
                        await msg.ShowAsync();

                    }
                }
                catch (Exception ex)
                {
                    OnViewModelError("SEARCH_FAILED");
                }
                finally
                {
                    IsLoading = false;
                }
            }
            else
            {
                OnViewModelError("SEARCH_INVALID_WITH_MESSAGE", coreSearchModel.SearchErrors);
            }
        }

        private void UpdatePassengers()
        {
            coreSearchModel.AdultsInFlights = PassengersViewModel.Adults;
            coreSearchModel.ChildrenInFlights = PassengersViewModel.Children;
            coreSearchModel.InfantsInFlights = PassengersViewModel.Infants;
        }

        /// <summary>
        /// Used to Load a search model in the ViewModel
        /// </summary>
        /// <param name="model"></param>
        /// <param name="passengerModel"></param>
        public void InitializeWith(FlightSearchModel model, PassengersViewModel passengerModel)
        {
            coreSearchModel = model;
            PassengersViewModel = passengerModel;

            // Notify Changes
            OnPropertyChanged("MultipleSegments");
            OnPropertyChanged("FromDate");
            OnPropertyChanged("To");
            OnPropertyChanged("Origin");
            OnPropertyChanged("Destination");
        }

        public void SetSearchMode(FlightSearchPages mode)
        {
            coreSearchModel.PageMode = mode;
        }
    }
}