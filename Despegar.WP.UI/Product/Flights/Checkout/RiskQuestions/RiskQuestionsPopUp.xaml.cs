using Despegar.WP.UI.Common;
using Despegar.WP.UI.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace Despegar.WP.UI.Product.Flights.Checkout.RiskQuestions
{
    public sealed partial class RiskQuestionsPopUp : UserControl, IPopupContent
    {

        public RiskQuestionsPopUp()
        {
            this.InitializeComponent();
            mainCanvas.Height = Window.Current.Bounds.Height;
            mainCanvas.Width = Window.Current.Bounds.Width;
        }

        public void Enter()
        {
            var test = this.DataContext;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            ShowDialogAnimation.Begin();
        }

        public void Leave()
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
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

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            Leave();
        }
    }
}
