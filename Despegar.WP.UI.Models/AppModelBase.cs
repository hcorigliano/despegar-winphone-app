using Despegar.WP.UI.Model.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model
{
    public abstract class AppModelBase: INotifyPropertyChanged 
    {
        string appName;

        internal AppModelBase()
        {
            InitializeModel();
        }

        internal void Validate()
        {
            throw new NotImplementedException();
        }

        internal void InitializeModel()
        {
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
