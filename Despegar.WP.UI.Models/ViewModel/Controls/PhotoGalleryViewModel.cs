using Despegar.Core.Neo.Business;
using Despegar.Core.Neo.Contract.Log;
using Despegar.WP.UI.Model.Interfaces;
using Despegar.WP.UI.Model.ViewModel;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Media.Imaging;

namespace Despegar.WP.UI.Model.Controls
{
    public class PhotoGalleryViewModel : ViewModelBase
    {
        public List<string> PictureListName { get; set; }
        private List<BitmapImage> _ImageList { get; set; }
        static string URLCONTENT = "http://staticontent.com/media/pictures/{0}/300x300";       

        public ICollection<BitmapImage> ImageList
        {
            get
            {
                if (PictureListName == null) return null;

                if (this._ImageList == null)
                {
                    this._ImageList = new List<BitmapImage>();
                    foreach (string key in PictureListName)
                    {
                        string urlimage = String.Format(URLCONTENT, key);

                        Uri imageURI = new Uri(urlimage, UriKind.Absolute);
                        BitmapImage bmi = new BitmapImage();
                        bmi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                        bmi.UriSource = imageURI;
                        _ImageList.Add(bmi);
                    }
                }

                return _ImageList;
            }
        }
        public string SelectedPicture { get; set; }

        public PhotoGalleryViewModel(INavigator nav, IBugTracker t) : base(nav, t) { }

        public override void OnNavigated(object navigationParams)
        {
        }
        
    }
}