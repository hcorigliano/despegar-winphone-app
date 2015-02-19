using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Hotels.UserReviews
{
    public class Qualifications
    {
        public List<Averageable> averageables { get; set; }
        public List<string> additives { get; set; }
        public double overall_rating { get; set; }
    }
}
