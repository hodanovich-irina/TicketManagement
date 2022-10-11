using System;

namespace ThirdPartyEventEditor.Exceptions
{
    /// <summary>
    /// Own exception class for validation.
    /// </summary>
    public class ValidationException : Exception
    {
        public ValidationException(string message)
        : base(message)
        {
        }
    }
}