using Despegar.LegacyCore.Service;
using Despegar.LegacyCore.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Despegar.LegacyCore.Model
{
    public class DPNSModel
    {

        public DPNSModel()
        {
            Logger.Info("[model:dpns] Despegar Push Notifications Model created");
        }

        
        public async Task Register() {

            DPNSRegisterModel PushData = new DPNSRegisterModel();

            if (string.IsNullOrEmpty(PushData.upaId))
                await PushData.SyncUpaId();

            string data = JsonConvert.SerializeObject(PushData);
            await DPushNotificationService.Register(data);
        }


        public async Task RegisterBooking(string productType, string idCro)
        {
            DPNSRegisterBookingModel PushData = new DPNSRegisterBookingModel() { productType = productType, idCro = idCro };

            if (string.IsNullOrEmpty(PushData.upaId))
                await PushData.SyncUpaId();

            string data = JsonConvert.SerializeObject(PushData);
            await DPushNotificationService.RegisterBooking(data);
        }
    }



    public class DPNSRegisterModel
    {
        public string upaId { get { return ApplicationConfig.Instance.UpaId; } }
        public string token { get { return ApplicationConfig.Instance.PushChannel; } }
        public string brand { get { return ApplicationConfig.Instance.Brand.ToLower(); } }
        public string countryId { get { return ApplicationConfig.Instance.Country; } }
        public string deviceType { get { return "wphone"; } }


        public async Task SyncUpaId()
        {
            ApplicationConfig.Instance.UpaId = await UPAService.Get();
        } 
    }


    public class DPNSRegisterBookingModel
    {
        public string upaId { get { return ApplicationConfig.Instance.UpaId; } }
        public string productType { get; set; }
        public string idCro { get; set; }


        public async Task SyncUpaId()
        {
            ApplicationConfig.Instance.UpaId = await UPAService.Get();
        }
    }
}
