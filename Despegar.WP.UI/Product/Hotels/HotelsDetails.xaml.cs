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
using Despegar.Core.Neo.Business.Hotels.CitiesAvailability;
using Windows.Phone.UI.Input;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Model;
using Despegar.WP.UI.BugSense;
using Despegar.WP.UI.Model.ViewModel.Hotels;
using Despegar.WP.UI.Model.ViewModel;
using Despegar.WP.UI.Controls;
using Despegar.Core.Neo.InversionOfControl;
using Despegar.WP.UI.Product.Hotels.Details.Controls;


namespace Despegar.WP.UI.Product.Hotels
{
    public sealed partial class HotelsDetails : Page
    {
        private HotelsDetailsViewModel ViewModel { get; set; }
        private ModalPopup loadingPopup = new ModalPopup(new Loading());

        public HotelsDetails()
        {
            this.InitializeComponent();
           
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            if(e.NavigationMode == NavigationMode.New || e.NavigationMode == NavigationMode.Back)
            {
                ViewModel = IoC.Resolve<HotelsDetailsViewModel>();
                ViewModel.PropertyChanged += Property_Changed;
                ViewModel.OnNavigated(e.Parameter);
                await ViewModel.Init();
                this.DataContext = ViewModel;
            }

            if(ViewModel.RoomsQuantity > 1)
            {
                MainPivot.Items.Remove(RoomSelectionPivot);
            }

        }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            //Stop all of your timers 

            base.OnNavigatingFrom(e);

        }
            
        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            ViewModel.Navigator.GoBack();
        }

        //private void GoToDetailsPivot(object sender, RoutedEventArgs e)
        //{
        //    //Not implemented (Yet)
        //    throw new NotImplementedException();
        //}

        //private void GoToCommentsPivot(object sender, RoutedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

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

    }
}