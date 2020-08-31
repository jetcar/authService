using System;

namespace CommonTools.services
{
    public class ControllerException : Exception
    {
        private readonly ErrorCodes _errorCodes;
        private readonly string _field;

        public ControllerException(string field, ErrorCodes errorCodes)
        {
            _errorCodes = errorCodes;
            _field = field;
        }
    }
}