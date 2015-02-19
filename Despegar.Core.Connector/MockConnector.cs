using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Connector
{
    public class MockConnector : IConnector
    {
        private string response;

        public MockConnector(string response) {
            this.response = response;
        }

        public Task<T> GetAsync<T>(string url) where T : class
        {            
           return Task.FromResult<T>(JsonConvert.DeserializeObject<T>(response));
        }

        public Task<T> PostAsync<T>(string url, object data) where T : class
        {
            return Task.FromResult<T>(JsonConvert.DeserializeObject<T>(response));
        }
    }
}