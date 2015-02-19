using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Hotels.UserReviews
{
    public class Item
    {
        public string id { get; set; }
        public string item_id { get; set; }
        public string type { get; set; }
        public string reviewed_date { get; set; }
        public string original_language { get; set; }
        public string provider { get; set; }
        public User user { get; set; }
        public Utility utility { get; set; }
        public Qualifications qualifications { get; set; }
        public List<Description> descriptions { get; set; }
    }
}
