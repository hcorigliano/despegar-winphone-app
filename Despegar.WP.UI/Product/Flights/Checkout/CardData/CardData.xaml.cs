using Despegar.Core.Business.Flight.BookingFields;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Despegar.WP.UI.Product.Flights.Checkout.CardData
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class CardData : UserControl
    {
        int[] Month = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        Despegar.Core.Business.Flight.BookingFields.Payment payments;
        

        public CardData()
        {
            this.InitializeComponent();
            this.Loaded += testing;
            MonthCombo.ItemsSource = Month;
        }

        private void testing(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                payments = this.DataContext as Despegar.Core.Business.Flight.BookingFields.Payment;
                string temp = payments.card.expiration.from;
                int fromDate = Convert.ToInt32((temp.Split(new Char[] { '-' }))[0]);
                temp = payments.card.expiration.to;
                int toDate = Convert.ToInt32((temp.Split(new Char[] { '-' }))[0]);

                List<int> list = new List<int>();
                for (int i = fromDate; i <= toDate; i++)
                {
                    list.Add(i);
                }
                YearCombo.ItemsSource = list;
            }
        }


        public void OnUCButtonClicked(object sender, RoutedEventArgs e)
        {
            RadioButton a = (RadioButton)e.OriginalSource;
            card.ItemsSource = a.DataContext;
            card.SelectedIndex = 0;
        }

        private void FillCardData(object sender, SelectionChangedEventArgs e)
        {
            PaymentDetail item = card.SelectedItem as PaymentDetail;
            if (item != null)
            {
                payments = this.DataContext as Despegar.Core.Business.Flight.BookingFields.Payment;
                payments.installment.bank_code.CoreValue = item.card.bank;
                payments.installment.quantity.CoreValue = item.installments.quantity.ToString();
                payments.installment.card_code.CoreValue = item.card.code;
                payments.installment.card_code.CoreValue = item.card.company;
                payments.installment.card_type.CoreValue = item.card.type;
                payments.installment.complete_card_code.CoreValue = item.card.code;
           }
        }

        private void FillExpiration(object sender, RoutedEventArgs e)
        {
            if (YearCombo.SelectedValue != null && MonthCombo.SelectedValue != null)
            {
                payments.card.expiration.coreValue = YearCombo.SelectedValue.ToString() + "-" + MonthCombo.SelectedValue.ToString();
            }
        }
    }
}
