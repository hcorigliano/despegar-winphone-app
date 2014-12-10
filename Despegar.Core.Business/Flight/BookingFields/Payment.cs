using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingFields
{
    public class Payment
    {
        // MAPI fields
        public bool required { get; set; }
        public Installment installment { get; set; }
        public Card2 card { get; set; }
        public InvoiceArg invoice { get; set; }
        public string data_type { get; set; }

        // Custom
        public List<int> YearRange 
        {
            get
            {
                string temp = this.card.expiration.from;
                int fromDate = Convert.ToInt32((temp.Split(new Char[] { '-' }))[0]);
                temp = this.card.expiration.to;
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