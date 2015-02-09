using Despegar.Core.Neo.Business.Flight.CitiesAutocomplete;
using Despegar.Core.Neo.Contract.API;
using Despegar.Core.Neo.InversionOfControl;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Model;
using Despegar.WP.UI.Model.ViewModel.Flights;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Despegar.WP.UI.Controls.Flights
{    
    public sealed partial class SearchAirport : UserControl
    {
        public static readonly DependencyProperty SelectedOriginCodeProperty = DependencyProperty.Register("SelectedOriginCode", typeof(string), typeof(SearchAirport), null);
        public static readonly DependencyProperty SelectedDestinationCodeProperty = DependencyProperty.Register("SelectedDestinationCode", typeof(string), typeof(SearchAirport), null);

        public static readonly DependencyProperty SelectedOriginTextProperty = DependencyProperty.Register("SelectedOriginText", typeof(string), typeof(SearchAirport), null);
        public static readonly DependencyProperty SelectedDestinationTextProperty = DependencyProperty.Register("SelectedDestinationText", typeof(string), typeof(SearchAirport), null);

        // Aux properties for setting Initial Autosuggestbox values
        public static readonly DependencyProperty InitialOriginTextProperty = DependencyProperty.Register("InitialOriginText", typeof(string), typeof(SearchAirport), null);
        public static readonly DependencyProperty InitialDestinationTextProperty = DependencyProperty.Register("InitialDestinationText", typeof(string), typeof(SearchAirport), null);

        #region ** BoilerPlate Code **
        public event PropertyChangedEventHandler PropertyChanged;
        private void SetValueAndNotify(DependencyProperty property, object value, [CallerMemberName] string p = null)
        {
            SetValue(property, value);
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }
        #endregion

        // Bindable Property from XAML
        public string SelectedOriginCode
        {
            get { return (string)GetValue(SelectedOriginCodeProperty); }
            set
            {
                SetValueAndNotify(SelectedOriginCodeProperty, value);
            }
        }

        // Bindable Property from XAML
        public string SelectedDestinationCode
        {
            get { return (string)GetValue(SelectedDestinationCodeProperty); }
            set
            {
                SetValueAndNotify(SelectedDestinationCodeProperty, value);
            }
        }

        // Bindable Property from XAML
        public string SelectedOriginText
        {
            get { return (string)GetValue(SelectedOriginTextProperty); }
            set
            {
                SetValueAndNotify(SelectedOriginTextProperty, value);
            }
        }

        // Bindable Property from XAML
        public string SelectedDestinationText
        {
            get { return (string)GetValue(SelectedDestinationTextProperty); }
            set
            {
                SetValueAndNotify(SelectedDestinationTextProperty, value);
            }
        }

        // Bindable Property from XAML (This property is OneTime binding only. It is used to set the Autosuggest Text from XAML the first time the control is loaded.)
        public string InitialOriginText
        {
            get { return (string)GetValue(InitialOriginTextProperty); }
            set
            {
                SetValueAndNotify(InitialOriginTextProperty, value);
            }
        }

        // Bindable Property from XAML (This property is OneTime binding only. It is used to set the Autosuggest Text from XAML the first time the control is loaded.)
        public string InitialDestinationText
        {
            get { return (string)GetValue(InitialDestinationTextProperty); }
            set
            {
                SetValueAndNotify(InitialDestinationTextProperty, value);
            }
        }

        // DataContext is the FlightSearchViewModel
        public SearchAirport()
        {
            this.InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;
        }
       
        private async void FlightsTextBlock_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput && sender.Text != "" && sender.Text.Length >= 3)
            {
                try
                {
                    sender.ItemsSource = (IEnumerable)(await GetCitiesAutocomplete(sender.Text));
                }
                catch (Exception) {
                    // do nothing. Try again...
                }
            }
        }

        /// <summary>
        /// This method is included in this user control to facilitate the Code Reuse of it
        /// </summary>
        /// <param name="cityString"></param>
        /// <returns></returns>
        private async Task<CitiesAutocomplete> GetCitiesAutocomplete(string cityString)
        {
            var flightService = IoC.Resolve<IMAPIFlights>();  // There is no need to test this control with Unit Tests, so we inject this dependency directly
            return await flightService.GetCitiesAutocomplete(cityString);
        }

        private async Task<CitiesAutocomplete> GetNearCities(double latitude , double longitude)
        {
            var flightService = IoC.Resolve<IMAPIFlights>();  // There is no need to test this control with Unit Tests, so we inject this dependency directly
            return await flightService.GetNearCities(latitude,longitude);
        }

        private async void SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            //  Saves ID city in InvoiceDefinition
            var selected = (CityAutocomplete)args.SelectedItem;
            if (selected != null)            
                await SetCity(sender, selected);            
        }

        private async Task SetCity(AutoSuggestBox sender, CityAutocomplete selected)
        {
            if (selected.type == "city" && !selected.has_airport)
            {
                // No tiene aeropuerto, buscar un aeropuerto cercano
                var data = await GetNearCities(selected.geo_location.latitude, selected.geo_location.longitude);
                if (data != null)
                {
                    SearchCloseAirport SearchAirport = new SearchCloseAirport(this, sender.Name, selected.name) { DataContext = data };
                    ModalPopup popup = new ModalPopup(SearchAirport);
                    popup.Show();
                                        
                    sender.IsSuggestionListOpen = false;                    
                }
                else
                {
                    ResourceLoader manager = new ResourceLoader();
                    MessageDialog dialog = new MessageDialog(manager.GetString("Flight_SearchAirport_Error"), "Error");
                    await dialog.ShowSafelyAsync();
                    sender.Text = "";
                }
            }
            else
            {
                sender.Text = selected.name;
                if (sender.Name == "DestinyInput")
                {
                    SelectedDestinationCode = selected.code;
                    SelectedDestinationText = selected.name;
                }
                else if (sender.Name == "OriginInput")
                {
                    SelectedOriginCode = selected.code;
                    SelectedOriginText = selected.name;
                }
                //For Fix Focus_Lost
                sender.ItemsSource = null;
                List<CityAutocomplete> source = new List<CityAutocomplete>();
                source.Add(selected);
                sender.ItemsSource = source;
            }

            UnFocus();
        }

        private static void UnFocus()
        {
            ((Control)Window.Current.Content).Focus(Windows.UI.Xaml.FocusState.Programmatic); // loose textbox focus
        }

        private void Focus_Lost(object sender, RoutedEventArgs e)
        {
            // Force pick city when focus lost
            UpdateTextbox((AutoSuggestBox)sender);
        }
        
        private void UpdateTextbox(AutoSuggestBox control)
        {
            bool selectionEmpty = false;

            if (control.Name == "DestinyInput")
            {
                selectionEmpty = SelectedDestinationCode == "";
            }
            else if (control.Name == "OriginInput")
            {
                selectionEmpty = SelectedOriginCode == "";
            }

            //if (selectionEmpty && control.Text.Length > 2)
            //{
            //CityAutocomplete city = ((List<CityAutocomplete>)control.ItemsSource).FirstOrDefault();

            //if (city != null)                
            //    await SetCity(control, city);                           
            //
            //}

            if (selectionEmpty)
                Clear(control);                                
        }

        /// <summary>
        /// Sets city to none, clears the control
        /// </summary>
        /// <param name="control"></param>
        private void Clear(AutoSuggestBox control)
        {
            control.Text = "";
            if (control.Name == "DestinyInput")
            {
                SelectedDestinationCode = "";
                SelectedDestinationText = "";
            }
            else if (control.Name == "OriginInput")
            {
                SelectedOriginCode = "";
                SelectedOriginText = "";
            }
        }

        /// <summary>
        /// Initially sets the Ui values of the Autosuggest boxes, Used for DEV TOOLS
        /// </summary>        
        public void UpdateAirportBoxes(string originCode, string originText, string destinationCode, string destinationText)
        {
            OriginInput.ItemsSource = new List<CityAutocomplete>() { new CityAutocomplete() { code = originCode, name = originText } };
            DestinyInput.ItemsSource = new List<CityAutocomplete>() { new CityAutocomplete() { code = destinationCode, name = destinationText } };

            OriginInput.Text = originText;
            DestinyInput.Text = destinationText;

            UpdateTextbox(OriginInput);
            UpdateTextbox(DestinyInput);
        }

        public void UpdateAirportBoxesOrigin(string originCode, string originText)
        {           
            SetCity(OriginInput, new CityAutocomplete() { code = originCode, name = originText });
        }

        public void UpdateAirportBoxesDestiny( string destinationCode, string destinationText)
        {
            SetCity(DestinyInput, new CityAutocomplete() { code = destinationCode, name = destinationText });
        }

        private void OriginInput_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Back && SelectedOriginCode != "") 
            {
                Clear((AutoSuggestBox)sender);
            }
        }

        private void DestinationInput_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Back && SelectedDestinationCode != "")
            {
                Clear((AutoSuggestBox)sender);
            }
        }
        
    }
}