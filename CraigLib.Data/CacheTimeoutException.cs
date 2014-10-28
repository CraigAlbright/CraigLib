using System;

namespace CraigLib.Data
{
    public class CacheTimeoutException : Exception
    {
        private readonly string _message = "";

        public override string Message
        {
            get
            {
                return _message;
            }
        }

        public CacheTimeoutException()
        {
        }

        public CacheTimeoutException(string message)
        {
            _message = message;
        }
    }
}
