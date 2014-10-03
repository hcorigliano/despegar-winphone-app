using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingCompletePostResponse
{
    public class RiskQuestion
    {
        public string id { get; set; }
        public string order { get; set; }
        public string title { get; set; }
        public string category { get; set; }
        public string mandatory { get; set; }
        public string free_text { get; set; }
        public FreeTextDescription free_text_description { get; set; }
        public Answers answers { get; set; }
    }
}
