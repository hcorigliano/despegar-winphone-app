﻿using Despegar.Core.Business;
using Despegar.Core.Business.Hotels.HotelsAutocomplete;
using Despegar.Core.Connector;
using Despegar.Core.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Service
{
    class HotelService : IHotelService
    {
        private CoreContext context;

        public HotelService(CoreContext context)
        {
            this.context = context;
        }

        public async Task<HotelsAutocomplete> GetHotelsAutocomplete(string hotelString)
        {
            string serviceUrl = ServiceURL.GetServiceURL(ServiceKey.HotelsAutocomplete);
            IConnector connector = context.GetServiceConnector(ServiceKey.HotelsAutocomplete);

            return await connector.GetAsync<HotelsAutocomplete>(serviceUrl);
        }
    }
}
