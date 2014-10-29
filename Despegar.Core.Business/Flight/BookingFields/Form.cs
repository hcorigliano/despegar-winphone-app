﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingFields
{
    public class Form
    {
        public List<Passenger> passengers { get; set; }
        public Payment payment { get; set; }
        public Contact contact { get; set; }
        public Comment comment { get; set; }
        public List<Voucher> vouchers { get; set; }
        public List<object> messages { get; set; }
        public List<object> destination_services { get; set; }
        public FieldDataType coupon_beneficiary_id_type { get; set; }
    }
}