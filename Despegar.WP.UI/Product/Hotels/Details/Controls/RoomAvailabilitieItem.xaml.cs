﻿using Despegar.Core.Neo.Business.Hotels.HotelDetails;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Despegar.WP.UI.Product.Hotels.Details.Controls
{
    public sealed partial class RoomAvailabilitieItem : UserControl
    {
        public RoomAvailabilitieItem()
        {
            this.InitializeComponent();
        }

        private void BuyButtonClick(object sender, RoutedEventArgs e)
        {
            ((RoomAvailability)this.DataContext).selectedRoom = true;
        }

    }
}