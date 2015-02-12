using Despegar.WP.UI.Common;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Despegar.WP.UI.Controls.Flights
{   
    public sealed partial class DateControl : UserControl
    {
        public static readonly DependencyProperty FromDateProperty = DependencyProperty.Register("FromDate", typeof(DateTimeOffset), typeof(DateControl), new PropertyMetadata(null));
        public static readonly DependencyProperty ToDateProperty = DependencyProperty.Register("ToDate", typeof(DateTimeOffset), typeof(DateControl), new PropertyMetadata(null));

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
        public DateTimeOffset FromDate
        {
            get { return (DateTimeOffset)GetValue(FromDateProperty); }
            set
            {
                SetValueAndNotify(FromDateProperty, value);                
            }
        }

        // Bindable Property from XAML
        public DateTimeOffset ToDate
        {
            get { return (DateTimeOffset)GetValue(ToDateProperty); }
            set
            {
                SetValueAndNotify(ToDateProperty, value);
            }
        }
     
        public DateControl()
        {
            this.InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;

            this.departure.MinYear = DateTimeOffset.Now;
            this.departure.MaxYear = DateTimeOffset.Now.AddYears(1);

            this.ToDateControl.MinYear = DateTimeOffset.Now;
            this.ToDateControl.MaxYear = DateTimeOffset.Now.AddYears(1);
        }        
    }
}
