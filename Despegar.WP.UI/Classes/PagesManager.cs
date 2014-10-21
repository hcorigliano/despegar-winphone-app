using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Despegar.WP.UI.Classes
{
    public static class PagesManager
    {
        public static void GoTo(Type page, object e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame != null)
            {
                rootFrame.Navigate(page, e);
            }
        }

        public static void ClearStack()
        {
            ((Frame)Window.Current.Content).BackStack.Clear();
        }

        public static void GoBack()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null && rootFrame.CanGoBack) rootFrame.GoBack();
        }

        public static void ClearPageCache()
        {
            Frame rootFrame = Window.Current.Content as Frame;

            var cacheSize = rootFrame.CacheSize;
            rootFrame.CacheSize = 0;
            rootFrame.CacheSize = cacheSize;
        }
    }
}