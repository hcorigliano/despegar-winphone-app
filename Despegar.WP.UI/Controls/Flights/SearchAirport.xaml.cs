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
    public sealed partial class SearchAirport : Page
    {
        private FlightsSearchBoxModel FlightSearchBoxModel = new FlightsSearchBoxModel();
        public AutoSuggestBox OriginAirportControl{set;get;}
        public AutoSuggestBox DestinyAirportControl { set; get; }

        public SearchAirport()
        {
            this.InitializeComponent();
            OriginAirportControl = origin;
            DestinyAirportControl = destiny;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }


        private async void FlightsTextBlock_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (sender.Text != "" && sender.Text.Length >= 3)
            {

                sender.ItemsSource = (IEnumerable)(await FlightSearchBoxModel.GetCities(sender.Text));
            }
        }

        private void OriginFlightsTextBlock_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                // TODO: 

            }
        }  
    }
}
