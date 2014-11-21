using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model.Common
{
    public class ViewModelErrorArgs : EventArgs
    {
        public string ErrorCode { get; set; }

        public ViewModelErrorArgs(string errorCode)
        {
            this.ErrorCode = errorCode;
        }
    }
}