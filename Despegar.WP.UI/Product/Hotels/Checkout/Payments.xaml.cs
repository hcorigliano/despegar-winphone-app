using Despegar.Core.Business.Hotels;
using Despegar.WP.UI.Model.ViewModel.Hotels;
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

namespace Despegar.WP.UI.Product.Hotels.Checkout
{
    public sealed partial class Payments : UserControl
    {
        private HotelsCheckoutViewModel ViewModel { get { return DataContext as HotelsCheckoutViewModel; } }

        public Payments()
        {
            this.InitializeComponent();
        }

        private void OnRadioButton_Clicked(object sender, RoutedEventArgs e)
        {
            RadioButton a = (RadioButton)e.OriginalSource;
            ViewModel.SelectedInstallment = a.DataContext as InstallmentOption;
        }
    }
}