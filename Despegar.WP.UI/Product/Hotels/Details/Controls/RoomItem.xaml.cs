using Despegar.Core.Neo.Business.Hotels.HotelDetails;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Model.Interfaces;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Despegar.WP.UI.Product.Hotels.Details.Controls
{
    public sealed partial class RoomItem : UserControl
    {
        public Button SelectedRoomButton;
        public RoomItem()
        {
            this.InitializeComponent();
            this.Width = Window.Current.Bounds.Width - 48;
        }


        private void ShowMoreFaresClick(object sender, RoutedEventArgs e)
        {
            Roompack roomPack = new Roompack();
            roomPack = (Roompack)this.DataContext;

            roomPack.room_availabilities.RemoveAt(0);
            foreach(RoomAvailability roomAvailability in roomPack.room_availabilities)
            {
                RoomAvailabilitieItem item = new RoomAvailabilitieItem();
                item.DataContext = roomAvailability;
                MoreFaresStackPanel.Children.Add(item);
            }

            ShowMoreFaresButton.Visibility = Visibility.Collapsed;
            ShowLessFaresButton.Visibility = Visibility.Visible;
        }

        private void ShowLessFaresClick(object sender, RoutedEventArgs e)
        {
            object test = MoreFaresStackPanel.Children;

            while (MoreFaresStackPanel.Children.Count() != 0)
            {
                MoreFaresStackPanel.Children.RemoveAt(0);
            }

            ShowMoreFaresButton.Visibility = Visibility.Visible;
            ShowLessFaresButton.Visibility = Visibility.Collapsed;
        }


    }
}
