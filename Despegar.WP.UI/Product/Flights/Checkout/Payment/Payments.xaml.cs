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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Despegar.WP.UI.Product.Flights.Checkout.Payment
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Payments : Page
    {
        public delegate void EventHandler(object sender, RoutedEventArgs e);
        public event EventHandler OnUserControlButtonClicked;

        public Payments()
        {
            this.InitializeComponent();

            PaymentWithInterest.OnUserControlButtonClicked += this.OnButtonClicked;
        
           //radioButtonOnePayWith.Checked += new EventHandler(OnButtonClicked);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        public void PayMethodChanged(object sender, RoutedEventArgs e)
        {
            
        }

        //private void OnButtonClicked(object sender, EventArgs e)
        //{
        //    // Delegate the event to the caller
        //    if (OnUserControlButtonClicked != null)
        //        OnUserControlButtonClicked(this, e);
        //}

        private void OnButtonClicked(object sender, RoutedEventArgs e)
        {
            var a = this.DataContext;
            var b = ((RadioButton)e.OriginalSource).DataContext;

            if (OnUserControlButtonClicked != null)
                OnUserControlButtonClicked(this, e);
        }
    }
}
