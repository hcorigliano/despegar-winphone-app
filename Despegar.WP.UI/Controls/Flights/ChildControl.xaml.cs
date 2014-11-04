using Despegar.Core.Business.Enums;
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
    public sealed partial class ChildControl : UserControl
    {
        public int Quantity { get; set; }
        public FlightSearchChildEnum SelectedItemTag { get { return (FlightSearchChildEnum)((ComboBoxItem)ComboBoxOptions.SelectedItem).Tag; } }

        public ChildControl(int quantity)
        {
            Quantity = quantity + 1;
            this.InitializeComponent();
            
            var resources =  App.Current.Resources;

            var i = new ComboBoxItem() { Content = resources["Flights_Passager_Baby_In_Arms"], Tag = FlightSearchChildEnum.Infant };
            ComboBoxOptions.Items.Add(i);
            i = new ComboBoxItem() { Content = resources["Flights_Passager_Baby_In_Seat"], Tag = FlightSearchChildEnum.Child };
            ComboBoxOptions.Items.Add(i);
            i = new ComboBoxItem() { Content = resources["Flights_Passager_Up_To_11_Years"], Tag = FlightSearchChildEnum.Child };
            ComboBoxOptions.Items.Add(i);
            i = new ComboBoxItem() { Content = resources["Flights_Passager_Over_11_Years"], Tag = FlightSearchChildEnum.Adult };
            ComboBoxOptions.Items.Add(i);
            ComboBoxOptions.SelectedIndex = 0;

            this.IdChild.DataContext = this;
        }
    }
}
