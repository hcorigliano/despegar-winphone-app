using Despegar.Core.Business;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

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
        }

        private void ClosePopup(object sender, RoutedEventArgs e)
        {
            // in this example we assume the parent of the UserControl is a Popup 
            Popup p = this.Parent as Popup;
            
            // close the Popup
            if (p != null) { p.IsOpen = false; }            
        }        
    }
}