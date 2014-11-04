using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model.Classes.Flights
{
    public class PassagersQuantity : BindableBase
    {
        private int _adultPassagerQuantity;
        public int AdultPassagerQuantity
        {
            get { return _adultPassagerQuantity; }
            set {
                if (_childPassagerQuantity + value > 8)
                    throw new InvalidOperationException("Max Passengers exceeded!");

                if (_adultPassagerQuantity != value)
                {
                    _adultPassagerQuantity = value;
                     OnPropertyChanged("AdultOptions");
                     OnPropertyChanged("ChildrenOptions");
                     OnPropertyChanged("AdultPassagerQuantity");
                     OnPropertyChanged("ChildPassagerQuantity");
                 }
            } 
        }

        private int _childPassagerQuantity;
        public int ChildPassagerQuantity
        {
            get { return _childPassagerQuantity; }
            set
            {
                if (_adultPassagerQuantity + value > 8)
                    throw new InvalidOperationException("Max Passengers exceeded!");

                if (_childPassagerQuantity != value)
                {
                    _childPassagerQuantity = value;
                    OnPropertyChanged("AdultOptions");
                    OnPropertyChanged("ChildrenOptions");
                    OnPropertyChanged("ChildPassagerQuantity");
                    OnPropertyChanged("AdultPassagerQuantity");
                }
            }
        }

        /// <summary>
        /// Returns the available options for Adults passengers
        /// </summary>
        public IEnumerable<int> AdultOptions
        {
            get { 
             List<int> options =  new List<int>();

                // 1 is the Minimum Adult count
             for(int i = 1; i <= 8 - ChildPassagerQuantity; i++)
                options.Add(i);

             return options;
            }
        }

        /// <summary>
        /// Returns the available options for Children passengers
        /// </summary>
        public IEnumerable<int> ChildrenOptions
        {
            get
            {
                List<int> options = new List<int>();

                // 0 is the Minimum Child count, and 1 adult is always present
                for (int i = 0; i <= 8 - AdultPassagerQuantity; i++)
                    options.Add(i);

                return options;
            }
        }
    }
}

