using System;

namespace Despegar.Core.Exceptions
{
    /// <summary>
    /// Represents an error when a JSON string could not be deserialized to a specific Type
    /// </summary>
    public class JsonDeserializationException : Exception
    {
        public JsonDeserializationException()
        {
        }

        public JsonDeserializationException(string message)
            : base(message)
        {
        }

        public JsonDeserializationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}