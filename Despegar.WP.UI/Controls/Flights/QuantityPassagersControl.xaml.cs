using Despegar.WP.UI.Model.Classes.Flights;
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

namespace Despegar.WP.UI.Controls.Flights
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class QuantityPassagersControl : Page
    {
        public TextBlock QuantityAdultsControl { get; set; }
        public TextBlock QuantityChildControl { get; set; }
        public PassagersQuantity Passagers = new PassagersQuantity();

        public QuantityPassagersControl()
        {
            this.InitializeComponent();

            QuantityAdultsControl = txbAdults;
            QuantityChildControl = txbChild;

            Passagers.ChildPassagerQuantity = 0;
            Passagers.AdultPassagerQuantity = 1;

            this.txbAdults.DataContext = Passagers;
            this.txbChild.DataContext = Passagers;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        //Bottons for passagers Quantity
        #region
        private void btnAdultAdd_Click(object sender, RoutedEventArgs e)
        {
            Passagers.AddAdult();
        }

        private void btnChildAdd_Click(object sender, RoutedEventArgs e)
        {
            Passagers.AddChild();
        }

        private void btnAdultSub_Click(object sender, RoutedEventArgs e)
        {
            Passagers.SubAdult();
        }

        private void btnChildSub_Click(object sender, RoutedEventArgs e)
        {
            Passagers.subChild();
        }

        #endregion
    }
}
