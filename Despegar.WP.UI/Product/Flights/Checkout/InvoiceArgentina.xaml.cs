using Despegar.Core.Business.Configuration;
using Despegar.WP.UI.Model.ViewModel.Flights;
using System;
using System.Collections;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Despegar.WP.UI.Product.Flights.Checkout
{
   /// <summary>
   /// This control will also act as a ViewModel for this Checkout Section
   /// </summary>
    public sealed partial class InvoiceArgentina : UserControl
    {
        public FlightsCheckoutViewModel ViewModel { get { return DataContext as FlightsCheckoutViewModel; } }

        public InvoiceArgentina()
        {
            this.InitializeComponent();
        }

        private async void CityTexbox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {           
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput && sender.Text != "" && sender.Text.Length >= 3)
            {
                try 
                {
                   ViewModel.CoreBookingFields.form.payment.invoice.address.city_id.CoreValue = null;
                   string stateId = ViewModel.CoreBookingFields.form.payment.invoice.address.state.CoreValue;
                   sender.ItemsSource = (IEnumerable)(await ViewModel.GetCities("AR", sender.Text, stateId));
                }
                catch (Exception)
                {
                    // Do nothing, retry on next TextChanged
                }
            }
        }

        private void SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            ViewModel.CoreBookingFields.form.payment.invoice.address.city_id.CoreValue = ((CitiesFields)args.SelectedItem).id;
        }

        private void AutoSuggestBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Back)
            {
                AutoSuggestBox asb = sender as AutoSuggestBox;

                if (asb!=null)
                {
                    asb.Text = String.Empty;
                    ViewModel.CoreBookingFields.form.payment.invoice.address.city_id.CoreValue = null;
                }
            }
        }
    }
}