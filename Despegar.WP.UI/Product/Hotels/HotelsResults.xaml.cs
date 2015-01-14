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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Despegar.WP.UI.Product.Hotels
{

    public sealed partial class HotelsResults : Page
    {
        private NavigationHelper navigationHelper;
        public HotelsResultsViewModel hotelResultsViewModel { get; set; } 

        public HotelsResults()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            //this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            //this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

   
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            CitiesAvailability citiesAvailability = e.Parameter as CitiesAvailability; //TODO: Never is null?
            hotelResultsViewModel = new HotelsResultsViewModel(Navigator.Instance, GlobalConfiguration.CoreContext.GetHotelService(), BugTracker.Instance);
            hotelResultsViewModel.citiesAvailability = citiesAvailability;
            hotelResultsViewModel.init();
            this.DataContext = hotelResultsViewModel;
        }

        private void ReSearchTapped(object sender, TappedRoutedEventArgs e)
        {
            NavigationHelper.GoBack();
        }


    }
}
