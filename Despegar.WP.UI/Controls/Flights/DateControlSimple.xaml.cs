using Despegar.WP.UI.Common;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Despegar.WP.UI.Controls.Flights
{
 
    public sealed partial class DateControlSimple : UserControl
    {
        public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register("SelectedDate", typeof(DateTimeOffset), typeof(DateControlSimple), new PropertyMetadata(null));

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
        public DateTimeOffset SelectedDate
        {
            get { return (DateTimeOffset)GetValue(SelectedDateProperty); }
            set
            {
                SetValueAndNotify(SelectedDateProperty, value);
            }
        }

        public DateControlSimple() 
        {
            this.InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;
        }

    }
}
