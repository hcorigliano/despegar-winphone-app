﻿using Despegar.Core.Neo.Business.Flight.Itineraries;
using Despegar.Core.Neo.Contract.Log;
using Despegar.WP.UI.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model.ViewModel.Flights
{
    public class FlightOrderByViewModel : ViewModelBase
    {
        public Sorting OrderOptions { get; set; }

        public FlightOrderByViewModel(INavigator nav, IBugTracker t)
            : base(nav, t)
        {                
        }

        public override void OnNavigated(object navigationParams)
        {
            BugTracker.LeaveBreadcrumb("Flight search Sort By View");
            OrderOptions = navigationParams as Sorting;
        }
    }
}