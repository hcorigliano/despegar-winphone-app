using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Despegar.WP.UI.Developer.Controls
{
    public sealed partial class MeasureControl : UserControl
    {
        private Point position;
        public Point Position 
        { 
            get { return position; }
            set 
            { 
                position = value;
                UpdateControl();
            }         
        }

        private void UpdateControl()
        {
            var x = Math.Round(position.X);
            var y = Math.Round(position.Y);

            TextX.Text = x.ToString();
            TextY.Text = y.ToString();
            //TextX.Margin = new Thickness(x - 80, y - 80, 0, 0);
            //TextY.Margin = new Thickness(x + 80, y - 80, 0, 0);

            LineX.Margin = new Thickness(x, 0, 0, 0);
            LineY.Margin = new Thickness(0, y, 0, 0);
        }

        public MeasureControl()
        {
            this.InitializeComponent();
        }
       
    }
}