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
        }

        private void Focus_Lost(object sender, RoutedEventArgs e)
        {
            UpdateTextbox((AutoSuggestBox)sender);
        }
        
        private void UpdateTextbox(AutoSuggestBox control)
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
                        SelectedDestinationCode = city.code;
                        SelectedDestinationText = city.name;
                    }
                    else if (control.Name == "OriginInput")
                    {
                        SelectedOriginCode = city.code;
                        SelectedOriginText = city.name;
                    }
                }
                else
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
            }
            else
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
    }
}