using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Despegar.WP.UI.Controls.Maps
{
    public sealed partial class CustomMapControl : UserControl
    {
        private CustomMapViewModel mapViewModel;

        public CustomMapControl()
        {
            this.InitializeComponent();

#if DEBUG

            //Test despegar.com
            //AjES8G-jtTGtkPzU7SY2cW6xa_Rp6HOvgzd0JVehbeKpMQyG2DN0X__wqB_NhGaH

            mainMap.MapServiceToken = "AjES8G-jtTGtkPzU7SY2cW6xa_Rp6HOvgzd0JVehbeKpMQyG2DN0X__wqB_NhGaH";
#endif

#if RELEASE
            //prod despegar.com
            //Ao5ktWI7RMpfZRyvcpOXEN-sgd05y8FzZG4I5GrrFRIb8GJ-zy4uvveWWHc8GlCs

            mainMap.MapServiceToken = "Ao5ktWI7RMpfZRyvcpOXEN-sgd05y8FzZG4I5GrrFRIb8GJ-zy4uvveWWHc8GlCs";
#endif

            
            mapViewModel = new CustomMapViewModel();

            //mapViewModel.Locations.Add(new CustomPinPoint() { Latitude = -34.6653 , Longitude=-58.7275 , Title="Hotel 1", Address="calle falsa 123, republica de merlo ......."});

            //mapViewModel.Locations.Add(new CustomPinPoint() { Latitude = -34.6650, Longitude = -58.7270, Title = "Hotel 2" });

            //TODO the following latitude and longitude city: guatemala  14.5611100 -90.7344400

            mainMap.Center = new Windows.Devices.Geolocation.Geopoint(new Windows.Devices.Geolocation.BasicGeoposition() { Latitude = 14.5611100, Longitude = -90.7344400 });
            mainMap.ZoomLevel = 2;

            DataContext = mapViewModel;
        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        //private void mainMap_Tapped(object sender, TappedRoutedEventArgs e)
        //{
        //    var location = new Windows.Devices.Geolocation.Geopoint(new Windows.Devices.Geolocation.BasicGeoposition() { Latitude = -34.6652, Longitude = -58.7265 });

        //    MapIcon mapicon = new MapIcon();
        //    //mapicon.Location = location;
        //    //mapicon.NormalizedAnchorPoint = new Point(0.5, 1.0);
        //    //mapicon.Title = "hola hola";

        //    mapicon.Location = mapViewModel.Locations.FirstOrDefault().Geopoint;
        //    mapicon.NormalizedAnchorPoint = mapViewModel.Locations.FirstOrDefault().Anchor;
        //    mapicon.Title = mapViewModel.Locations.FirstOrDefault().Title;

        //    mainMap.MapElements.Add(mapicon);
        //}

    }
}
