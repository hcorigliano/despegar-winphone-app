using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Despegar.LegacyCore.Util;
using Despegar.LegacyCore.Connector;
using Despegar.LegacyCore.Repository;
using Despegar.LegacyCore.Connector.Domain.API;
using Despegar.LegacyCore.Service;

namespace Despegar.LegacyCore.Model
{
    public class ConfigurationModel
    {
        
        public ConfigurationModel()
        {
            Logger.Info("[model:configuration] Configuration Model created");
        }


        public async Task Sync()
        {
            if (ConfigurationRep.All == null)
                ConfigurationRep.All = await APIConfigurationService.GetAll();
            else
                Logger.Info("[model:configuration] configurations all already syncronized");
        }

        public async Task<Configurations> GetAll()
        {
            if (ConfigurationRep.All == null)
                ConfigurationRep.All = await APIConfigurationService.GetAll();

            else
                Logger.Info("[model:configuration] getting all from repository");
            
            return ConfigurationRep.All;
        }

        public Configuration Get(string channel)
        {
            Configuration selected = new Configuration();
            if (ConfigurationRep.All != null)
            {
                for (int i = 0; i < ConfigurationRep.All.configuration.Count; i++)
                {
                    if (ConfigurationRep.All.configuration.ElementAt(i).id == channel)
                    {
                        selected = ConfigurationRep.All.configuration.ElementAt(i);
                        break;
                    }
                }
            }

            return selected;
        }

        public Configuration GetCurrent()
        {
            return this.Get(ApplicationConfig.Instance.Country);
        }

        public string GetCurrentDomain()
        {
            Configuration current = this.GetCurrent();
            string domain = "";
            if (current != null) domain = current.domains.@base;
            return String.Format("http://{0}/", domain.Replace("www", "m"));
        }

        public string GetCurrentSecureDomain()
        {
            Configuration current = this.GetCurrent();
            string domain = "";
            if (current != null) domain = current.domains.@base;
            return String.Format("https://{0}/", domain.Replace("www", "secure"));
        }
    }
}
