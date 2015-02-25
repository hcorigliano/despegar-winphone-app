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
        public RoomItem()
        {
            this.InitializeComponent();
            this.Width = Window.Current.Bounds.Width - 48;
        }

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                Room room = (Room)(((TextBlock)sender).DataContext);
                Navigator.Instance.GoTo(ViewModelPages.HotelsAmenities, room.amenities);
            }
            catch
            {
                //Catch Error
            }

        }

        private void ChangeRoomInformationVisibility(object sender, RoutedEventArgs e)
        {
            if (RoomInformationTextBlock.Visibility == Visibility.Collapsed)
                RoomInformationTextBlock.Visibility = Visibility.Visible;
            else
                RoomInformationTextBlock.Visibility = Visibility.Collapsed;
        }

        private void ShowMoreFaresClick(object sender, RoutedEventArgs e)
        {
            RoomAvailabilitieItem test = new RoomAvailabilitieItem();
            test.DataContext = this.DataContext;

            MoreFaresStackPanel.Children.Add(test);

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
