using Despegar.Core.Business.Common.State;
using Despegar.WP.UI.Model;
using System;
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
    public sealed partial class InvoiceArg : Page
    {
        public InvoiceArg()
        {
            this.InitializeComponent();
            FillStates("AR");
            //address_state.ItemsSource =  ((States)FillStates("AR")).StatesList;

            
        }

        private async void FillStates(string country)
        {
            FlightsCheckoutModel flightCheckoutModel = new FlightsCheckoutModel();
            try
            {
                address_state.ItemsSource = await flightCheckoutModel.GetStates("AR");
            }
            catch { }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
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
    }
}
