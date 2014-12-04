using Despegar.Core.Business.Configuration;
using Despegar.Core.Business.Flight.BookingFields;
using Despegar.Core.IService;
using Despegar.WP.UI.Model;
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


namespace Despegar.WP.UI.Product.Flights.Checkout.Passegers
{
    public sealed partial class Passengers : UserControl
    {
        public FlightsCheckoutModel ViewModel { get { return DataContext as FlightsCheckoutModel; } }

        public Passengers()
        {
            this.InitializeComponent();           
        }

        private void Autosuggest_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput && sender.Text != "" && sender.Text.Length >= 2)
            {
                sender.ItemsSource = ViewModel.Countries.Where(x => x.name.ToUpper().Contains(sender.Text.ToUpper())).ToList();
            }
        }

        private void SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            Passenger context = (Passenger)sender.DataContext;
            context.nationality.CoreValue = ((CountryFields)args.SelectedItem).id;
        }

    }
}
