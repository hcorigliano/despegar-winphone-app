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
    public sealed partial class FieldCombo : UserControl
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(FieldCombo), new PropertyMetadata(null));
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(object), typeof(FieldCombo), new PropertyMetadata(null));
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(FieldCombo), new PropertyMetadata(null));

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

        // Bindable Property from XAML
        public object ItemsSource
        {
            get { return GetValue(ItemsSourceProperty); }
            set
            {
                SetValueAndNotify(ItemsSourceProperty, value);
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

        public FieldCombo()
        {
            this.InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;
        }
    }
}