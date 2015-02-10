using Despegar.WP.UI.Common;
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
using Windows.UI.Xaml.Navigation;

namespace Despegar.WP.UI.Product.Hotels
{
    public sealed partial class HotelsFilters : Page
    {
        public HotelsFilters()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.DataContext = e.Parameter;
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Navigator.Instance.GoBack();
        }

        private void AppBarCancelButton_Click(object sender, RoutedEventArgs e)
        {
            Navigator.Instance.GoBack();
        }
    }
}
