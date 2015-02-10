using Despegar.Core.Neo.InversionOfControl;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Model.Controls;
using Despegar.WP.UI.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace Despegar.WP.UI.Controls.PhotoGallery
{
    public sealed partial class PhotoGalleryControl : UserControl
    {
        //TODO create property to set the size of picture
        static string URLCONTENT = "http://staticontent.com/media/pictures/{0}/118x118";
        public PhotoGalleryViewModel photoGalleryViewModel;

        public PhotoGalleryControl()
        {    
            this.InitializeComponent();
            photoGalleryViewModel = IoC.Resolve<PhotoGalleryViewModel>();
        }

        private void VariableSizedWrapGrid_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (args.NewValue != null)
            {
                List<string> keyList = new List<string>();

                if (args.NewValue as List<string> != null)
                {
                    keyList.AddRange(args.NewValue as List<string>);

                    foreach (string key in keyList)
                    {

                        string urlimage = String.Format(URLCONTENT, key);

                        Uri imageURI = new Uri(urlimage, UriKind.Absolute);
                        BitmapImage bmi = new BitmapImage();
                        bmi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                        bmi.UriSource = imageURI;

                        Grid gridvariable = sender as Grid;
                        var imagesItems = gridvariable.Children.Where(r => r.GetType() == typeof(Image));

                        foreach (Image item in imagesItems)
                        {
                            if (item.Source == null)
                            {
                                item.Source = bmi;
                                item.Tag = key;
                                break;
                            }
                        }
                    }

                   // if (photoGalleryViewModel.PictureListName == null)
                   // {
                    photoGalleryViewModel.PictureListName = new List<string>();
                   // }
                    photoGalleryViewModel.PictureListName.AddRange(keyList);
                }
            }
        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Image image = sender as Image;

            photoGalleryViewModel.SelectedPicture = image.Tag as string;
             
            //var f = Window.Current.Content as Frame;


            Navigator.Instance.GoTo(ViewModelPages.PhotoPresenter, photoGalleryViewModel);
            //f.Navigate(typeof(PhotoPresenter), photoGalleryViewModel);
        }
    }
}