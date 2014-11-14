using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Despegar.WP.UI.Developer.Controls
{
    public sealed partial class UserControlDesignGrid : UserControl
    {
        public UserControlDesignGrid()
        {
            this.InitializeComponent();
            double square = 20;
            double offset = 9.6;
            int maxWidth = 25;
            int maxHeight = 20*2;
            int y = 0;
            int x = 0;

            for (; y <= maxHeight; y++)
            {
                if (y % 2 == 0)
                {
                    TheGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(square) });
                    x = 0;
                    for (; x <= maxWidth; x++)
                    {
                        if (x % 2 == 0)
                        {
                            TheGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(square) });

                            var rect = new Rectangle
                            {
                                Width = 20,
                                Height = 20,                             
                                IsHitTestVisible = false,
                                Opacity = 0.7,
                                Fill = new SolidColorBrush(Color.FromArgb(220, 255, 0, 0)),
                            };

                            Grid.SetColumn(rect, x);
                            Grid.SetRow(rect, y);
                            TheGrid.Children.Add(rect);                           
                        }
                        else
                        {
                            TheGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(offset) });
                        }
                    }
                }
                else
                {
                    TheGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(offset) });
                }                
            }
        }
    }
}
