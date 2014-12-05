using Despegar.Core.Business.Common.State;
using Despegar.Core.Business.Configuration;
using Despegar.Core.Business.Flight.BookingFields;
using Despegar.WP.UI.Model;
using Despegar.WP.UI.Model.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Despegar.WP.UI.Product.Flights.Checkout.Invoice
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
    }
}