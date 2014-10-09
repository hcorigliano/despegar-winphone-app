using System;

namespace Despegar.Core.Exceptions
{
    /// <summary>
    /// Represents an error when a JSON string could not be deserialized to a specific Type
    /// </summary>
    public class JsonSerializerException : Exception
    {
        public JsonSerializerException()
        {
        }

        public JsonSerializerException(string message)
            : base(message)
        {
        }

        public JsonSerializerException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}