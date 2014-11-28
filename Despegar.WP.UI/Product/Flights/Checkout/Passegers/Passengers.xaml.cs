using Despegar.Core.Business.Configuration;
using Despegar.Core.IService;
using Despegar.WP.UI.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace Despegar.WP.UI.Product.Flights.Checkout.Passegers
{
    public sealed partial class Passengers : UserControl, INotifyPropertyChanged
    {
        private List<CountryFields> countries;
        public List<CountryFields> Countries
        {
            get
            {
                return countries;
            }
            set 
            {
                countries = value;
                OnPropertyChanged();
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public Passengers()
        {
            this.InitializeComponent();
            var a = this.DataContext;

            FillCountries();
        }
          
        private async void FillCountries()
        {
            IConfigurationService configurationService = GlobalConfiguration.CoreContext.GetConfigurationService();
            Countries = (await configurationService.GetCountries()).countries;
            //nationality.Text = Countries.countries.FirstOrDefault(x => x.id == ((Passenger)this.DataContext).nationality.value).name;
            //NationalitySelection.DataContext = Countries;
        }

        private async void Autosuggest_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput && sender.Text != "" && sender.Text.Length >= 2)
            {
                //nationality.ItemsSource = Countries.countries.Where(x => x.name.Contains(sender.Text)).ToList();
            }
        }

        private void SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            Despegar.Core.Business.Flight.BookingFields.Passenger context = (Despegar.Core.Business.Flight.BookingFields.Passenger)this.DataContext;
            context.nationality.coreValue = ((CountryFields)args.SelectedItem).id;
        }

        private async void SelectNationality(object sender, RoutedEventArgs e)
        {
            IConfigurationService configurationService = GlobalConfiguration.CoreContext.GetConfigurationService();
            //Countries = await configurationService.GetCountries();
            //Navigator.GoTo(CountrySelection, null);
            //navigator.GoTo(ViewModelPages.FlightsCheckout, null);
        }

    }
}
