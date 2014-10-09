﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Despegar.Core.Business.Configuration;
using Despegar.Core.Business;
using Despegar.Core.Connector;
using Despegar.Core.IService;

namespace Despegar.Core.Service
{
    class ConfigurationService : IConfigurationService
    {
        private CoreContext context;

        public ConfigurationService(CoreContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Retrives Configuration for all Countries.
        /// </summary>
        /// <returns></returns>
        public async Task<Configurations> GetConfigurations()
        {
            string serviceUrl = ServiceURL.GetServiceURL(ServiceKey.Configurations);
            IConnector connector = context.GetServiceConnector(ServiceKey.Configurations);

            return await connector.GetAsync<Configurations>(serviceUrl);
        }
    }
}
