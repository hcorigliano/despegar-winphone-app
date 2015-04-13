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
using Despegar.WP.UI.Product.Hotels.Details;
using Despegar.WP.UI.Model.Common;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;


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

        # region ** ERROR HANDLING **
        private async void ErrorHandler(object sender, ViewModelErrorArgs e)
        {
            ViewModel.BugTracker.LeaveBreadcrumb("Hotels details Error raised - " + e.ErrorCode);

            ResourceLoader manager = new ResourceLoader();
            MessageDialog dialog;

            switch (e.ErrorCode)
            {
                case "SEARCH_ERROR":
                    int errorID = (int)e.Parameter;
                    switch (errorID)
                    {
                        case 2380:
                            dialog = new MessageDialog(manager.GetString("Hotels_Search_ERROR_CHECKIN_INVALID"), "Error");
                            break;
                        case 2399:
                            dialog = new MessageDialog(manager.GetString("Hotels_Search_ERROR_MAX_DAYS_LIMIT"), "Error");
                            break;
                        default:
                            dialog = new MessageDialog(manager.GetString("Hotels_Search_ERROR"), "Error");
                            break;
                    }

                    await dialog.ShowSafelyAsync();
                    break;
                case "NO_AVAILABILITY":
                    dialog = new MessageDialog(manager.GetString("Hotels_Search_ERROR_NO_AVAILABILITY"), "Error");
                    await dialog.ShowSafelyAsync();
                    break;             
                case "UNKNOWN_ERROR":
                    dialog = new MessageDialog(manager.GetString("Hotels_Search_UNKNOWN_ERROR"), "Error");
                    await dialog.ShowSafelyAsync();
                    break;
            }
            
            // Back to search box
            ViewModel.Navigator.GoBack();
        }
        #endregion

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {

            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            if(e.NavigationMode == NavigationMode.New || e.NavigationMode == NavigationMode.Back)
            {
                ViewModel = IoC.Resolve<HotelsDetailsViewModel>();
                ViewModel.PropertyChanged += Property_Changed;
                ViewModel.ViewModelError += ErrorHandler;
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

            if (e.NavigationMode == NavigationMode.Back)
            {
                ResetPageCache();
            }
        }
            
        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            if (!ViewModel.IsLoading)
            {
                ViewModel.Navigator.GoBack();
            }
        }

        private void ResetPageCache()
        {
            this.NavigationCacheMode = NavigationCacheMode.Disabled;

            //Works?

            //if (Parent != null)
            //{
            //    var cacheSize = ((Frame)Parent).CacheSize;
            //    ((Frame)Parent).CacheSize = 0;
            //    ((Frame)Parent).CacheSize = cacheSize;
            //}
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
                MainPivot.SelectedIndex = GetSectionIndex(ViewModel.GoToPivot);
        }

        private int GetSectionIndex(string sectionID)
        {
            PivotItem errorPivot = this.FindName("Pivot_" + sectionID) as PivotItem;
            if (errorPivot == null)
                return MainPivot.SelectedIndex;
            return MainPivot.Items.IndexOf(errorPivot);
        }

    }
}