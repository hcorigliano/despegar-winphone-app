using Despegar.Core.Neo.Business.Configuration;
using Despegar.WP.UI.Model.ViewModel.Hotels;
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

namespace Despegar.WP.UI.Product.Hotels.Checkout
{
    public sealed partial class InvoiceArgentina : UserControl
    {
        public HotelsCheckoutViewModel ViewModel { get { return DataContext as HotelsCheckoutViewModel; } }

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
                    ViewModel.CoreBookingFields.form.Invoice.address.city_id.CoreValue = null;
                    string stateId = ViewModel.CoreBookingFields.form.Invoice.address.state.CoreValue;
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
            ViewModel.CoreBookingFields.form.Invoice.address.city_id.CoreValue = ((CitiesFields)args.SelectedItem).id;
        }

        private void AutoSuggestBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Back)
            {
                AutoSuggestBox asb = sender as AutoSuggestBox;

                if (asb != null)
                {
                    asb.Text = String.Empty;
                    ViewModel.CoreBookingFields.form.Invoice.address.city_id.CoreValue = null;
                }
            }
        }
    }
}