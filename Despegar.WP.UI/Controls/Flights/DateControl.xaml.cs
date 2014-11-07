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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DateControl : UserControl
    {
        public DatePicker DepartureDateControl { get; set; }
        public DatePicker ReturnDateControl { get; set; }
        public DateControl()
        {
            this.InitializeComponent();
            DepartureDateControl = departure;
            ReturnDateControl = returnn;
        }
        
        private void departure_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            returnn.Date = departure.Date;
            //returnn.
        }
    }
}
