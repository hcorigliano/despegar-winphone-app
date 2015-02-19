﻿using Despegar.Core.Neo.API;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Connector
{
    public interface IConnector
    {
        Task<T> GetAsync<T>(string url, ServiceKey key) where T:class;
        Task<T> PostAsync<T>(string url, object data, ServiceKey key) where T : class;
    }
}