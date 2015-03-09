using Despegar.Core.Neo.Business.Hotels.HotelDetails;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Model.Interfaces;
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
    public sealed partial class RoomInformationControl : UserControl
    {
        public RoomInformationControl()
        {
            this.InitializeComponent();
        }

        private void ChangeRoomInformationVisibility(object sender, RoutedEventArgs e)
        {
            if (RoomInformationTextBlock.Visibility == Visibility.Collapsed)
                RoomInformationTextBlock.Visibility = Visibility.Visible;
            else
                RoomInformationTextBlock.Visibility = Visibility.Collapsed;
        }

        private void ShowAllAmenities(object sender, RoutedEventArgs e)
        {
            try
            {
                Roompack room = (Roompack)(((Button)sender).DataContext);
                Navigator.Instance.GoTo(ViewModelPages.HotelsAmenities, room.rooms[0].amenities);
            }
            catch
            {
                //Catch Error
            }
        }
    }
}
