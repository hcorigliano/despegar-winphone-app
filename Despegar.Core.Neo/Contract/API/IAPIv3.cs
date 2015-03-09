﻿using Despegar.Core.Neo.Business.Hotels.UserReviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Contract.API
{
    public interface IAPIv3
    {
        Task<HotelUserReviews> GetHotelUserReviews(string hotelId, int limit, int offset, string language, string provider);
        
    }
}