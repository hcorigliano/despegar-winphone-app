using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Connector
{
    public interface IConnector
    {
        Task<T> GetAsync<T>(string url) where T:class;
        Task<T> PostAsync<T>(string url, object data) where T : class;
    }
}