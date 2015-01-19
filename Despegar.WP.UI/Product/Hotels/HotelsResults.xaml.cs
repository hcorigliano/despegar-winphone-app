using Despegar.Core.Business.Hotels.CitiesAvailability;
using Despegar.WP.UI.BugSense;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Model;
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

namespace Despegar.WP.UI.Product.Hotels
{

    public sealed partial class HotelsResults : Page
    {
        public HotelsResultsViewModel ViewModel { get; set; } 

        public HotelsResults()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            CitiesAvailability citiesAvailability = e.Parameter as CitiesAvailability; //TODO: Never is null?
            ViewModel = new HotelsResultsViewModel(Navigator.Instance, GlobalConfiguration.CoreContext.GetHotelService(), BugTracker.Instance);
            ViewModel.CitiesAvailability = citiesAvailability;
            this.DataContext = ViewModel;
        }

        private void ReSearchTapped(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.Navigator.GoBack();
        }

    }
}
