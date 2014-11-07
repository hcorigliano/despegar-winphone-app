using Despegar.Core.Business.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.SampleData
{
    public class CountrySelectionSample
    {
        public List<Site> sites;

        public CountrySelectionSample()
        {
           this.sites = new List<Site>() { new Site() { name="Argentina", code="AR"}, new Site() { name="Colombia", code="CO"}, new Site() { name="Venezuela", code="VZ"}  } ;
        }
    }
}
