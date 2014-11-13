using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model
{
    public abstract class BusinessModelBase : INotifyPropertyChanged 
    {
        public BusinessModelBase()
        {
            InitializeModel();
        }

        public virtual void Validate() { }
        public virtual void InitializeModel() { }                           

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