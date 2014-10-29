using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Despegar.WP.UI.Developer.Controls
{
    public sealed partial class ColorPicker : UserControl
    {
        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorPicker), null);

        public List<ColorOption> ColorList { get; set; }
        private ColorOption _dropdownChosenColor;
        
        public ColorOption DropdownChosenColor
        {
            get { return _dropdownChosenColor; }
            set
            {
                if (value != _dropdownChosenColor) 
                {                
                    _dropdownChosenColor = value;
                    OnPropertyChanged("DropdownChosenColor");

                    SelectedColor = value.Color;
               }
            }
        }
    
        // Bindable Property from XAML
        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { 
                SetValue(SelectedColorProperty, value);
                DropdownChosenColor = ColorList.First(x => x.Color == value);
            }
        }

        public ColorPicker()
        {
            this.InitializeComponent();
            this.ColorList = ColorOption.ColorData();            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
