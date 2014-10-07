using System.ComponentModel;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml.Controls;

namespace Despegar.WP.UI.Product.Legacy
{
    public class LegacyBasePage : Page
    {
        
        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            var args = new CancelEventArgs();
            OnBackKeyPress(args);
            if (args.Cancel)
            {
                e.Handled = true;
            }
        }

        protected virtual void OnBackKeyPress(CancelEventArgs e)
        {
        }

    }
}
