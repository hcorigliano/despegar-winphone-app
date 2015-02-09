using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Hotels.UserReviews
{
    public class Description
    {
        public MultiLanguageStrings title { get; set; }
        public MultiLanguageStrings good { get; set; }
        public MultiLanguageStrings description { get; set; }
        public MultiLanguageStrings bad { get; set; }
    }
}
