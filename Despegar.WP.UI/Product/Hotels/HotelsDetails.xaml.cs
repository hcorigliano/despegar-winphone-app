using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Despegar.WP.UI.Model.ViewModel.Hotels;
using Despegar.Core.Business.Hotels.CitiesAvailability;


namespace Despegar.WP.UI.Product.Hotels
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HotelsDetails : Page
    {
        public HotelsDetailsViewModel hotelDetailViewModel { get; set; }
        public HotelsDetails()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        //private  void HotelMap_Tapped(object sender, TappedRoutedEventArgs e)
        //{
        //    HotelMap.Center = new Windows.Devices.Geolocation.Geopoint(new Windows.Devices.Geolocation.BasicGeoposition() { Latitude = -34.6653, Longitude = -58.7275 });
        //    HotelMap.ZoomLevel = 12;
        //    HotelMap.LandmarksVisible = true;

        //    //var pushpin = CreatePushPin();
        //    //HotelMap.Children.Add(pushpin);

        //    var location = new Windows.Devices.Geolocation.Geopoint(new Windows.Devices.Geolocation.BasicGeoposition() { Latitude = -34.6653, Longitude = -58.7275 });

        //    MapIcon mapicon = new MapIcon();
        //    mapicon.Location = location;
        //    mapicon.NormalizedAnchorPoint = new Point(0.5, 1.0);
        //    mapicon.Title = "Ehh Merlo loco";

        //    HotelMap.MapElements.Add(mapicon);
        //    //await HotelMap.TrySetViewAsync(location, 15D, 0, 0, Windows.UI.Xaml.Controls.Maps.MapAnimationKind.Bow);
            
        //}

       
    }
}
