using Despegar.Core.Log;
using Despegar.WP.UI.Controls.Flights;
using Despegar.WP.UI.Model.ViewModel.Flights;
using Despegar.WP.UI.Product.Flights;
using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Despegar.WP.UI.Common;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using Despegar.Core.Business.Flight.CitiesAutocomplete;
using Windows.UI.Popups;
using System;

namespace Despegar.WP.UI.Developer
{
    public sealed partial class DevTools : UserControl
    {
        private DeveloperViewModel ViewModel { get; set; }

        public DevTools()
        {
            this.InitializeComponent();
            this.DataContext = Window.Current.Bounds;

            // Load State
            ViewModel = new DeveloperViewModel();
            this.DataContext = ViewModel;

            // Default Picker Values
            this.OpacityPicker.SelectedValue = MetroGridHelper.Opacity;
            this.ColourPicker.SelectedColor = MetroGridHelper.Color;

            ShowDialogAnimation.Begin();
        }

        private void ClosePopup(object sender, RoutedEventArgs e)
        {
            HideDialogAnimation.Begin();
            HideDialogAnimation.Completed += DoClosePopup;                 
        }

        private void DoClosePopup(object sender, object e)
        {
            // in this example we assume the parent of the UserControl is a Popup 
            Popup p = this.Parent as Popup;

            // close the Popup
            if (p != null) { p.IsOpen = false; }     
        }

               
    }
}