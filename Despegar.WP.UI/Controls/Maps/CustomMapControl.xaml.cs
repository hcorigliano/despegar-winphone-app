using Despegar.WP.UI.Model.ViewModel.Classes;
using Despegar.WP.UI.Model.ViewModel.Controls.Maps;
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


            //mapViewModel.Locations.Add(new CustomPinPoint() { Latitude = -34.6653, Longitude = -58.7275, Title = "Hotel 1", Address = "3456789, republica de merlo _-_-__-_-_" });

            //mapViewModel.Locations.Add(new CustomPinPoint() { Latitude = -34.6653, Longitude = -59.7280, Title = "Hotel 2", Address = "poi 234567, imperio de merlo _-_-__-_-_" });

            //mapViewModel.Locations.Add(new CustomPinPoint() { Latitude = -34.6653, Longitude = -57.7290, Title = "Hotel 3", Address = " hkj 123, merlo imperio _-_-__-_-_" });

            //mapViewModel.Locations.Add(new CustomPinPoint() { Latitude = -34.6653, Longitude = -56.7250, Title = "Hotel 4", Address = "jklddjklsdjkl  123, republica de merlo _-_-__-_-_" });

            //mapViewModel.Locations.Add(new CustomPinPoint() { Latitude = -34.6650, Longitude = -58.7270, Title = "Hotel 2" });

            //TODO the following latitude and longitude city: guatemala  14.5611100 -90.7344400

            mainMap.Center = new Windows.Devices.Geolocation.Geopoint(new Windows.Devices.Geolocation.BasicGeoposition() { Latitude = 14.5611100, Longitude = -90.7344400 });
            mainMap.ZoomLevel = 2;

            DataContext = mapViewModel;
        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Image image = sender as Image;

            if (image == null) return;

            CustomPinPoint custompinpoint = image.DataContext as CustomPinPoint;

            if (custompinpoint == null) return;

            mainMap.Center = new Windows.Devices.Geolocation.Geopoint(new Windows.Devices.Geolocation.BasicGeoposition() { Latitude = custompinpoint.Latitude, Longitude = custompinpoint.Longitude });
            mainMap.ZoomLevel = 17.5;

        }

        private void mainMap_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if(args.NewValue != null)
            {
                CustomMapViewModel viewmodel = args.NewValue as CustomMapViewModel;

                if (viewmodel == null) return;

                IEnumerable<CustomPinPoint> elements = viewmodel.Locations;

                if (elements == null) return;

                if (elements.Count() == 1)
                {
                    CustomPinPoint pinpoint = elements.FirstOrDefault();
                    mainMap.Center = new Windows.Devices.Geolocation.Geopoint(new Windows.Devices.Geolocation.BasicGeoposition() { Latitude = pinpoint.Latitude, Longitude = pinpoint.Longitude });
                    mainMap.ZoomLevel = 17.5;
                }
                
                if (elements.Count()>1)
                {
                    Random randomNumber = new Random((int)DateTime.Now.Ticks);
                    int random = randomNumber.Next(elements.Count());

                    IEnumerable<CustomPinPoint> pinpointList = elements.Take(random);

                    CustomPinPoint pinpoint = pinpointList.LastOrDefault();

                    if (pinpoint == null) return;

                    mainMap.Center = new Windows.Devices.Geolocation.Geopoint(new Windows.Devices.Geolocation.BasicGeoposition() { Latitude = pinpoint.Latitude, Longitude = pinpoint.Longitude });
                    mainMap.ZoomLevel = 10;
                }

            }
        }
    }
}
