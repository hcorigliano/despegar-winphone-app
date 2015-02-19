using Despegar.Core.Neo.Business.Configuration;
using Despegar.WP.UI.Common;
using Despegar.WP.UI.Controls;
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

namespace Despegar.WP.UI.Product.Flights.Checkout.Controls
{
    public sealed partial class NationalitySelection : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(object), typeof(NationalitySelection), new PropertyMetadata(null));
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(NationalitySelection), new PropertyMetadata(null));      
        private ModalPopup popup;

        #region ** BoilerPlate Code **
        public event PropertyChangedEventHandler PropertyChanged;
        private void SetValueAndNotify(DependencyProperty property, object value, [CallerMemberName] string p = null)
        {
            SetValue(property, value);
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }

        internal void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        private string _NationalityText;
        public string NationalityText
        {
            get { return _NationalityText; }
            set
            {
                _NationalityText = value;
                NotifyPropertyChanged("NationalityText");
                NationalityTextBox.Text = value; //TODO : Fijarse por que no funciona directamente con el propertychange.
            }
        }
       
        // Bindable Property from XAML       
        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set
            {
                SetValueAndNotify(ItemsSourceProperty, value);
            }
        }

        // Bindable Property from XAML 
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set
            {
                SetValueAndNotify(SelectedItemProperty, value);
            }
        }

        public NationalitySelection()
        {
            this.InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;

            popup = new ModalPopup(new NationalitySelectionPopup() { DataContext = this });
            
        }

        private void SelectionChangedListBox(object sender, SelectionChangedEventArgs e)
        {
            CountryFields Selected = (CountryFields)(((ListView)sender).SelectedItem);
            NationalityTextBox.Text = Selected.name;
        }

        private void ShowPopup(object sender, RoutedEventArgs e)
        {                      
            popup.Show();
        }

        public void SetDisplayText(string displayText)
        {
            NationalityTextBox.Text = displayText;
        }

    }
}
