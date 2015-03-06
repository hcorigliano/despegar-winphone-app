using Despegar.Core.Neo.Business;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Business.Hotels.SearchBox
{
    public class PassengersForRooms : Bindable
    {
        public int Index { get; set; }
        public int IndexZeroBased { get { return Index - 1; } }
        public ObservableCollection<HotelsMinorsAge> MinorsAge { get; set; }
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
                    if (MinorsAge.Count < value)
                    {
                        for (int i = MinorsAge.Count; i < value; i++)
                        {
                            MinorsAge.Add(new HotelsMinorsAge { SelectedAge = 0, Index = i + 1 });
                        }
                    }
                    else
                    {
                        for (int i = MinorsAge.Count - 1; i >= value; i--)
                        {
                            MinorsAge.RemoveAt(i);
                        }
                    } 
                }


                generalMinors = value;
                OnPropertyChanged();
                OnPropertyChanged("AdultOptions");
                OnPropertyChanged("GeneralAdults");

            }
        }

        /// <summary>
        /// Returns the available options for Adults passengers
        /// </summary>
        public IEnumerable<int> AdultOptions
        {
            get
            {
                List<int> options = new List<int>();

                // 1 is the Minimum Adult count
                for (int i = 1; i <= 8 - GeneralMinors; i++)
                    options.Add(i);

                return Enumerable.Range(1, 8 - GeneralMinors);
            }
        }

        /// <summary>
        /// Returns the available options for Children passengers
        /// </summary>
        public IEnumerable<int> ChildrenOptions
        {
            get
            {                
                return Enumerable.Range(0, 9 - GeneralAdults);
            }
        }

        public PassengersForRooms()
        {
            MinorsAge = new ObservableCollection<HotelsMinorsAge>();
            this.GeneralAdults = 1;
            this.GeneralMinors = 0;
        }
    }
}