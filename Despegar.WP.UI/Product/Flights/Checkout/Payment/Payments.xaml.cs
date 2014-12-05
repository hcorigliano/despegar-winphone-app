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


namespace Despegar.WP.UI.Product.Flights.Checkout.Payment
{
    public sealed partial class Payments : Page
    {
        public delegate void EventHandler(object sender, RoutedEventArgs e);
        public event EventHandler OnUserControlButtonClicked;

        public Payments()
        {
            this.InitializeComponent();
            this.Loaded += SelectFirstRadioButton;
            PaymentWithInterest.OnUserControlButtonClicked += this.OnButtonClicked;
        
        }

        private void SelectFirstRadioButton(object sender, RoutedEventArgs e)
        {
            RadioButton rbToSelect = (RadioButton)(payments.Children.First(x => x.GetType().Name == "RadioButton"));
            rbToSelect.IsChecked = true;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        public void PayMethodChanged(object sender, RoutedEventArgs e)
        {
            
        }

        private void OnButtonClicked(object sender, RoutedEventArgs e)
        {
            if (OnUserControlButtonClicked != null)
                OnUserControlButtonClicked(this, e);
        }
    }
}
