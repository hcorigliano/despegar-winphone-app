using Despegar.Core.Business.Flight.CitiesAutocomplete;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Model;
using Despegar.WP.UI.Model.ViewModel.Flights;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Despegar.WP.UI.Controls.Flights
{    
    public sealed partial class SearchAirport : UserControl
    {
        public static readonly DependencyProperty SelectedOriginProperty = DependencyProperty.Register("SelectedOrigin", typeof(string), typeof(SearchAirport), null);
        public static readonly DependencyProperty SelectedDestinationProperty = DependencyProperty.Register("SelectedDestination", typeof(string), typeof(SearchAirport), null);

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
        public string SelectedOrigin
        {
            get { return (string)GetValue(SelectedOriginProperty); }
            set
            {
                SetValueAndNotify(SelectedOriginProperty, value);
            }
        }

        // Bindable Property from XAML
        public string SelectedDestination
        {
            get { return (string)GetValue(SelectedDestinationProperty); }
            set
            {
                SetValueAndNotify(SelectedDestinationProperty, value);
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
                //TODO : TRY CATCH
                sender.ItemsSource = (IEnumerable)(await GetCitiesAutocomplete(sender.Text));
            }
        }

        /// <summary>
        /// This method is included in this user control to facilitate the Code Reuse of it
        /// </summary>
        /// <param name="cityString"></param>
        /// <returns></returns>
        private async Task<CitiesAutocomplete> GetCitiesAutocomplete(string cityString)
        {
            var flightService = GlobalConfiguration.CoreContext.GetFlightService();  // There is no need to test this control with Unit Tests, so we inject this dependency directly
            return await flightService.GetCitiesAutocomplete(cityString);
        }

        private void SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            //  Saves ID city in InvoiceDefinition
            var selected = (CityAutocomplete)args.SelectedItem;
            if (selected != null)
            {
                if (sender.Name == "DestinyInput")
                {
                    SelectedDestination = selected.code;
                }
                else if (sender.Name == "OriginInput")
                {
                    SelectedOrigin = selected.code;
                }

                //For Fix Focus_Lost
                sender.ItemsSource = null;
                List<CityAutocomplete> source = new List<CityAutocomplete>();
                source.Add(selected);
                sender.ItemsSource = source;
            }
        }

        private void Focus_Lost(object sender, RoutedEventArgs e)
        {
            UpdateTextbox((AutoSuggestBox)sender);
        }
        
        public void UpdateTextbox(AutoSuggestBox control)
        {         
            // Force complete city when focus lost
            if (control.Text.Length > 2 && control.ItemsSource != null)
            {
                List<CityAutocomplete> cities = (List<CityAutocomplete>)control.ItemsSource;
                CityAutocomplete city = cities.FirstOrDefault();
                if (city != null)
                {
                    control.Text = city.name;
                    if (control.Name == "DestinyInput")
                    {
                        SelectedDestination = city.code;
                    }
                    else if (control.Name == "OriginInput")
                    {
                        SelectedOrigin = city.code;
                    }
                }
                else
                {
                    control.Text = "";
                    if (control.Name == "DestinyInput")
                    {
                        SelectedDestination = "";
                    }
                    else if (control.Name == "OriginInput")
                    {
                        SelectedOrigin = "";
                    }
                }
            }
            else
            {
                control.Text = "";
                if (control.Name == "DestinyInput")
                {
                    SelectedDestination = "";
                }
                else if (control.Name == "OriginInput")
                {
                    SelectedOrigin = "";
                }
            }
        }
        
    }
}