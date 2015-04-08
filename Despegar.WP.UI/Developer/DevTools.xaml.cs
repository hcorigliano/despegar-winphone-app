using Despegar.WP.UI.Common;
using System.Windows;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Controls;
using Despegar.Core.Neo.Log;

namespace Despegar.WP.UI.Developer
{
    public sealed partial class DevTools : UserControl, IPopupContent
    {
        private DeveloperViewModel ViewModel { get; set; }

        public DevTools()
        {
            this.InitializeComponent();
            
            // Load State
            ViewModel = new DeveloperViewModel();
            this.DataContext = ViewModel;

            // Default Picker Values
            this.OpacityPicker.SelectedValue = MetroGridHelper.Opacity;
            this.ColourPicker.SelectedColor = MetroGridHelper.Color;
        }

        public void Enter()
        {
            ShowDialogAnimation.Begin();
        }

        public void Leave()
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Leave();
        }

        private async void TextBlock_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var data = ((FrameworkElement)sender).DataContext as APICall;
            var dialog = new MessageDialog(data.Response, "Server Response");
            await dialog.ShowSafelyAsync();
        }
    }
}