using Despegar.Core.IService;
using Despegar.Core.Log;
using Despegar.WP.UI.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model.ViewModel.Hotels
{
    public class HotelsResultsViewModel
    {
        public class HotelsDetailsViewModel : ViewModelBase
        {
            public INavigator navigator { get; set; }
            public IHotelService hotelService { get; set; }


            public HotelsDetailsViewModel(INavigator navigator, IHotelService hotelService, IBugTracker t)
                : base(t)
            {
                this.navigator = navigator;
                this.hotelService = hotelService;
            }

        }

    }
}
