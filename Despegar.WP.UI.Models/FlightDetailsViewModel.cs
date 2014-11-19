using Despegar.Core.Business.Flight.Itineraries;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Despegar.WP.UI.Model
{
    public class FlightDetailsViewModel
    {
        private INavigator navigator;
        public Route OutModel { get; set; } // Also for Multiples
        public Route InModel { get; set; }

        /// <summary>
        /// Inbound + Outbound Initialization
        /// </summary>
        /// <param name="outBound"></param>
        /// <param name="route2"></param>
        public FlightDetailsViewModel(INavigator navigator, Route outBound, Route inBound)
        {
            this.navigator = navigator;
            this.InModel = inBound;
            this.OutModel = outBound;            
        }

        public bool IsTwoWaySearch
        {
            get { return this.InModel.segments != null; }
        }

        public ICommand BuyCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    // Todo send product data
                    navigator.GoTo(ViewModelPages.FlightsCheckout, null);
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