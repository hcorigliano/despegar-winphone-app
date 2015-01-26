using Despegar.Core.Business.Hotels.CitiesAvailability;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Hotels
{
    public class City : IdCodeAndName
    {
        public IdCodeAndName country { get; set; }
    }
}
