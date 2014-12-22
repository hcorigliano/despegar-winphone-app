using Despegar.WP.UI.Model.Classes.Flights;
using Despegar.WP.UI.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model.ViewModel.Classes.Results
{
    public class BindableItemsLoadingCollection : IIncrementalSource<BindableItem>
    {
        public List<BindableItem> BindableItems { get; set; }

        public async Task<IEnumerable<BindableItem>> GetPagedItems(int pageIndex, int pageSize)
        {
            //TODO this method should be applied on model
            return await Task.Run<IEnumerable<BindableItem>>(() =>
            {
                var result = (from p in BindableItems
                              select p).Skip(pageIndex * pageSize).Take(pageSize);

                return result;
            });
        }
    }
}
