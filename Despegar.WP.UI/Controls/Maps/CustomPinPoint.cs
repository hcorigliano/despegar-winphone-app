using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;

namespace Despegar.WP.UI.Controls.Maps
{
    public class CustomPinPoint
    {
        public string Title { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public Point point { get; set; }
        public Geopoint Geopoint 
            {
                get { 
                    var location = new Windows.Devices.Geolocation.Geopoint(new Windows.Devices.Geolocation.BasicGeoposition() { Latitude = Latitude , Longitude = Longitude });
                    return location;
                    }
            }
        public Point Anchor { get { return new Point(0.5, 1); } }

        public CustomPinPoint()
        {
            
        }
    }
}
