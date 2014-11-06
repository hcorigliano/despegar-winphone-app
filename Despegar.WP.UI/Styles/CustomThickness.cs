using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Despegar.WP.UI.Styles
{
    public class CustomThickness : DependencyObject
    {
        public CustomThickness() : base() { }

        public double Left
        {
            get { return (double)GetValue(LeftProperty); }
            set { SetValue(LeftProperty, value); }
        }
         
        public static readonly DependencyProperty LeftProperty =   DependencyProperty.Register("Left", typeof(double), typeof(CustomThickness), new PropertyMetadata(0.0, OnPropertyChanged));
        public static readonly DependencyProperty TopProperty = DependencyProperty.Register("Top", typeof(double), typeof(CustomThickness), new PropertyMetadata(0.0, OnPropertyChanged));
        public static readonly DependencyProperty RightProperty = DependencyProperty.Register("Right", typeof(double), typeof(CustomThickness), new PropertyMetadata(0.0, OnPropertyChanged));
        public static readonly DependencyProperty BottomProperty = DependencyProperty.Register("Bottom", typeof(double), typeof(CustomThickness), new PropertyMetadata(0.0, OnPropertyChanged));
        public static readonly DependencyProperty ThicknessProperty = DependencyProperty.Register("Thickness", typeof(Thickness), typeof(CustomThickness), new PropertyMetadata(0.0));
 
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomThickness custom = d as CustomThickness;

            Thickness thickness = new Thickness(custom.Left, custom.Top, custom.Right, custom.Bottom);
            custom.Thickness = thickness;
        }

        public double Top
        {
            get { return (double)GetValue(TopProperty); }
            set { SetValue(TopProperty, value); }
        }

        public double Right
        {
            get { return (double)GetValue(RightProperty); }
            set { SetValue(RightProperty, value); }
        }

        public double Bottom
        {
            get { return (double)GetValue(BottomProperty); }
            set { SetValue(BottomProperty, value); }
        }

        public Thickness Thickness
        {
            get { return (Thickness)GetValue(ThicknessProperty); }
            set { SetValue(ThicknessProperty, value); }
        }

    }
}