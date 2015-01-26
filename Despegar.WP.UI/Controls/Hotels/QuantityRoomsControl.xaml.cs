using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace Despegar.WP.UI.Controls.Hotels
{
    public sealed partial class QuantityRoomsControl : UserControl
    {
        public static readonly DependencyProperty RoomsProperty = DependencyProperty.Register("Rooms", typeof(int), typeof(QuantityRoomsControl), new PropertyMetadata(null));

        #region ** BoilerPlate Code **
        public event PropertyChangedEventHandler PropertyChanged;
        private void SetValueAndNotify(DependencyProperty property, object value, [CallerMemberName] string p = null)
        {
            SetValue(property, value);
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }
        #endregion

        public int Rooms
        {
            get { return (int)GetValue(RoomsProperty); }
            set
            {
                SetValueAndNotify(RoomsProperty, value);
            }
        }


        public QuantityRoomsControl()
        {
            this.InitializeComponent();
        }
    }
}
