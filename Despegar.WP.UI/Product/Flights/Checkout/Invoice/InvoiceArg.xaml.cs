using Despegar.Core.Business.Common.State;
using Despegar.Core.Business.Configuration;
using Despegar.Core.Business.Flight.BookingFields;
using Despegar.WP.UI.Model;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Despegar.WP.UI.Product.Flights.Checkout.Invoice
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InvoiceArg : UserControl
    {
        public InvoiceArg()
        {
            this.InitializeComponent();
            FillStates("AR");
        }

        private async void FillStates(string country)
        {
            //TODO : FIXIT
            FlightsCheckoutModel flightCheckoutModel = new FlightsCheckoutModel();
            try
            {
                address_state.ItemsSource = await flightCheckoutModel.GetStates("AR");
            }
            catch { }
        }

        private async void FlightsTextBlock_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput && sender.Text != "" && sender.Text.Length >= 3)
            {
                //TODO : TRY CATCH
                State state = (State)address_state.SelectedItem;
                sender.ItemsSource = (IEnumerable)(await FlightsCheckoutModel.GetCities("AR", sender.Text, state.id));
            }
        }

        private void FiscalStatus_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedValue.ToString() == "FINAL")
            {
                fiscal_name_StackPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                fiscal_name_StackPanel.Visibility = Visibility.Visible;
            }
        }

        private void Focus_Lost(object sender, RoutedEventArgs e)
        {
            //TODO: Agarrar el primero o si no hay nada , dejarlo en blanco.
        }

        private void SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {

            Despegar.Core.Business.Flight.BookingFields.Invoice context = (Despegar.Core.Business.Flight.BookingFields.Invoice)this.DataContext;
            context.address.city_id.coreValue = ((CitiesFields)args.SelectedItem).id;
        }
    }
}
