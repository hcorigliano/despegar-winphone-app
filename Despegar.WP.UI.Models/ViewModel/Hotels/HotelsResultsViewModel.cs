using Despegar.Core.Business.Hotels.CitiesAvailability;
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

    public class HotelsResultsViewModel : ViewModelBase
    {
        private IHotelService hotelService { get; set; }

        public INavigator Navigator { get; set; }        
        public CitiesAvailability CitiesAvailability { get; set; }


        public HotelsResultsViewModel(INavigator navigator, IHotelService hotelService, IBugTracker t)
            : base(t)
        {
            this.Navigator = navigator;
            this.hotelService = hotelService;
        }
    }
}