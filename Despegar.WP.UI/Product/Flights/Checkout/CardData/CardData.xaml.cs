﻿using Despegar.Core.Business.Flight.BookingFields;
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
        
        public CardData()
        {
            this.InitializeComponent();
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
                Despegar.Core.Business.Flight.BookingFields.Payment payments = this.DataContext as Despegar.Core.Business.Flight.BookingFields.Payment;
                payments.installment.bank_code.coreValue = item.card.bank;
                payments.installment.quantity.coreValue = item.installments.quantity.ToString();
                payments.installment.card_code.coreValue = item.card.company;
                payments.installment.card_type.coreValue = item.card.type;
                payments.installment.complete_card_code.coreValue = item.card.code;

            }
           

        }
    }
}
