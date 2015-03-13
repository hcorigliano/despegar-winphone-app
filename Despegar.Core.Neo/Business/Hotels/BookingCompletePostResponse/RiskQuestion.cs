using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Hotels.BookingCompletePostResponse
{
    public class RiskQuestion
    {
        public string id { get; set; }
        public string order { get; set; }
        public string title { get; set; }
        public string category { get; set; }
        public bool mandatory { get; set; }
        public bool free_text { get; set; }
        public List<Answer> answers { get; set; }
    }
}
