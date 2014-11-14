using Despegar.Core.Log;
using Despegar.WP.UI.Controls.Flights;
using Despegar.WP.UI.Model.ViewModel.Flights;
using Despegar.WP.UI.Product.Flights;
using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Despegar.WP.UI.Common;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using Despegar.Core.Business.Flight.CitiesAutocomplete;
using Windows.UI.Popups;
using System;

namespace Despegar.WP.UI.Developer
{
    public sealed partial class DevTools : UserControl
    {
        private DeveloperViewModel ViewModel { get; set; }

        public DevTools()
        {
            this.InitializeComponent();
            this.DataContext = Window.Current.Bounds;

            // Load State
            ViewModel = new DeveloperViewModel();
            this.DataContext = ViewModel;

            // Default Picker Values
            this.OpacityPicker.SelectedValue = MetroGridHelper.Opacity;
            this.ColourPicker.SelectedColor = MetroGridHelper.Color;

            ShowDialogAnimation.Begin();
        }

        private void ClosePopup(object sender, RoutedEventArgs e)
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

        private void Button_FillIdaYVuelta(object sender, RoutedEventArgs e)
        {
            var page = (Window.Current.Content as Frame).Content as FlightSearch;
            if (page == null) 
            {
                Logger.Log("[Developer Tools]: Can't use this functionality in this page. Go to the correct page.");
                ShowInvalidMessage();
                return;
            }

            // Fill From EZE to MIA
            FlightSearchViewModel viewModel = page.DataContext as FlightSearchViewModel;
            //viewModel.Origin = "Aeropuerto Buenos Aires Ministro ¨Pistarini Ezeiza, Buenos Aires, Argentina";
            //viewModel.Destination = "Miami, Florida, Estados Unidos";
            viewModel.PassengersViewModel.GeneralAdults = 2;
            viewModel.PassengersViewModel.GeneralMinors = 1;
            viewModel.FromDate = new System.DateTimeOffset(2015, 2, 10, 0, 0, 0, TimeSpan.FromDays(0));
            viewModel.ToDate = new System.DateTimeOffset(2015, 3, 20, 0, 0, 0, TimeSpan.FromDays(0));

            // Update UI
            var userControl = page.FindVisualChildren<SearchAirport>().First();                        
            var originBox = userControl.FindName("OriginInput") as AutoSuggestBox;
            var destinationBox =userControl.FindName("DestinyInput") as AutoSuggestBox;

            originBox.ItemsSource = new List<CityAutocomplete>() { new CityAutocomplete() { code = "EZE", name = "Aeropuerto Buenos Aires Ministro ¨Pistarini Ezeiza, Buenos Aires, Argentina" } };
            destinationBox.ItemsSource = new List<CityAutocomplete>() { new CityAutocomplete() { code = "MIA", name = "Miami, Florida, Estados Unidos" } };

            originBox.Text = (originBox.ItemsSource as IEnumerable<CityAutocomplete>).First().name as string;
            destinationBox.Text = (destinationBox.ItemsSource as IEnumerable<CityAutocomplete>).First().name as string;

            userControl.UpdateTextbox(originBox);
            userControl.UpdateTextbox(destinationBox);
        }

        private void ShowInvalidMessage()
        {
            var msg = new MessageDialog("Not available for current View.");
            msg.ShowAsync();
        } 
    }
}