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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Despegar.WP.UI.Product.Flights.Checkout.Payment.Controls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Cards : UserControl
    {
        public Cards()
        {
            this.InitializeComponent();
        }

        private void Image_Load_Failed(object sender, ExceptionRoutedEventArgs e)
        {
            //var test = ((Image)sender).Source;
            //int i = 1;
            //((Image)sender).Source = new ImageSource
            ((Image)sender).Source = new BitmapImage(new Uri("ms-appx:/Assets/Icon/CreditCard/GRL.png", UriKind.Absolute));//BitmapImage(new Uri(@"/Assets/Icon/CreditCard/GRL.png", UriKind.RelativeOrAbsolute));
            //File.Exists("path to file")
            //int i = 1;
            //((Image)sender).SetValue( //Source = new ImageSource()
        }
    }
}
