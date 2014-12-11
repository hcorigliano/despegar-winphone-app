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
using Windows.Phone.UI.Input;


namespace Despegar.WP.UI.Controls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Loading : UserControl, IPopupContent
    {
        private int NumberOfFrames = 73;
        private int FrameSize = 120;

        public Loading()
        {
            this.InitializeComponent();

            
            this.Loaded += Loading_Loaded;

            this.DataContext = Window.Current.Bounds;

            // Create Loading Animation
            var animation = new ObjectAnimationUsingKeyFrames();

            for (int i = 0; i < NumberOfFrames; i++)
            {
                TranslateTransform transform = new TranslateTransform();
                transform.X = -i * FrameSize;
                transform.Y = 0;

                DiscreteObjectKeyFrame keyframe = new DiscreteObjectKeyFrame()
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(42 * i)), //Time Interval
                    Value = transform
                };

                animation.KeyFrames.Add(keyframe);
            }

            Storyboard.SetTarget(animation, ImageBrush);
            Storyboard.SetTargetProperty(animation, "Transform");

            loadingStoryboard.Children.Add(animation);
            loadingStoryboard.Begin();
        }

        private void Loading_Loaded(object sender, RoutedEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
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

            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;

        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
                e.Handled = true;
        }
    }
}