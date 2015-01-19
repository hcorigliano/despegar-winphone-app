// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using Despegar.WP.UI.Developer.Controls;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace System.Windows
{
    /// <summary>
    /// A utility class that overlays a designer-friendly grid on top of the
    /// application frame, for use similar to the performance counters in
    /// App.xaml.cs. The color and opacity are configurable. The grid contains
    /// a number of squares that are 24x24, offset with 12px gutters, and all
    /// 24px away from the edge of the device.
    /// </summary>
    public static class MetroGridHelper
    {
        private static bool _visible;
        private static double _opacity = 0.2;
        private static Color _color = Colors.Red;
        private static List<DesingUnit> _squares;
        private static Grid _grid;

        /// <summary>
        /// Gets or sets a value indicating whether the designer grid is
        /// visible on top of the application's frame.
        /// </summary>
        public static bool IsVisible
        {
            get
            {
                return _visible;
            }
            set
            {
                _visible = value;
                UpdateGrid();
            }
        }

        /// <summary>
        /// Gets or sets the color to use for the grid's squares.
        /// </summary>
        public static Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                UpdateGrid();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the opacity for the grid's squares.
        /// </summary>
        public static double Opacity
        {
            get { return _opacity; }
            set
            {
                _opacity = value;
                UpdateGrid();
            }
        }

        /// <summary>
        /// Updates the grid (if it already has been created) or initializes it
        /// otherwise.
        /// </summary>
        private static void UpdateGrid()
        {
            if (_squares != null)
            {
                var brush = new SolidColorBrush(_color);
                foreach (var square in _squares)
                {
                    square.InnerRect.Fill = brush;
                }
                if (_grid != null)
                {
                    _grid.Visibility = _visible ? Visibility.Visible : Visibility.Collapsed;
                    _grid.Opacity = _opacity;
                }
            }
            else
            {
                BuildGrid();
            }
        }

        /// <summary>
        /// Builds the grid.
        /// </summary>
        private static async void BuildGrid()
        {
            _squares = new List<DesingUnit>();

            var frame = Window.Current.Content as Frame;
            if (frame == null || VisualTreeHelper.GetChildrenCount(frame) == 0)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                          () =>
                          {
                              BuildGrid();
                          });
                return;
            }

            var child = VisualTreeHelper.GetChild(frame, 0);
            var childAsBorder = child as Border;
            var childAsGrid = child as Grid;
            if (childAsBorder != null)
            {
                // Not a pretty way to control the root visual, but I did not
                // want to implement using a popup.
                var content = childAsBorder.Child;
                if (content == null)
                {
                   //Deployment.Current.Dispatcher.BeginInvoke();

                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                          () =>
                          {
                              BuildGrid();
                          });
                    return;
                }

                childAsBorder.Child = null;
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    Grid newGrid = new Grid();
                    childAsBorder.Child = newGrid;
                    newGrid.Children.Add(content);
                    PrepareGrid(frame, newGrid);
                });
            }
            else if (childAsGrid != null)
            {
                PrepareGrid(frame, childAsGrid);
            }
            else
            {
                Debug.WriteLine("Dear developer:");
                Debug.WriteLine("Unfortunately the design overlay feature requires that the root frame visual");
                Debug.WriteLine("be a Border or a Grid. So the overlay grid just isn't going to happen.");
                return;
            }
        }
 
        private static void PrepareGrid(Frame frame, Grid parent)
        {
            var brush = new SolidColorBrush(_color);

            _grid = new Grid { IsHitTestVisible = false };

            // To support both orientations, unfortunately more visuals need to
            // be used. An alternate implementation would be to react to the
            // orientation change event and re-draw/remove squares.
            double width = frame.ActualWidth;
            double height = frame.ActualHeight;
            double max = Math.Max(width, height); 
         
            double tileWidth = 20;
            double tileHeight = 20;
            double x = 19.2;
            double y = 38.4;
            double block = 29.6;

            int xCounter = 1;
            int yCounter = 1;  

            for (; x < /*width*/ max; x += block)
            {
                y = 38.4;
                yCounter = 1;

                for (; y < /*height*/ max; y += block)
                {
                    var rect = new DesingUnit
                    {
                        Width = tileWidth,
                        Height = tileHeight,
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(x, y, 0, 0),
                        IsHitTestVisible = false,                        
                    };

                    rect.InnerRect.Fill = brush;
                    rect.InnerText.Text = "";

                    if (yCounter == 1) // first line                        
                       rect.InnerText.Text = xCounter.ToString();

                    if (xCounter == 1) // first column                        
                        rect.InnerText.Text = yCounter.ToString();

                    _grid.Children.Add(rect);
                    _squares.Add(rect);

                    yCounter++;
                }

                xCounter++;
            }

            _grid.Opacity = _opacity;
            _grid.Visibility = _visible ? Visibility.Visible : Visibility.Collapsed;
            
            // For performance reasons a single surface should ideally be used
            // for the grid.
            _grid.CacheMode = new BitmapCache();

            // Places the grid into the visual tree. It is never removed once
            // being added.
            parent.Children.Add(_grid);
}
    }
}