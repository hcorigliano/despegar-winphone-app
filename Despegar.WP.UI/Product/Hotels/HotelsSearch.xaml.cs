﻿using System;
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
using Despegar.WP.UI.Model.ViewModel.Hotels;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Model;
using Despegar.WP.UI.BugSense;
using Windows.Phone.UI.Input;
using System.ComponentModel;
using Despegar.WP.UI.Model.ViewModel;
using Despegar.WP.UI.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Despegar.WP.UI.Product.Hotels
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HotelsSearch : Page
    {

        public HotelsSearchViewModel ViewModel { get; set; }
        private ModalPopup loadingPopup = new ModalPopup(new Loading());


        public HotelsSearch()
        {
            this.InitializeComponent();
            ViewModel = new HotelsSearchViewModel(Navigator.Instance, GlobalConfiguration.CoreContext.GetHotelService(), BugTracker.Instance);
            ViewModel.PropertyChanged += Checkloading;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            this.DataContext = ViewModel;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            BugTracker.Instance.LeaveBreadcrumb("Flight Search View - Back button pressed");

            if (ViewModel != null)
            {
                if (ViewModel.IsLoading)
                {
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


    }
}
