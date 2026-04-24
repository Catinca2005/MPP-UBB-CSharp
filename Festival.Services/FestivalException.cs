using System;

namespace Festival.Services
{
    /// <summary>
    /// Custom exception class for business logic and network-related errors within the Festival domain.
    /// </summary>
    public class FestivalException : Exception
    {
        public FestivalException() : base() { }

        public FestivalException(string message) : base(message) { }

        public FestivalException(string message, Exception innerException) : base(message, innerException) { }
    }
}