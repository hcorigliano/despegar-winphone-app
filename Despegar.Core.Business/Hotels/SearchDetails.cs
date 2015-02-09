using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Business.Hotels
{
    public class HotelsSearchParameters
    {

        public int Rooms 
        {
            get
            {
                if (this.distribution != null)
                    return (distribution.Split('!')).Count();
                else return 1;
            }
        }
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
        public int Adults
        {
            get
            {
                int count = 0;
                if(this.distribution != null)
                {
                    string[] Ad = distribution.Split('!');
                    foreach(string str in Ad)
                    {
                        count += Convert.ToInt32((str.Split('-'))[0]);
                    }
                }
                return count;        
            }
        }
        public int ? Childs
        {
            get
            {
                int ? count = 0;
                if (this.distribution != null)
                {
                    string[] Ad = distribution.Split('!');
                    foreach (string str in Ad)
                    {
                        count += (str.Split('-')).Count() - 1;
                    }
                }
                return (count == 0) ? null : count;
            }
        }
        public string Checkin { get; set; }
        public string Checkout { get; set; }
        public int destinationNumber { get; set; }
        public string distribution { get; set; }
        public string currency { get; set; }
        public int offset { get; set; }
        public int limit { get; set; }
        public string order { get; set; }
        public string extraParameters { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }

    }
}
