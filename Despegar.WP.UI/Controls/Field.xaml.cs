using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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


namespace Despegar.WP.UI.Controls
{
    public sealed partial class Field : UserControl
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(Field), new PropertyMetadata(null));
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(Field), new PropertyMetadata(null));
        public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register("PlaceholderText", typeof(string), typeof(Field), new PropertyMetadata(null));
        

        #region ** BoilerPlate Code **
        public event PropertyChangedEventHandler PropertyChanged;
        private void SetValueAndNotify(DependencyProperty property, object value, [CallerMemberName] string p = null)
        {
            SetValue(property, value);
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }
        #endregion

        // Bindable Property from XAML
        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set
            {
                SetValueAndNotify(ValueProperty, value);
            }
        }

        // Text Label set by using the X:Uid
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set
            {
                SetValueAndNotify(LabelProperty, value);
            }
        }

        // Bindable Property from XAML
        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set
            {
                SetValueAndNotify(PlaceholderTextProperty, value);
            }
        }

        public Field()
        {
            this.InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;
        }
    }
}