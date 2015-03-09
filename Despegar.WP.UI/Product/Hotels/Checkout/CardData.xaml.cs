using Despegar.WP.UI.Model.ViewModel.Hotels;
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

namespace Despegar.WP.UI.Product.Hotels.Checkout
{
    public sealed partial class CardData : UserControl
    {

        private HotelsCheckoutViewModel ViewModel { get { return DataContext as HotelsCheckoutViewModel; } }

        public CardData()
        {
            this.InitializeComponent();
        }

        private void FillExpiration(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CoreBookingFields.form.CardInfo.expiration.CoreValue = null;

            if (YearCombo.SelectedValue != null && MonthCombo.SelectedValue != null)
                ViewModel.CoreBookingFields.form.CardInfo.expiration.CoreValue = YearCombo.SelectedValue.ToString() + "-" + MonthCombo.SelectedValue.ToString();
        }

        private void Voucher_LostFocus(object sender, RoutedEventArgs e)
        {
            ViewModel.ValidateVoucher();
        }
    }
}