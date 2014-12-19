using Despegar.Core.Business.Flight.CitiesAutocomplete;
using Despegar.WP.UI.Common;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Despegar.WP.UI.Controls.Flights 
{
    public sealed partial class SearchCloseAirport : UserControl, IPopupContent
    {
        private SearchAirport searchAirport;
        private string SelectedName;
        //public string searchedAirport;

        //public SearchCloseAirport(SearchAirport searchAirport)
        //{
        //    this.InitializeComponent();
        //    this.searchAirport = searchAirport;
        //}

        public SearchCloseAirport(SearchAirport searchAirport, string selectedName , string airportName)
        {
            this.InitializeComponent();
            this.searchAirport = searchAirport;
            this.SelectedName = selectedName;

            searchedAirport.Text = airportName;

            mainCanvas.Margin = new Thickness((Window.Current.Bounds.Width - mainCanvas.Width) / 2, (Window.Current.Bounds.Height - mainCanvas.Height) / 2, 0, 0);
        }

        public void Enter()
        {
            Windows.UI.ViewManagement.InputPane.GetForCurrentView().TryHide();
            
            ShowDialogAnimation.Begin();
        }

        public void Leave()
        {
            HideDialogAnimation.Begin();
            HideDialogAnimation.Completed += DoClosePopup;  
        }

        private void DoClosePopup(object sender, object e)
        {
            // in this example we assume the parent of the UserControl is a Popup 
            Popup p = this.Parent as Popup;

            // close the Popup
            if (p != null) { p.IsOpen = false; }
        }

        private void ListView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            
            if (SelectedName == "DestinyInput")
            {
                searchAirport.UpdateAirportBoxesDestiny(((CityAutocomplete)((ListView)sender).SelectedItem).code, ((CityAutocomplete)((ListView)sender).SelectedItem).name);
            }

            if (SelectedName == "OriginInput")
            {
                searchAirport.UpdateAirportBoxesOrigin(((CityAutocomplete)((ListView)sender).SelectedItem).code, ((CityAutocomplete)((ListView)sender).SelectedItem).name);
            }

            Leave();
        }

    }
}
