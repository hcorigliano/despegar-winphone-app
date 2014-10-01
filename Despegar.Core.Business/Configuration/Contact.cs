using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Configuration
{
    public class Contact
    {
        public string callcenter { get; set; }
        public string csd { get; set; }
        public string skype { get; set; }
        public Hours hours { get; set; }
        public Mobile mobile { get; set; }
        public string callingCode { get; set; }
    }
}
