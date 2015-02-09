using Despegar.WP.UI.Common;
using Despegar.WP.UI.Model.Interfaces;
//using Despegar.WP.UI.Model.ViewModel.Hotels;
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


namespace Despegar.WP.UI.Controls.Hotels
{
    public sealed partial class HotelDescriptionControl : UserControl
    {
        public HotelDescriptionControl()
        {
            this.InitializeComponent();
        }

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {

            HotelsDetailsViewModel model = this.DataContext as HotelsDetailsViewModel;

            if (model != null)
            {
                Navigator.Instance.GoTo(ViewModelPages.HotelsAmenities, model.HotelDetail.hotel.amenities);
            }
        }
    }
}
