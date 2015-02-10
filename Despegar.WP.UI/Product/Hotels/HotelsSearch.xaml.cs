﻿using Despegar.Core.Neo.Business.CustomErrors;
using Despegar.Core.Neo.InversionOfControl;
using Despegar.WP.UI.Controls;
using Despegar.WP.UI.Model.Common;
using Despegar.WP.UI.Model.ViewModel;
using Despegar.WP.UI.Model.ViewModel.Hotels;
using System.ComponentModel;
using Windows.ApplicationModel.Resources;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;


namespace Despegar.WP.UI.Product.Hotels
{
    public sealed partial class HotelsSearch : Page
    {
        public HotelsSearchViewModel ViewModel { get; set; }
        private ModalPopup loadingPopup = new ModalPopup(new Loading());

        public HotelsSearch()
        {
            this.InitializeComponent();
        }

        # region ** ERROR HANDLING **
        private async void ErrorHandler(object sender, ViewModelErrorArgs e)
        {
            ViewModel.BugTracker.LeaveBreadcrumb("Flight search Error Raised: " + e.ErrorCode);

            ResourceLoader manager = new ResourceLoader();
            MessageDialog dialog;

            switch (e.ErrorCode)
            {
                case "SEARCH_INVALID_WITH_MESSAGE":
                    CustomError message = e.Parameter as CustomError;
                    if (message == null) break;

                    string msg = manager.GetString(message.Code);

                    if (message.HasDates)
                    {
                        string msgunformated = msg;
                        msg = string.Format(msgunformated, message.Date);
                    }

                    dialog = new MessageDialog(msg, manager.GetString("Hotels_Search_ERROR_SEARCH_INVALID_TITLE"));
                    await dialog.ShowSafelyAsync();
                    break;
            }
        }
        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            ViewModel = IoC.Resolve<HotelsSearchViewModel>();
            ViewModel.PropertyChanged += Checkloading;
            ViewModel.ViewModelError += ErrorHandler;
            ViewModel.OnNavigated(null);           
            this.DataContext = ViewModel;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            ViewModel.BugTracker.LeaveBreadcrumb("Flight Search View - Back button pressed");

            if (ViewModel != null)
            {
                if (ViewModel.IsLoading)
                {
                    e.Handled = true;
                }
                else
                {
                    ViewModel.Navigator.GoBack();
                    e.Handled = true;
                }
            }
        }

        private void Checkloading(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsLoading")
            {
                if ((sender as ViewModelBase).IsLoading)
                    loadingPopup.Show();
                else
                    loadingPopup.Hide();
            }
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.GetPositionCommand.Execute(null);
        }
    }
}