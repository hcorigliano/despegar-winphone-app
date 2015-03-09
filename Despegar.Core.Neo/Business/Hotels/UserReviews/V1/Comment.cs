using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Hotels.UserReviews.V1
{
    public class Comment
    {
        public string title { get; set; }
        public string description { get; set; }
        public string language { get; set; }
        public string good { get; set; }
        public string bad { get; set; }
        public string type { get; set; }
    }
}
