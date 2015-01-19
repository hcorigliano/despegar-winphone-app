using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Business.Hotels
{
    public class SearchDetails
    {
        public int Rooms { get; set; }
        public int Nights
        {
            get
            {
                if (Checkin != null && Checkout != null)
                {
                    DateTime checkinDateTime = DateTime.ParseExact(Checkin, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    DateTime checkoutDateTime = DateTime.ParseExact(Checkout, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    return (checkoutDateTime - checkinDateTime).Days;
                }

                return 0;
            }
        }
        public int Adults { get; set; }
        public int Childs { get; set; }
        public string Checkin { get; set; }
        public string Checkout { get; set; }
    }
}
