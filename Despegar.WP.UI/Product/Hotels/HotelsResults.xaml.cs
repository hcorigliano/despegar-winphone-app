using Despegar.Core.Neo.Business;
using Despegar.Core.Neo.Business.Hotels.CitiesAvailability;
using Despegar.Core.Neo.InversionOfControl;
using Despegar.WP.UI.BugSense;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Controls;
using Despegar.WP.UI.Model;
using Despegar.WP.UI.Model.ViewModel;
using Despegar.WP.UI.Model.ViewModel.Hotels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
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
        private ModalPopup loadingPopup = new ModalPopup(new Loading());
        private HotelsResultsViewModel ViewModel { get; set; }

        public HotelsResults()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            if (e.NavigationMode == NavigationMode.New)
            {
                
                    ViewModel = IoC.Resolve<HotelsResultsViewModel>();
                    ViewModel.PropertyChanged += Checkloading;
                    ViewModel.OnNavigated(e.Parameter);
                    await ViewModel.Search();
                    this.DataContext = ViewModel;                   
                
                //if (ViewModel.CitiesAvailability.SearchStatus == SearchStates.SearchAgain)
                //    await ViewModel.SearchAgaing();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            ViewModel.Navigator.GoBack();
        }

        private void ReSearchTapped(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.Navigator.GoBack();
        }

        private void FilterOrSortClick(object sender, RoutedEventArgs e)
        {
            //ViewModel.CitiesAvailability.SearchStatus = SearchStates.SearchAgain;
        }

        private void Checkloading(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsLoading")
            {
                if ((sender as ViewModelBase).IsLoading)
                    loadingPopup.Show();
                else
                    loadingPopup.Hide();
            }
        }

        private void HotelSelected(object sender, TappedRoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                ViewModel.CrossParameters.IdSelectedHotel = ((HotelItem)((ListView)sender).SelectedItem).id.ToString();
                ViewModel.CrossParameters.HotelsExtraData.Distance = ((HotelItem)((ListView)sender).SelectedItem).distance;
                ViewModel.GoToDetails();
            }
        }

    }
}
