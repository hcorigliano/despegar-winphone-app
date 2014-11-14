using Despegar.Core.Business.Enums;
using Despegar.WP.UI.Model.Classes.Flights;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Despegar.WP.UI.Model.ViewModel.Flights;
using Windows.UI.Xaml.Data;

namespace Despegar.WP.UI.Controls.Flights
{
    public sealed partial class QuantityPassagersControl : UserControl
    {
        public static readonly DependencyProperty AdultsProperty = DependencyProperty.Register("Adults", typeof(int), typeof(QuantityPassagersControl), new PropertyMetadata(null));
        public static readonly DependencyProperty ChildrenProperty = DependencyProperty.Register("Children", typeof(int), typeof(QuantityPassagersControl), new PropertyMetadata(null));
        public static readonly DependencyProperty InfantsProperty = DependencyProperty.Register("Infants", typeof(int), typeof(QuantityPassagersControl), new PropertyMetadata(null));

        // Bindable Property from XAML
        public int Adults
        {
            get { return (int)GetValue(AdultsProperty); }
            set
            {
                SetValue(AdultsProperty, value);
            }
        }
        // Bindable Property from XAML
        public int Children
        {
            get { return (int)GetValue(ChildrenProperty); }
            set
            {
                SetValue(ChildrenProperty, value);
            }
        }
        // Bindable Property from XAML
        public int Infants
        {
            get { return (int)GetValue(InfantsProperty); }
            set
            {
                SetValue(InfantsProperty, value);
            }
        }


        public QuantityPassagersControl()
        {
            this.InitializeComponent();
        }
    }
}