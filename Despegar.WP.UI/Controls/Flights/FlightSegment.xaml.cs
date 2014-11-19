﻿using Despegar.WP.UI.Common;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace Despegar.WP.UI.Controls.Flights
{
    public sealed partial class FlightSegment : UserControl
    {
        public static readonly DependencyProperty ToDateProperty = DependencyProperty.Register("IsReturn", typeof(bool), typeof(FlightSegment), new PropertyMetadata(null));

        public bool IsReturn {get; set;}

        #region ** BoilerPlate Code **
        public event PropertyChangedEventHandler PropertyChanged;
        private void SetValueAndNotify(DependencyProperty property, object value, [CallerMemberName] string p = null)
        {
            SetValue(property, value);
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }
        #endregion

        public FlightSegment() 
        {
            this.InitializeComponent();
            //(this.Content as FrameworkElement).DataContext = this;
        }
    }
}