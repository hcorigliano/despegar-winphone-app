using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Hotels.UserReviews.V1
{
    public class Review
    {
        public List<Comment> comments { get; set; }
        public User user { get; set; }
        public object replies { get; set; }
        public Scores scores { get; set; }
        public int averageScore { get; set; }
        public string status { get; set; }
        public List<string> categories { get; set; }
        public string provider { get; set; }
        public string reviewDate { get; set; }
        public string reviewTime { get; set; }
        public Destination destination { get; set; }
        public object productTypeCode { get; set; }
        public long id { get; set; }
    }
}
