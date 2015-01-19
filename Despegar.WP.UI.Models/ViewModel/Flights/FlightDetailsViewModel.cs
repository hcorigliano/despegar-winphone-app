using Despegar.Core.Business.Flight.Itineraries;
using Despegar.Core.Log;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Model.ViewModel;
using Despegar.WP.UI.Model.ViewModel.Classes.Flights;
using Despegar.WP.UI.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Despegar.WP.UI.Model
{
    public class FlightDetailsViewModel : ViewModelBase
    {
        private INavigator navigator;
        public FlightsCrossParameter flightsCrossParameters { get; set; } 

        /// <summary>
        /// Inbound + Outbound Initialization
        /// </summary>
        /// <param name="outBound"></param>
        /// <param name="route2"></param>
        public FlightDetailsViewModel(INavigator navigator, FlightsCrossParameter parameters, IBugTracker t) : base(t) //Route outBound, Route inBound)
        {
            this.navigator = navigator;
            flightsCrossParameters = parameters;
        }

        public bool IsTwoWaySearch
        {
            get { return this.flightsCrossParameters.Inbound.segments != null; }
        }

        public ICommand BuyCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    // Todo send product data
                    navigator.GoTo(ViewModelPages.FlightsCheckout, flightsCrossParameters);
                });
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    navigator.GoBack();
                });
            }
        }
    }
}