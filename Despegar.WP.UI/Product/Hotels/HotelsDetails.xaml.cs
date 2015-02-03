using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Despegar.Core.Business.Hotels.CitiesAvailability;
using Windows.Phone.UI.Input;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Model;
using Despegar.WP.UI.BugSense;
using Despegar.WP.UI.Model.ViewModel.Hotels;
using Despegar.WP.UI.Model.ViewModel;
using Despegar.WP.UI.Controls;


namespace Despegar.WP.UI.Product.Hotels
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HotelsDetails : Page
    {
        public HotelsDetailsViewModel ViewModel { get; set; }
        private ModalPopup loadingPopup = new ModalPopup(new Loading());

        public HotelsDetails()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            if(ViewModel == null)
            {
                ViewModel = new HotelsDetailsViewModel(Navigator.Instance, GlobalConfiguration.CoreContext.GetHotelService(), BugTracker.Instance) { CrossParameters = e.Parameter as HotelsCrossParameters };
                await ViewModel.Init();
                this.DataContext = ViewModel;
            }

            ViewModel.PropertyChanged += Property_Changed;
        }

        private void Property_Changed(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsLoading")
            {
                if ((sender as ViewModelBase).IsLoading)
                    loadingPopup.Show();
                else
                    loadingPopup.Hide();
            }
            if (e.PropertyName == "GoToPivot")
                MainPivot.SelectedIndex = ViewModel.GoToPivot;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }
            
        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            Navigator.Instance.GoBack();
        }

        private void GoToDetailsPivot(object sender, RoutedEventArgs e)
        {
            //Not implemented (Yet)
        }

        private void GoToCommentsPivot(object sender, RoutedEventArgs e)
        {

        }

       
    }
}
