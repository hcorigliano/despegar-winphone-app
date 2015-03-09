using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Hotels.CustomUserReviews
{
    public class CustomReviewsItem
    {
        public string name { get; set; }
        public string rating { get; set; }
        public string country { get; set; }
        public string countryCode { get; set; }
        public string description { get; set; }
        public string good { get; set; }
        public string bad { get; set; }
    }
}
