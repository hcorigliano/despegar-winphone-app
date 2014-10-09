using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.LegacyCore.Connector.Domain.API
{
    public class ValidationCreditcards
    {
        public List<ValidationCreditcard> data { get; set; }
    }

    public class ValidationCreditcard
    {
        public string cardCode { get; set; }
        public string bankCode { get; set; }
        public string cardType { get; set; }
        public string numberRegex { get; set; }
        public string lengthRegex { get; set; }
        public string codeRegex { get; set; }
    }
}
