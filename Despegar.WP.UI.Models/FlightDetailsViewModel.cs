using Despegar.Core.Business.Flight.Itineraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model
{
    public class FlightDetailsViewModel
    {
        public Route OutModel { get; set; } // Also for Multiples      
        public Route InModel { get; set; }

        /// <summary>
        /// Inbound + Outbound Initialization
        /// </summary>
        /// <param name="outBound"></param>
        /// <param name="route2"></param>
        public FlightDetailsViewModel(Route outBound, Route inBound)
        {
            this.InModel = inBound;
            this.OutModel = outBound;            
        }
    }
}