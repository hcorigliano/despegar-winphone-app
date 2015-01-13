using Despegar.WP.UI.Developer.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Despegar.WP.UI.Developer
{
    public class MeasureTool
    {

        private static MeasureControl control;

        private static bool _visible;
        public static bool IsVisible
        {
            get
            {
                return _visible;
            }
            set
            {
                _visible = value;
                UpdateTool();
            }
        }

        private static async void UpdateTool()
        {
            if (control == null)
            {
                // Build tool
               
                var frame = Window.Current.Content as Frame;
                var child = VisualTreeHelper.GetChild(frame, 0);
                var childAsBorder = child as Border;

                if (childAsBorder != null)
                {
                    // Not a pretty way to control the root visual, but I did not
                    // want to implement using a popup.
                    var content = childAsBorder.Child;                  
                    childAsBorder.Child = null;

                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        Grid newGrid = new Grid();
                        childAsBorder.Child = newGrid;
                        newGrid.Children.Add(content);

                        PrepareGrid(frame, newGrid);
                    });
                }                

            }

            control.Visibility = _visible ? Visibility.Visible : Visibility.Collapsed;
        }

        private static void PrepareGrid(Frame frame, Grid parent)
        {
            control = new MeasureControl();

            // Places the grid into the visual tree. It is never removed once
            // being added.
            parent.Children.Add(control);
            parent.PointerPressed += Pointer_pressed;
            parent.PointerMoved += Pointer_pressed;
        }

        private static void Pointer_pressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var p = e.GetCurrentPoint(null);
            control.Position = p.Position;
        }

    }
}
