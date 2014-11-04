using Despegar.Core.Business.Enums;
using Despegar.WP.UI.Model;
using Despegar.WP.UI.Model.Classes.Flights;
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
    public sealed partial class QuantityPassagersControl : UserControl
    {
        public PassagersQuantity Passagers = new PassagersQuantity();

        private List<object> _childAgeOptions;
        public List<object> ChildAgeOptions 
        { 
            get { 

            if(_childAgeOptions == null)
            {
                _childAgeOptions = new List<object>();
                var resources =  App.Current.Resources;

                _childAgeOptions.Add(new  { Content = resources["Flights_Passager_Baby_In_Arms"], Tag = FlightSearchChildEnum.Infant });
                _childAgeOptions.Add(new  { Content = resources["Flights_Passager_Baby_In_Seat"], Tag = FlightSearchChildEnum.Child });
                _childAgeOptions.Add(new  { Content = resources["Flights_Passager_Up_To_11_Years"], Tag = FlightSearchChildEnum.Child });
                _childAgeOptions.Add(new  { Content = resources["Flights_Passager_Over_11_Years"], Tag = FlightSearchChildEnum.Adult });
            }

                return _childAgeOptions;
           } 
        }

        public int ChildrenInFlights
        {
            get
            {
                //return ChildAgeStackPanel.Children.Where(x => (x as ChildControl).SelectedItemTag.Equals(Core.Business.Enums.FlightSearchChildEnum.Child)).Count();
                return 1;
            }
        }
        public int AdultsInFlights
        {
            get
            {
               // return ChildAgeStackPanel.Children.Where(x => (x as ChildControl).SelectedItemTag.Equals(Core.Business.Enums.FlightSearchChildEnum.Adult)).Count() + Passagers.AdultPassagerQuantity;
                return 1;
            }
        }
        public int InfantsInFlights
        {
            get
            {
                //return ChildAgeStackPanel.Children.Where(x => (x as ChildControl).SelectedItemTag.Equals(Core.Business.Enums.FlightSearchChildEnum.Infant)).Count();
                return 1;
            }
        }

        public QuantityPassagersControl()
        {
            this.InitializeComponent();

            Passagers.AdultPassagerQuantity = 1;
            Passagers.ChildPassagerQuantity = 0;

            this.DataContext = Passagers;
        }

        private void ReturnPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Hide All
            for (int i = 0; i < 7; i++)
                ((StackPanel)this.FindName("ChildrenAgePicker_" + i)).Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            // Show controls
            for (int i = 0; i < Passagers.ChildPassagerQuantity; i++)
                ((StackPanel)this.FindName("ChildrenAgePicker_" + i)).Visibility = Windows.UI.Xaml.Visibility.Visible;                    
        }

        //private void btnChildAdd_Click(object sender, RoutedEventArgs e)
        //{
        //    if (Passagers.ChildPassagerQuantity + Passagers.AdultPassagerQuantity < 8 )
        //    {
        //       ChildAgeStackPanel.Children.Add(new ChildControl(Passagers.ChildPassagerQuantity));
        //    }
        //    Passagers.AddChild();
        //}

        //private void btnAdultSub_Click(object sender, RoutedEventArgs e)
        //{            
        //    Passagers.SubAdult();
        //}

        //private void btnChildSub_Click(object sender, RoutedEventArgs e)
        //{
        //    if(Passagers.ChildPassagerQuantity  > 0)
        //    {
        //    ChildAgeStackPanel.Children.RemoveAt(Passagers.ChildPassagerQuantity - 1);
        //    }
        //    Passagers.SubChild();
        //}

        //private void Departure_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    Passagers.ResetAdults();

        //    for (int i = 0; i < ; i++)
        //       Passagers.AddAdult();
        //}

    }
}
