using Despegar.Core.Business.Flight.CitiesAutocomplete;
using Despegar.WP.UI.Model;
using System;
using System.Collections;
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
    public sealed partial class SearchAirport : UserControl
    {
        private FlightsSearchBoxModel FlightSearchBoxModel = new FlightsSearchBoxModel();  
        public string AirportOrigin { get; set; }
        public string AirportDestiny { get; set; }

        public SearchAirport()
        {
            this.InitializeComponent();            
        }


        private async void FlightsTextBlock_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {           
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput && sender.Text != "" && sender.Text.Length >= 3)
            {
                //TODO : TRY CATCH
                sender.ItemsSource = (IEnumerable)(await FlightSearchBoxModel.GetCities(sender.Text));
            }
        }

        private void SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            //  Saves ID city in InvoiceDefinition
            var selected = (CityAutocomplete)args.SelectedItem;
            if (selected != null)
            {
                if (sender.Name == "DestinyInput")
                {
                    AirportDestiny = selected.code;
                }
                else if (sender.Name == "OriginInput")
                {
                    AirportOrigin = selected.code;
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
            AutoSuggestBox _sender = (AutoSuggestBox)sender;

            // Force complete city when focus lost
            if (_sender.Text.Length > 2 && _sender.ItemsSource != null)
            {
                List<CityAutocomplete> cities = (List<CityAutocomplete>)_sender.ItemsSource;
                CityAutocomplete city = cities.FirstOrDefault();
                if (city != null)
                {
                    _sender.Text = city.name;
                    if (_sender.Name == "DestinyInput")
                    {
                        AirportDestiny = city.code;
                    }
                    else if (_sender.Name == "OriginInput")
                    {
                        AirportOrigin = city.code;
                    }
                }
                else
                {
                    _sender.Text = "";
                    if (_sender.Name == "DestinyInput")
                    {
                        AirportDestiny = "";
                    }
                    else if (_sender.Name == "OriginInput")
                    {
                        AirportOrigin = "";
                    }
                }
            }
            else
            {
                _sender.Text = "";
                if (_sender.Name == "DestinyInput")
                {
                    AirportDestiny = "";
                }
                else if (_sender.Name == "OriginInput")
                {
                    AirportOrigin = "";
                }
            }
        }
    }
}
