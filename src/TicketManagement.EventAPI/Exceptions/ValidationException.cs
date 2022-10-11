using System;

namespace TicketManagement.EventAPI.Exceptions
{
    /// <summary>
    /// Own exception class for validation.
    /// </summary>
    internal class ValidationException : Exception
    {
        public ValidationException(string message)
        : base(message)
        {
        }
    }
}
