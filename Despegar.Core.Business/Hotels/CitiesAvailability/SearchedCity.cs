﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Hotels.CitiesAvailability
{
    public class SearchedCity
    {
        public string id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public IdCodeAndName country { get; set; }
    }
}
