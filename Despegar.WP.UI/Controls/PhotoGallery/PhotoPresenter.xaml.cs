using Despegar.WP.UI.BugSense;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Model.Interfaces;
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


namespace Despegar.WP.UI.Controls.PhotoGallery
{
    public sealed partial class PhotoPresenter : Page
    {
        private PhotoGalleryViewModel ViewModel;

        public PhotoPresenter()
        {
            this.InitializeComponent();

            ViewModel = new PhotoGalleryViewModel(Navigator.Instance, SplunkMintBugTracker.Instance);
            this.DataContext = ViewModel;

            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.DataContext = e.Parameter as PhotoGalleryViewModel;
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            ViewModel.GoBack();
        }

    }
}
