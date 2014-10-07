using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Despegar.LegacyCore.Util;
using Despegar.LegacyCore.Model;
using Despegar.LegacyCore.Resource;
using System.Resources;
using System.ComponentModel;
using Despegar.LegacyCore.Connector.Domain.API;
using Despegar.LegacyCore.Service;

namespace Despegar.LegacyCore.ViewModel
{
    public class HomeViewModel : INotifyPropertyChanged
    {

        public ConfigurationModel Configuration { get; set; }

        public bool Geolocation { get; set; }
        public string Domain { get; set; }

        public HomeViewModel()
        {
            //Configuration = new ConfigurationModel();            
            Domain = "http://m.despegar.com.ar/";
            Geolocation = ApplicationConfig.Instance.Location; // TODO: Is it Necesary?

            NotifyPropertyChanged("Geolocation");
        }


        public void NotifyPropertyChanged(string propertyName) { if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
