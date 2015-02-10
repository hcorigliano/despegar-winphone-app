using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Business.Hotels.HotelDetails
{
    public class HotelDatails
    {
        public string id { get; set; }
        public string token { get; set; }
        public Hotel hotel { get; set; }
        public string suggested_room_choice { get; set; }
        public List<Roompack> roompacks { get; set; }
    }
}
