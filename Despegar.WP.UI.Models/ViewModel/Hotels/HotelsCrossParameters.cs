using Despegar.Core.Neo.Business.Hotels;
using Despegar.Core.Neo.Business.Hotels.HotelDetails;
using Despegar.Core.Neo.Business.Hotels.SearchBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model.ViewModel.Hotels
{
    public class HotelsCrossParameters
    {
        public HotelSearchModel SearchModel { get; set; }        
        public HotelsExtraData HotelsExtraData { get; set; }
        public string IdSelectedHotel { get; set; }
        public BedOption BedSelected { get; set; }

        public HotelsBookingFieldsRequest BookRequest { get; set; }

        public HotelsCrossParameters()
        {
            this.HotelsExtraData = new HotelsExtraData();
        }

        // UPA
        public int UPA_SelectedItemIndex { get; set; }
    }
}