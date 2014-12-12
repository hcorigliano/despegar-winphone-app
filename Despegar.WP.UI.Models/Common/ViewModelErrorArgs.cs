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
        public object Parameter { get; set; }

        public ViewModelErrorArgs(string errorCode)
        {
            this.ErrorCode = errorCode;
        }

        public ViewModelErrorArgs(string errorCode, object parameter)
        {
            this.ErrorCode = errorCode;
            this.Parameter = parameter;
        }
    }
}