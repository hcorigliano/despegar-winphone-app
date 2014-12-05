using Despegar.Core.Business.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Despegar.WP.UI.Product.Flights.Checkout.Passegers.Controls
{
    public sealed partial class NationalitySelectionPopup : UserControl, Despegar.WP.UI.Common.IPopupContent
    {

        public NationalitySelectionPopup()
        {
            this.InitializeComponent();
            NationalityControl.Height = Window.Current.Bounds.Height;
            NationalityControl.Width = Window.Current.Bounds.Width;

            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

        }

        public void Enter()
        {
            //ShowDialogAnimation.Begin();
        }

        public void Leave()
        {
            //HideDialogAnimation.Begin();
            //HideDialogAnimation.Completed += DoClosePopup;
        }

        private void DoClosePopup()
        {
            // in this example we assume the parent of the UserControl is a Popup 
            Popup p = this.Parent as Popup;

            // close the Popup
            if (p != null) { p.IsOpen = false; }
        }

        private void SelectionChangedListBox(object sender, SelectionChangedEventArgs e)
        {
            CountryFields countrySelected = (CountryFields)(((ListView)NationalityControl).SelectedItem);
            ((NationalitySelection)DataContext).NationalityText = countrySelected.name;
            //Leave();
            DoClosePopup();
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null)
            {
                e.Handled = true;
                DoClosePopup();
            }
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

    }
}
