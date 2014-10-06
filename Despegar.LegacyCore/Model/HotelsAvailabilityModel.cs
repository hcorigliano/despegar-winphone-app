using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Despegar.LegacyCore.Util;
using Despegar.LegacyCore.Service;
using Despegar.LegacyCore.Connector.Domain.API;

namespace Despegar.LegacyCore.Model
{
    public class HotelsAvailabilityModel
    {

        public HotelsAvailabilityModel()
        {
            Logger.Info("[model:hotels:detail] Hotels Availability model created");
        }


        public void SetParamsByUrl(Uri uri)
        {
            int len = uri.LocalPath.Length;
            int idx = uri.LocalPath.IndexOf("checkout/") + 9;
            if (uri.LocalPath.Contains("s1/")) idx = idx + 3;
            string param = uri.LocalPath.Substring(idx, len - idx);
            string[] opts = param.Split('/');

            Hotel   = opts[0];
            Checkin = opts[1];
            Checkout = opts[2];
            Distribution = new HotelsDistributionModel(opts[3]);

            if (uri.Query.Contains("room="))
                Room = uri.Query.Substring(uri.Query.IndexOf("room=") + 5);

            ApplicationConfig.Instance.BrowsingPages.Pop();
        }

        public async Task<HotelAvailability> GetAvailability()
        {
            HotelAvailability avai = await APIHotelsService.Availability(Hotel, Checkin, Checkout, Distribution.ToString());
            MiscCurrency Curr = await CurrenciesModel.GetById(avai.meta.currencyCode);
            if (Curr != null) Currency = Curr.symbol;
            else Currency = "";
            return avai;
        }

        public string Hotel { set; get; }

        public string Checkin { get; set; }
        public DateTime checkin { get { return DateTime.Parse(Checkin); } }
        public string TheCheckin { get { return checkin.ToString("d-MMM"); } }

        public string Checkout { get; set; }
        public DateTime checkout { get { return DateTime.Parse(Checkout); } }

        public HotelsDistributionModel Distribution { get; set; }

        public string Currency { get; set; }

        public string Room { get; set; }

        public int Nights { get { return Convert.ToInt32((checkout - checkin).TotalDays); } }
        public int Rooms { get { return Distribution.Count; } }
        public int Hostages
        { 
            get 
            {
                int i = 0;

                foreach (var it in Distribution)                
                    i += int.Parse(it.Adults) + it.Children.Count;                

                return i;
            } 
        }

    }
}
