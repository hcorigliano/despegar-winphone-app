using Despegar.Core.Business.Configuration;
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
        public Countries Countries {get; set;}
          

        public Passengers()
        {
            this.InitializeComponent();
            var a = this.DataContext;
            FillCountries();
        }

        private async void FillCountries()
        {
            IConfigurationService configurationService = GlobalConfiguration.CoreContext.GetConfigurationService();
            Countries = await configurationService.GetCountries();
        }

        private async void Autosuggest_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput && sender.Text != "" && sender.Text.Length >= 2)
            {
                nationality.ItemsSource = Countries.countries.Where(x => x.name.Contains(sender.Text)).ToList();
            }
        }

        private void SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            Despegar.Core.Business.Flight.BookingFields.Passenger context = (Despegar.Core.Business.Flight.BookingFields.Passenger)this.DataContext;
            context.nationality.coreValue = ((CountryFields)args.SelectedItem).id;
        }

    }
}
