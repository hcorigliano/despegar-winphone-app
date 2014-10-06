using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Despegar.WP.UI.Classes
{
    static public class PagesManager
    {
        static public void GoTo(Type page, ItemClickEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame != null)
            {
                rootFrame.Navigate(page, e);
            }
        }
    }
}
