using Despegar.Core.Business.Flight.Itineraries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Despegar.WP.UI.Controls.Flights
{
    //public FlightSegmentTrip origin {get;set;}

    public sealed partial class FlightSegment : UserControl
    {
        //public StackPanel SegmentControl { get; set; }

        public FlightSegment()
        {
            this.InitializeComponent();
        }

    }
}
