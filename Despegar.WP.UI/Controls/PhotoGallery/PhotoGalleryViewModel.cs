using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Despegar.WP.UI.Controls.PhotoGallery
{
    public class PhotoGalleryViewModel
    {
        public List<string> PictureListName { get; set; }
        private List<BitmapImage> _ImageList { get; set; }
        static string URLCONTENT = "http://staticontent.com/media/pictures/{0}/118x118";
        public ICollection<BitmapImage> ImageList
        {
            get
            {

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

        public PhotoGalleryViewModel()
        {

        }


    }
}
