using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Hotels.UserReviews.V1
{
    public class Review
    {
        public Comments comments { get; set; }
        public User user { get; set; }
        public object replies { get; set; }
        public Scores scores { get; set; }
        public double averageScore { get; set; }
        public string status { get; set; }
        public List<string> categories { get; set; }
        public string provider { get; set; }
        public string reviewDate { get; set; }
        public string reviewTime { get; set; }
        public int id { get; set; }
    }
}
