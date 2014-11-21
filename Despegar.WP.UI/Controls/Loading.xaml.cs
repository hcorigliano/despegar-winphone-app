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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Despegar.WP.UI.Common;


namespace Despegar.WP.UI.Controls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Loading : UserControl, IPopupContent
    {
     
        public Loading()
        {
            this.InitializeComponent();

            var animation = new ObjectAnimationUsingKeyFrames();

            // Create the image element.
            for (int i = 0; i <= 72; i++) //16 is the number of images that are going to be displayed
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new Uri(String.Format("ms-appx:///Assets/Animations/loading/loading{0}.png", i));
                DiscreteObjectKeyFrame keyframe = new DiscreteObjectKeyFrame()
                {
                   KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(42 * i)), //Time Interval
                   Value = bitmapImage
                };

                animation.KeyFrames.Add(keyframe);
            } 

            Storyboard.SetTarget(animation, ImageView);
            Storyboard.SetTargetProperty(animation, "Source");

            loadingStoryboard.Children.Add(animation);

            //(this.Content as FrameworkElement).DataContext = this;
            loadingStoryboard.Begin();

            this.DataContext = Window.Current.Bounds;
        }

        public void ShowLoading()
        {
            ShowDialogAnimation.Begin();
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
    }
}