using System;

namespace CraigLib.Data
{
    internal class DatabaseAdapterException : ApplicationException
    {
        private readonly string _errorMessage;
        private readonly string _commandText;

        public string ErrorMessage
        {
            get { return _errorMessage; }
        }

        public string CommandText
        {
            get { return _commandText; }
        }

        public DatabaseAdapterException(string errMsg)
            : base(errMsg)
        {
            _errorMessage = errMsg;
            _commandText = string.Empty;
        }

        public DatabaseAdapterException(string errMsg, string cmdText)
            : base(errMsg)
        {
            _errorMessage = errMsg;
            _commandText = cmdText;
        }

        public DatabaseAdapterException(string errMsg, Exception inner)
            : base(errMsg, inner)
        {
            _errorMessage = errMsg;
            _commandText = string.Empty;
        }

        public DatabaseAdapterException(string errMsg, string cmdText, Exception inner)
            : base(errMsg + "\r\nSQL:" + cmdText, inner)
        {
            _errorMessage = errMsg;
            _commandText = cmdText;
        }
    }
}
