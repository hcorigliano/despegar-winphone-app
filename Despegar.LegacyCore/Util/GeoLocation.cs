/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace Despegar.LegacyCore.Util
{
    public class GeoLocation
    {
        private static GeoLocation instance;
        private Geolocator geoLocator;
        private GeoPosition geoLocation;
        private bool available = false;

        private GeoLocation()
        {
            this.Init();
            this.geoLocator = new Geolocator();
        }

        public static GeoLocation Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GeoLocation();
                }
                return instance;
            }
        }

        public void Init()
        {
            TimeSpan timeout = new TimeSpan(0, 0, 5);
            if (geoLocator.Permission == GeoPositionPermission.Granted)
            {
                if (geoLocator.TryStart(true, timeout))
                {
                    available = true;
                    geoLocation = geoLocator.Position;
                }
            }
        }

        public string getLatitude()
        {
            if (this.isAvailable())
                return geoLocation.Location.Latitude.ToString().Replace(",", ".");
            return "";
        }

        public string getLongitude()
        {
            if (this.isAvailable())
                return geoLocation.Location.Longitude.ToString().Replace(",", ".");
            return "";
        }

        public bool isAvailable()
        {
            return available;
        }
    }
}
*/