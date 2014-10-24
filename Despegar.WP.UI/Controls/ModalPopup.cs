using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace Despegar.WP.UI.Controls
{
    public class ModalPopup
    {
        private Popup popupControl;

        public ModalPopup(UIElement content)
        {
            popupControl = new Popup();
            popupControl.IsLightDismissEnabled = false;
            popupControl.HorizontalAlignment = HorizontalAlignment.Center;
            popupControl.VerticalAlignment = VerticalAlignment.Center;
            // Set Modal Content
            popupControl.Child = content;                
        }

        public void Show() 
        {
            popupControl.IsOpen = true;  
        }

        public void Hide()
        {
            popupControl.IsOpen = false;            
        }

    }
}
