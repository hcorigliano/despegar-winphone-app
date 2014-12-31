using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Despegar.WP.UI.Controls
{
    public static class SafeMessageDialog
    {
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1);        

        public static async Task<IUICommand> ShowSafelyAsync(this MessageDialog dialog)
        {
            await _semaphore.WaitAsync();
            var result = await dialog.ShowAsync();
            _semaphore.Release();
            return result;
        }
    }
}