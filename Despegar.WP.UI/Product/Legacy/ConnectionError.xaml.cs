using Windows.Phone.UI.Input;
using Windows.UI.Xaml.Controls;

namespace Despegar.WP.UI.Product.Legacy
{
    public sealed partial class ConnectionError : Page
    {
        public ConnectionError()
        {
            this.InitializeComponent();

            #if DECOLAR
            MainLogo.Source = new BitmapImage(new Uri("/Assets/Image/decolar-logo.png", UriKind.RelativeOrAbsolute));
            #endif

        }

        //protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        //{
        //    if (!NetworkInterface.GetIsNetworkAvailable()) {
        //        Application.Current.Exit();
        //    }
        //    else
        //    {
        //        PagesManager.GoTo(typeof(Home), null);
        //        //NavigationHelper.RemoveBackEntry();
        //    }
        //}
    }
}