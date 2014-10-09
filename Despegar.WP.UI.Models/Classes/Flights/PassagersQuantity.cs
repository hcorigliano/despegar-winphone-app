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
        private int _AdultPassagerQuantity;
        public int AdultPassagerQuantity
        {
            get { return _AdultPassagerQuantity; }
            set { SetProperty(ref _AdultPassagerQuantity, value); }
        }

        private int _ChildPassagerQuantity;
        public int ChildPassagerQuantity
        {
            get { return _ChildPassagerQuantity; }
            set { SetProperty(ref _ChildPassagerQuantity, value); }
        }
        
        public bool CheckMaxPassagers()
        {
            if (ChildPassagerQuantity + AdultPassagerQuantity >= 8)
            {
                return false;
            }
            else return true;
        }

        public void AddAdult()
        {
            if (CheckMaxPassagers())
            {
                AdultPassagerQuantity += 1;
            }
        }

        public void AddChild()
        {
            if (CheckMaxPassagers())
            {
                ChildPassagerQuantity += 1;
            }
        }

        public void SubAdult()
        {
            if (AdultPassagerQuantity >= 2)
            {
                AdultPassagerQuantity -= 1;
            }
        }

        public void subChild()
        {
            if (ChildPassagerQuantity >= 1)
            {
                ChildPassagerQuantity -= 1;
            }
        }
    }
}
