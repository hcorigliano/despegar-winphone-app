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



// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Despegar.WP.UI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Loading : Page
    {
        public Loading()
        {
            this.InitializeComponent();

            var animation = new ObjectAnimationUsingKeyFrames();

            // Create the image element.

            for (int i = 0; i <= 15; i++)//16 is the number of images that are going to be displayed
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new Uri(string.Format("ms-appx:///Assets/Images/win_" + i.ToString() + ".png"));
                DiscreteObjectKeyFrame keyframe = new DiscreteObjectKeyFrame()
                {
                   KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(75 * i)),//Time Interval
                   Value = bitmapImage
                };

                animation.KeyFrames.Add(keyframe);
            } 

            Storyboard.SetTarget(animation, ImageView);
            Storyboard.SetTargetProperty(animation, "Source");

            storyboard.Children.Add(animation);

            storyboard.Begin();
        }

        public void ShowAnimation()
        {



        }
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}



