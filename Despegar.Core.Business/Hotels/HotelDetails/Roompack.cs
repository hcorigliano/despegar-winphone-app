using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Hotels.HotelDetails
{
    public class Roompack
    {
        public string name { get; set; }
        public List<Room> rooms { get; set; }
        public List<RoomAvailability> room_availabilities { get; set; }
    }
}
