using Despegar.Core.Log;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model.ViewModel.Hotels
{
    public class RoomsViewModel : ViewModelBase
    {

        public ObservableCollection<PassengersForRooms> RoomsDetailList { get; set; }
        public int Rooms { get; set; }
        private int roomsSelectedOption;
        public int RoomsSelectedOption 
        {
            get { return roomsSelectedOption; }
            set
            {
                if(roomsSelectedOption != value)
                {
                    if (RoomsDetailList.Count < value)
                    {
                        for (int i = RoomsDetailList.Count; i < value; i++)
                        {
                            RoomsDetailList.Add(new PassengersForRooms() {Index = i + 1 });
                        }
                    }
                    else
                    {
                        for (int i = RoomsDetailList.Count -1; i >= value; i--)
                        {
                            RoomsDetailList.RemoveAt(i);
                        }
                    }
                }
                roomsSelectedOption = value;
            }
        }

        public int Adults
        {
            get
            {
                return RoomsDetailList.Sum(x => x.GeneralAdults);
            }
        }

        public int Minors
        {
            get
            {
                return RoomsDetailList.Sum(x => x.GeneralMinors);
            }
        }

        public IEnumerable<int> RoomOptions
        {
            get
            {
                IEnumerable<int> options = Enumerable.Range(1, 4);
                return options;
            }
        }

        public RoomsViewModel(IBugTracker t) : base(t)
        {
            RoomsDetailList = new ObservableCollection<PassengersForRooms>();
            this.Rooms = 1;
            this.RoomsSelectedOption = 1;
        }

    }
}
