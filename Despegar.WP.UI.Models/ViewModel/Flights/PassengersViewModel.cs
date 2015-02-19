using Despegar.Core.Neo.Business;
using Despegar.Core.Neo.Business.Enums;
using Despegar.WP.UI.Model.Classes.Flights;
using Despegar.WP.UI.Model.ViewModel.Classes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel.Resources;

namespace Despegar.WP.UI.Model.ViewModel.Flights
{
    
    public class PassengersViewModel : Bindable
    {
        public ObservableCollection<Minor> MinorsList { get; set; }

        private int generalAdults;
        public int GeneralAdults
        {
            get { return generalAdults; }
            set
            {
                generalAdults = value;
                OnPropertyChanged();
                OnPropertyChanged("ChildrenOptions");
                OnPropertyChanged("GeneralMinors");
                OnPropertyChanged("TotalAdults");
            }
        }

        private int generalMinors;
        public int GeneralMinors
        {
            get { return generalMinors; }
            set
            {
                if (generalMinors != value)
                {
                    // Update ChildrenList Items
                    if (MinorsList.Count < value)
                    {
                        ChildrenAgeOption defaultOption = ChildAgeOptions.First();

                        // Add new empty childrens
                        for (int i = MinorsList.Count; i < value; i++) 
                        {
                            MinorsList.Add(new Minor() { Index = i + 1, OptionsItems = ChildAgeOptions, SelectedAge = defaultOption});
                        }
                    } else { 
                      // Remove children
                        for (int i = MinorsList.Count-1; i >= value; i--)
                            MinorsList.RemoveAt(i);
                    }

                    generalMinors = value;
                    OnPropertyChanged();
                    OnPropertyChanged("AdultOptions");
                    OnPropertyChanged("GeneralAdults");

                    OnPropertyChanged("TotalAdults");
                    OnPropertyChanged("Children");
                    OnPropertyChanged("Infants");

                    OnPropertyChanged("MinorsList");
                }
            }
        }

        public int Adults { get { return GeneralAdults + MinorsList.Count(x => x.SelectedAge.Value == FlightSearchChildEnum.Child); } }
        public int Children { get { return MinorsList.Count(x => x.SelectedAge.Value == FlightSearchChildEnum.Child); } }
        public int Infants { get { return MinorsList.Count(x => x.SelectedAge.Value == FlightSearchChildEnum.Infant); } }
       
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

        public PassengersViewModel()
        {
            this.GeneralAdults = 1;
            this.GeneralMinors = 0;
            MinorsList = new ObservableCollection<Minor>();
        }
        
        /// <summary>
        /// Returns the available options for Adults passengers
        /// </summary>
        public IEnumerable<int> AdultOptions
        {
            get { 
             List<int> options =  new List<int>();

                // 1 is the Minimum Adult count
             for(int i = 1; i <= 8 - GeneralMinors; i++)
                options.Add(i);

             return options;
            }
        }

        /// <summary>
        /// Returns the available options for Children passengers
        /// </summary>
        public IEnumerable<int> ChildrenOptions
        {
            get
            {
                List<int> options = new List<int>();

                // 0 is the Minimum Child count, and 1 adult is always present
                for (int i = 0; i <= 8 - GeneralAdults; i++)
                {
                    options.Add(i);                    
                }

                return options;
            }
        }
    }
}