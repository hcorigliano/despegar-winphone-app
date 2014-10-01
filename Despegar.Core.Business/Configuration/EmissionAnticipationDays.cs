using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Configuration
{
    public class EmissionAnticipationDays
    {
        public DaysAndLastAvailableHour flights { get; set; }
        public DaysAndLastAvailableHour hotels { get; set; }
        public DaysAndLastAvailableHour packages { get; set; }
        public DaysAndLastAvailableHour cruises { get; set; }
        public DaysAndLastAvailableHour cars { get; set; }
    }
}
