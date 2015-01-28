using Despegar.Core.Business.Hotels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model.ViewModel.Hotels
{
    public class HotelsCrossParameters
    {
        public HotelsSearchParameters SearchParameters { get; set; }
        public HotelsExtraData HotelsExtraData { get; set; }
        public string IdSelectedHotel { get; set; }

        public HotelsCrossParameters()
        {
            this.SearchParameters = new HotelsSearchParameters();
            this.HotelsExtraData = new HotelsExtraData();
        }
    }
}
