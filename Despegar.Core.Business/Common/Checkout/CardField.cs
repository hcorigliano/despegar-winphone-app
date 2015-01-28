using Despegar.Core.Business.Common.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Common.Checkout
{
    public class CardField
    {
        public bool required { get; set; }
        public string data_type { get; set; }
        public RegularField number { get; set; }
        public Expiration expiration { get; set; }
        public RegularField security_code { get; set; }
        public RegularOptionsField owner_type { get; set; }
        public RegularField owner_name { get; set; }
        public OwnerDocument owner_document { get; set; }
        public RegularOptionsField owner_gender { get; set; }

        // Custom
        public List<int> YearRange
        {
            get
            {
                string temp = this.expiration.from;
                int fromDate = Convert.ToInt32((temp.Split(new Char[] { '-' }))[0]);
                temp = this.expiration.to;
                int toDate = Convert.ToInt32((temp.Split(new Char[] { '-' }))[0]);

                List<int> list = new List<int>();
                for (int i = fromDate; i <= toDate; i++)
                    list.Add(i);


                return list;
            }
        }

        public List<int> Months
        {
            get
            {
                return new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            }
        }
    }
}