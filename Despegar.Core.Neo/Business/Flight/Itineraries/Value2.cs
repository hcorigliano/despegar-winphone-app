﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Neo.Business.Flight.Itineraries
{
    public class FacetValue
    {
        public string value { get; set; }
        public string label { get; set; }
        public int count { get; set; }
        public bool selected { get; set; }
    }
}