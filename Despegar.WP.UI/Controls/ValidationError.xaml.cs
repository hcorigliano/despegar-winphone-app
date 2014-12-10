using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
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
    public sealed partial class ValidationError : UserControl
    {
        public static readonly DependencyProperty ErrorCodeProperty = DependencyProperty.Register("ErrorCode", typeof(string), typeof(ValidationError), new PropertyMetadata(null, OnErrorCodeChanged));
        public static readonly DependencyProperty ErrorPrefixProperty = DependencyProperty.Register("ErrorPrefix", typeof(string), typeof(ValidationError), new PropertyMetadata(null));


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
        public string ErrorCode
        {
            get { return (string)GetValue(ErrorCodeProperty); }
            set
            {              
                SetValueAndNotify(ErrorCodeProperty, value);
            }
        }

        // Bindable Property from XAML
        public string ErrorPrefix
        {
            get { return (string)GetValue(ErrorPrefixProperty); }
            set
            {
                SetValueAndNotify(ErrorPrefixProperty, value);
            }
        }


        private static void OnErrorCodeChanged(DependencyObject source, DependencyPropertyChangedEventArgs e) 
        {
            ValidationError control = source as ValidationError;
            // Error code has changed
            object resourceString = "";
            string resourceKey = control.ErrorPrefix + "_ERROR_" + control.ErrorCode;
            resourceString = (new ResourceLoader()).GetString(resourceKey);
            control.TextLabel.Text = resourceString as string;
        }

        public ValidationError()
        {
            this.InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;
        }
    }
}
