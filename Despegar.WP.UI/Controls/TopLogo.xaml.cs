using Despegar.WP.UI.Developer;
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

namespace Despegar.WP.UI.Controls
{
    public sealed partial class TopLogo : UserControl
    {
        private ModalPopup instance = null;

        public TopLogo()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Launch Dev Tools
        /// </summary>
        private void TopLogo_Tapped(object sender, TappedRoutedEventArgs e)
        {
#if DEBUG
            //if (instance == null)    // Recreate it everytime beacuse the data needs to be updated
                instance = new ModalPopup(new DevTools());

            instance.Show();
#endif
        }
    }
}