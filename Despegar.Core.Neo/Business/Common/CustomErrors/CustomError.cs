using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Business.CustomErrors
{
    public class CustomError
    {
        public string Message { get; set; }
        public string Code { get; set; }
        public string Method { get; set; }
        public bool HasDates { get; set; }
        public string Date { get; set; }

        public CustomError(string message)
        {
            this.Message = message;
        }

        public CustomError(string message, string code)
        {
            this.Message = message;
            this.Code = code;
        }

        public CustomError(string message, string code, string method)
        {
            this.Message = message;
            this.Code = code;
            this.Method = method;
        }

        public CustomError(string message, string code, string method, bool hasDates, string date)
        {
            this.Message = message;
            this.Code = code;
            this.Method = method;
            this.HasDates = hasDates;
            this.Date = date;
        }
    }
}
