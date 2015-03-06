using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Business.Common.Checkout
{
    public class PaymentInstallments
    {
        public int quantity { get; set; }
        public decimal first { get; set; }
        public decimal others { get; set; }
    }
}
