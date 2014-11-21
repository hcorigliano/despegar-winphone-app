using Despegar.WP.UI.Common;
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
        private IPopupContent content;

        public ModalPopup(IPopupContent controlContent)
        {
            popupControl = new Popup();
            popupControl.IsLightDismissEnabled = false;
            popupControl.HorizontalAlignment = HorizontalAlignment.Center;
            popupControl.VerticalAlignment = VerticalAlignment.Center;
            // Set Modal Content
            content = controlContent;
            popupControl.Child = controlContent as UIElement;                
        }

        public void Show() 
        {
            popupControl.IsOpen = true;
            content.Enter();
        }

        public void Hide()
        {
            //popupControl.IsOpen = false;
            content.Leave();
        }

    }
}
