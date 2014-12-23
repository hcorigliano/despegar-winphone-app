using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Despegar.LegacyCore.Util;
using Despegar.LegacyCore.Model;
using System.Resources;
using System.ComponentModel;
using Despegar.LegacyCore.Connector.Domain.API;
using Despegar.LegacyCore.Service;

namespace Despegar.LegacyCore.ViewModel
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        public bool Geolocation { get; set; }
       

        public HomeViewModel()
        {
            Geolocation = ApplicationConfig.Instance.Location; // TODO: Is it Necesary?

            NotifyPropertyChanged("Geolocation");
        }


        public void NotifyPropertyChanged(string propertyName) { if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        public event PropertyChangedEventHandler PropertyChanged;

        public static string GetDomain(string siteCode)
        {
#if DECOLAR
            return "http://m.decolar.com/";
#else
            return "http://m.despegar.com." + siteCode.ToLowerInvariant() + "/";
#endif
        }
    }
}
