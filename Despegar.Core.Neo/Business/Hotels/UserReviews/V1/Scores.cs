using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Hotels.UserReviews.V1
{
    public class Scores
    {
        public int cleaning { get; set; }
        public int qualityprice { get; set; }
        public int servicePersonal { get; set; }
        public int service { get; set; }
        public int location { get; set; }
        public int building { get; set; }
        public int? room { get; set; }
    }
}
