using Despegar.Core.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model.ViewModel.Hotels
{
    public class RoomsViewModel : ViewModelBase
    {

        private int roomsSelectedOpotion;
        public int RoomsSelectedOpotion { get; set; }

        public int Rooms { get; set; }

        public RoomsViewModel(IBugTracker t) : base(t)
        {
            this.Rooms = 1;
        }
    }
}
