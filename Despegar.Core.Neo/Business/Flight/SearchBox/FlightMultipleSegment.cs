using Despegar.WP.UI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Business.Flight.SearchBox
{
    public class FlightMultipleSegment
    {
        public int Index { get; set; }
        public DateTimeOffset DepartureDate { get; set; }
        public string AirportOrigin { get; set; }
        public string AirportDestination { get; set; }
        public string AirportOriginText { get; set; }
        public string AirportDestinationText { get; set; }
        /// <summary>
        /// Indicates whether the Segment has just been created with empty values
        /// </summary>
        public bool IsNew { get { return String.IsNullOrWhiteSpace(AirportOrigin) && String.IsNullOrWhiteSpace(AirportDestination); } }
    }
}