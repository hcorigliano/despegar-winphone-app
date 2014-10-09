using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Despegar.LegacyCore.Util;
using Despegar.LegacyCore.Model;

namespace Despegar.LegacyCore.ViewModel
{
    public class SplashViewModel
    {
        public SplashViewModel()
        {
            Logger.Info("[vm:splash] Splash ViewModel initialized");

            channels = new ChannelsModel();
            channels.Add(new ChannelModel() { id = null, description = "Seleccione País" });
            channels.Add(new ChannelModel() { id = "AR", description = "Argentina" });
            channels.Add(new ChannelModel() { id = "BO", description = "Bolivia" });
            channels.Add(new ChannelModel() { id = "CL", description = "Chile" });
            channels.Add(new ChannelModel() { id = "CO", description = "Colombia" });
            channels.Add(new ChannelModel() { id = "CR", description = "Costa Rica" });
            channels.Add(new ChannelModel() { id = "EC", description = "Ecuador" });
            channels.Add(new ChannelModel() { id = "SV", description = "El Salvador" });
            channels.Add(new ChannelModel() { id = "ES", description = "España" });
            channels.Add(new ChannelModel() { id = "US", description = "Estados Unidos" });
            channels.Add(new ChannelModel() { id = "GT", description = "Guatemala" });
            channels.Add(new ChannelModel() { id = "HN", description = "Honduras" });
            channels.Add(new ChannelModel() { id = "MX", description = "México" });
            channels.Add(new ChannelModel() { id = "NI", description = "Nicaragua" });
            channels.Add(new ChannelModel() { id = "PA", description = "Panama" });
            channels.Add(new ChannelModel() { id = "PY", description = "Paraguay" });
            channels.Add(new ChannelModel() { id = "PE", description = "Perú" });
            channels.Add(new ChannelModel() { id = "PR", description = "Puerto Rico" });
            channels.Add(new ChannelModel() { id = "DO", description = "República Dominicana" });
            channels.Add(new ChannelModel() { id = "UY", description = "Uruguay" });
            channels.Add(new ChannelModel() { id = "VE", description = "Venezuela" });
            selectedChannel = channels[0];
            
            Configuration     = new ConfigurationModel();
            Countries         = new CountriesModel();
            Currencies        = new CurrenciesModel();
            PushNotifications = new DPNSModel();
        }

        public async Task Init()
        {
            //await this.SplashModel.RemoteConfiguration();
            await this.Configuration.Sync();
        }

        public async Task Resume()
        {
            await this.Countries.Sync();
            await this.Currencies.Sync();
        }


        public async Task SyncWithDPNS()
        {
            await PushNotifications.Register();
        }


        private ConfigurationModel Configuration { get; set; }
        private CountriesModel Countries { get; set; }
        private CurrenciesModel Currencies { get; set; }
        private DPNSModel PushNotifications { get; set; }
        
        public ChannelsModel channels { get; set; }
        public ChannelModel selectedChannel { get; set; }
    }
}
