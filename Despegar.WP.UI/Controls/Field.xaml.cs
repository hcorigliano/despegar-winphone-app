using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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
    public enum InputType
    { 
        Numeric,  // [0-9]
        TextOnly,
        AlphaNumeric,
        Email
        //NumberMath,  /  [0-9] plus , and .
    }

    public sealed partial class Field : UserControl
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(Field), new PropertyMetadata(null));
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(Field), new PropertyMetadata(null));
        public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register("PlaceholderText", typeof(string), typeof(Field), new PropertyMetadata(null));
        public static readonly DependencyProperty InputTypeProperty = DependencyProperty.Register("InputType", typeof(InputType), typeof(Field), new PropertyMetadata(InputType.AlphaNumeric, inputTypeChanged));

        public event RoutedEventHandler TextChanged;

        private Regex regex;
        private string lastInputText = String.Empty;

        private static void inputTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Field;            
            InputScopeNameValue scopeValue;

            switch ((InputType)e.NewValue)
            {
                case Despegar.WP.UI.Controls.InputType.Numeric:
                    scopeValue = InputScopeNameValue.Number;
                    control.regex = new Regex(@"^[0-9]*$");
                    break;
                case Despegar.WP.UI.Controls.InputType.TextOnly:
                    scopeValue = InputScopeNameValue.Default;
                    control.regex = new Regex(@"^[a-zA-Z\s]*$");  
                    break;
                case Despegar.WP.UI.Controls.InputType.AlphaNumeric:
                    scopeValue = InputScopeNameValue.AlphanumericFullWidth;
                    control.regex = new Regex(@"^[a-zA-Z0-9\s]*$");  
                    break;
                case Despegar.WP.UI.Controls.InputType.Email:
                   scopeValue = InputScopeNameValue.EmailSmtpAddress;
                   control.regex = new Regex(@"^.*$");
                    break;
                default:
                    return;
            }

             // Set input scope
             InputScope scope = new InputScope();
             InputScopeName name = new InputScopeName();
             name.NameValue = scopeValue;
             scope.Names.Add(name);
             control.InnerTextbox.InputScope = scope;
        }

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

        // Bindable Property from XAML
        public InputType InputType
        {
            get { return (InputType)GetValue(InputTypeProperty); }
            set
            {
                SetValueAndNotify(InputTypeProperty, value);
            }
        }

        public Field()
        {
            this.InitializeComponent();           
            (this.Content as FrameworkElement).DataContext = this;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox control = sender as TextBox;

            if (regex.IsMatch(control.Text) || control.Text == "")
            {
                // Input is valid

                // Notify text change
                if(TextChanged != null)
                  TextChanged(sender, e);
            }
            else
            {
                control.Text = lastInputText;
                control.Select(control.Text.Length, 0);
            }

            // Save the current value to resume it if the next input was invalid
            lastInputText = control.Text;

            
        }     
    }
}