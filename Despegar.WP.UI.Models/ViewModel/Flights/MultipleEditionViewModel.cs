﻿using Despegar.Core.Business.Flight.SearchBox;
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

namespace Despegar.WP.UI.Model.ViewModel.Flights
{
    public class MultipleEditionViewModel : ViewModelBase
    {
        private INavigator navigator;
        public int SelectedNavigationIndex;
        public FlightSearchModel coreSearchModel;
        public PassengersViewModel passengerModel;

        public ObservableCollection<FlightMultipleSegment> Segments { get; set; }

        public MultipleEditionViewModel(INavigator navigator) 
        {
            this.navigator = navigator;
        }

        public void Initialize(EditMultiplesNavigationData navigationData)
        { 
            this.coreSearchModel = navigationData.SearchModel;
            this.SelectedNavigationIndex = navigationData.SelectedSegmentIndex;
            this.passengerModel = navigationData.PassengerModel;

            // Make a copy in order to not modify the oringinal data, so the user can Cancel the changes.
            var copiedSegments = coreSearchModel.MultipleSegments.Select(
                x => new FlightMultipleSegment() { 
                Index = x.Index, AirportDestination = x.AirportDestination, 
                AirportOrigin = x.AirportOrigin, DepartureDate = x.DepartureDate, 
                AirportOriginText = x.AirportOriginText, AirportDestinationText = x.AirportDestinationText
            });         
   
            this.Segments = new ObservableCollection<FlightMultipleSegment>(copiedSegments);
        }

        public ICommand ApplyCommand
        {
            get
            {
                return new RelayCommand(() => 
                {
                    // Apply User Changes
                    coreSearchModel.MultipleSegments.Clear();
                    coreSearchModel.MultipleSegments.AddRange(Segments);

                    navigator.RemoveBackEntry(); // Remove the SearchBox page, go to new instance
                    navigator.GoTo(ViewModelPages.FlightsSearch, new FlightSearchNavigationData() {NavigatedFromMultiples = true,  SearchModel = coreSearchModel, PassengerModel = passengerModel});
                });
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return new RelayCommand(() => navigator.GoBack());
            }
        }      
    }
}