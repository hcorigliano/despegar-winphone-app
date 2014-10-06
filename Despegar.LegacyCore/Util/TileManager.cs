/*using System;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows;
using Despegar.LegacyCore.Resource;
using Despegar.LegacyCore.Connector.Domain.API;

namespace Despegar.LegacyCore.Util
{
    public static class TileManager
    {
        private const string ImageServiceBaseUri = "http://{0}.staticontent.com/geo-pictures/640x480/{1}-01.jpg";

        public static void SetIATATile()
        {
            if (!string.IsNullOrEmpty(ApplicationConfig.Instance.IATA))
            {
                var serviceUri = new Uri(string.Format(ImageServiceBaseUri, ApplicationConfig.Instance.Country.ToLower(), ApplicationConfig.Instance.IATA));

                var tileData = new FlipTileData
                {
                    Title = Properties.CommonBrandName,
                    BackTitle = string.Empty,
                    BackContent = "",
                    WideBackContent = "",
                    SmallBackgroundImage = new Uri("/Assets/Image/ApplicationIcon.png", UriKind.Relative),
                    BackgroundImage = new Uri("/Assets/Image/ApplicationNormalIcon.png", UriKind.Relative),
                    BackBackgroundImage = serviceUri,
                    WideBackgroundImage = new Uri("/Assets/Image/ApplicationWideIcon.png", UriKind.Relative),
                    WideBackBackgroundImage = serviceUri,
                };

                Deployment.Current.Dispatcher.BeginInvoke(() => ShellTile.ActiveTiles.First().Update(tileData));
            }
        }

        public static void SetPromoTile(DiscountCountry c)
        {
            FlipTileData tileData;
            if (DateTime.UtcNow > DateTime.Parse(c.discount.hotels.endDate))
            {
                //fin de promo
                tileData = new FlipTileData
                {
                    Title = Properties.CommonBrandName,
                    BackTitle = "",
                    BackContent = Properties.CommonBrandName,
                    WideBackContent = Properties.CommonBrandName,
                    SmallBackgroundImage = new Uri("/Assets/Image/ApplicationIcon.png", UriKind.Relative),
                    BackgroundImage = new Uri("/Assets/Image/ApplicationNormalIcon.png", UriKind.Relative),
                    BackBackgroundImage = null,
                    WideBackgroundImage = new Uri("/Assets/Image/ApplicationWideIcon.png", UriKind.Relative),
                    WideBackBackgroundImage = null,
                };

                TileManager.SetIATATile();
                return;
            }

            else
            {
                var banner = c.discount.hotels.banner.homeBanner;

                tileData = new FlipTileData
                {
                    Title = Properties.CommonBrandName,
                    BackTitle = Properties.CommonBrandName,
                    BackContent = banner.leftText,
                    WideBackContent = banner.leftText,
                    SmallBackgroundImage = new Uri("/Assets/Image/ApplicationIcon.png", UriKind.Relative),
                    BackgroundImage = new Uri("/Assets/Image/ApplicationNormalIcon.png", UriKind.Relative),
                    BackBackgroundImage = null,
                    WideBackgroundImage = new Uri("/Assets/Image/ApplicationWideIcon.png", UriKind.Relative),
                    WideBackBackgroundImage = null,
                };
                
            }
            
            Deployment.Current.Dispatcher.BeginInvoke(() => ShellTile.ActiveTiles.First().Update(tileData));
        }
    }
}
*/