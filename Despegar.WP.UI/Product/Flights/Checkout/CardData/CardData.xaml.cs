using Despegar.Core.Business.Flight.BookingFields;
using Despegar.WP.UI.Model.ViewModel;
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

namespace Despegar.WP.UI.Product.Flights.Checkout.CardData
{
    public sealed partial class CardData : UserControl
    {       
        private FlightsCheckoutViewModel ViewModel { get { return DataContext as FlightsCheckoutViewModel; } }
        
        public CardData()
        {
            this.InitializeComponent();
        }

        private void FillExpiration(object sender, SelectionChangedEventArgs e)
        {
            if (YearCombo.SelectedValue != null && MonthCombo.SelectedValue != null)
            {
                ViewModel.CoreBookingFields.form.payment.card.expiration.CoreValue = YearCombo.SelectedValue.ToString() + "-" + MonthCombo.SelectedValue.ToString();
            }
        }
    }
}