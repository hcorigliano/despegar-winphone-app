﻿using Despegar.Core.Neo.Business.Coupons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.IService
{
    public interface ICouponsService
    {
        Task<CouponResponse> Validity(CouponParameter parameter);
    }
}
