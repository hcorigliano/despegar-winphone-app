using Despegar.Core.Business.Enums;
using Despegar.WP.UI.Model.Classes.Flights;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace Despegar.WP.UI.Controls.Flights
{
    public sealed partial class QuantityPassagersControl : UserControl
    {
        public PassagersQuantity Passagers = new PassagersQuantity();
        public List<ComboBox> ChildrenControls { get; set; }

        private List<ChildrenAgeOption> _childAgeOptions;
        public List<ChildrenAgeOption> ChildAgeOptions
        {
            get
            {
                if (_childAgeOptions == null)
                {
                    _childAgeOptions = new List<ChildrenAgeOption>();
                    var resources = ResourceLoader.GetForCurrentView("Resources");

                    _childAgeOptions.Add(new ChildrenAgeOption() { DisplayText = resources.GetString("Flights_Passager_Baby_In_Arms"), Value = FlightSearchChildEnum.Infant });
                    _childAgeOptions.Add(new ChildrenAgeOption() { DisplayText = resources.GetString("Flights_Passager_Baby_In_Seat") as string, Value = FlightSearchChildEnum.Child });
                    _childAgeOptions.Add(new ChildrenAgeOption() { DisplayText = resources.GetString("Flights_Passager_Up_To_11_Years") as string, Value = FlightSearchChildEnum.Child });
                    _childAgeOptions.Add(new ChildrenAgeOption() { DisplayText = resources.GetString("Flights_Passager_Over_11_Years") as string, Value = FlightSearchChildEnum.Adult });
                }

                return _childAgeOptions;
            }
        }

        public int ChildrenInFlights
        {
            get
            {
                return ChildrenControls.Where(x => (x.SelectedItem as ChildrenAgeOption).Value == FlightSearchChildEnum.Child && ((FrameworkElement)x.Parent).Visibility == Visibility.Visible).Count();
            }
        }

        public int AdultsInFlights
        {
            get
            {
                return ChildrenControls.Where(x => (x.SelectedItem as ChildrenAgeOption).Value == FlightSearchChildEnum.Child && ((FrameworkElement)x.Parent).Visibility == Visibility.Visible).Count() + Passagers.AdultPassagerQuantity;
            }
        }

        public int InfantsInFlights
        {
            get
            {
                return ChildrenControls.Where(x => (x.SelectedItem as ChildrenAgeOption).Value == FlightSearchChildEnum.Infant && ((FrameworkElement)x.Parent).Visibility == Visibility.Visible).Count();
            }
        }

        public QuantityPassagersControl()
        {
            this.InitializeComponent();
            Passagers.AdultPassagerQuantity = 1;
            Passagers.ChildPassagerQuantity = 0;

            this.DataContext = new QuantityPassagersControViewModel() { Passengers = Passagers, ChildrenAgeOptions = ChildAgeOptions };

            ChildrenControls = new List<ComboBox>();
            ChildrenControls.Add(ChildrenAgePickerComboBox_0);
            ChildrenControls.Add(ChildrenAgePickerComboBox_1);
            ChildrenControls.Add(ChildrenAgePickerComboBox_2);
            ChildrenControls.Add(ChildrenAgePickerComboBox_3);
            ChildrenControls.Add(ChildrenAgePickerComboBox_4);
            ChildrenControls.Add(ChildrenAgePickerComboBox_5);
            ChildrenControls.Add(ChildrenAgePickerComboBox_6);

            ChildrenAgePickerComboBox_0.SelectedIndex = 0;
            ChildrenAgePickerComboBox_1.SelectedIndex = 0;
            ChildrenAgePickerComboBox_2.SelectedIndex = 0;
            ChildrenAgePickerComboBox_3.SelectedIndex = 0;
            ChildrenAgePickerComboBox_4.SelectedIndex = 0;
            ChildrenAgePickerComboBox_5.SelectedIndex = 0;
            ChildrenAgePickerComboBox_6.SelectedIndex = 0;
        }

        private void ReturnPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Hide All
            for (int i = 0; i < 7; i++)
                ((StackPanel)this.FindName("ChildrenAgePicker_" + i)).Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            // Show controls
            for (int i = 0; i < Passagers.ChildPassagerQuantity; i++)
                ((StackPanel)this.FindName("ChildrenAgePicker_" + i)).Visibility = Windows.UI.Xaml.Visibility.Visible;                    
        }

    }
}
