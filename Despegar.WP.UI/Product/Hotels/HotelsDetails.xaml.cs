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
using Despegar.Core.Business.Hotels.CitiesAvailability;
using Windows.Phone.UI.Input;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Model;
using Despegar.WP.UI.BugSense;
using Despegar.WP.UI.Model.ViewModel.Hotels;


namespace Despegar.WP.UI.Product.Hotels
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HotelsDetails : Page
    {
        public HotelsDetailsViewModel ViewModel { get; set; }
        public HotelsDetails()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            if(ViewModel == null)
            {
                ViewModel = new HotelsDetailsViewModel(Navigator.Instance, GlobalConfiguration.CoreContext.GetHotelService(), BugTracker.Instance) { CrossParameters = e.Parameter as HotelsCrossParameters };
                await ViewModel.Init();
            }
            
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            Navigator.Instance.GoBack();
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
