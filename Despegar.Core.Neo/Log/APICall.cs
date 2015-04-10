using System;

namespace Despegar.Core.Neo.Log
{
    public class APICall
    {
        public DateTime Time { get; set; }
        public string ServiceKey { get; set; }
        public string URL { get; set; }
        public string Exception { get; set; }
        public string Response { get; set; }
        public string Headers { get; set; }
        public string Payload { get; set; }
    }
}