using Despegar.WP.UI.Classes;
using System.Net.NetworkInformation;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

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

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
                e.Handled = true;
                PagesManager.ClearStack();
                PagesManager.GoTo(typeof(Home), null);
                //NavigationHelper.RemoveBackEntry();  // TODO!!!            
        }
    }
}