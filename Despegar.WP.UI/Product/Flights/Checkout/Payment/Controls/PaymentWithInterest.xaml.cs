using Despegar.Core.Business.Flight.BookingFields;
using Despegar.WP.UI.Model.Classes.Flights.Checkout;
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

namespace Despegar.WP.UI.Product.Flights.Checkout.Payment.Controls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PaymentWithInterest : Page
    {
        public delegate void EventHandler(object sender, RoutedEventArgs e);
        public event EventHandler OnUserControlButtonClicked;

        public PaymentWithInterest()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void RadioButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void Image_Load_Failed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void OnButtonClicked(object sender, RoutedEventArgs e)
        {
            //PaymentDetail bla = (PaymentDetail)(((CheckBox)e.OriginalSource).DataContext);
            //var ble = ((RadioButton)e.OriginalSource).DataContext;
            //((RadioButton)e.OriginalSource).DataContext = PaymentsFormated.CardToArray((PaymentDetail)(((RadioButton)e.OriginalSource).DataContext)); 
            if (OnUserControlButtonClicked != null)
                OnUserControlButtonClicked(this, e);
        }

    }
}
