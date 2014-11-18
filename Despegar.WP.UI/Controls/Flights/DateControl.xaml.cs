using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Despegar.WP.UI.Controls.Flights
{   
    public sealed partial class DateControl : UserControl
    {
        public static readonly DependencyProperty FromDateProperty = DependencyProperty.Register("FromDate", typeof(DateTimeOffset), typeof(DateControl), new PropertyMetadata(null));
        public static readonly DependencyProperty ToDateProperty = DependencyProperty.Register("ToDate", typeof(DateTimeOffset), typeof(DateControl), new PropertyMetadata(null));

        // Bindable Property from XAML
        public DateTimeOffset FromDate
        {
            get { return (DateTimeOffset)GetValue(FromDateProperty); }
            set
            {
                SetValue(FromDateProperty, value);                
            }
        }

        // Bindable Property from XAML
        public DateTimeOffset ToDate
        {
            get { return (DateTimeOffset)GetValue(ToDateProperty); }
            set
            {
                SetValue(ToDateProperty, value);
            }
        }
     

        public DateControl()
        {
            this.InitializeComponent();         
        }        
    }
}
