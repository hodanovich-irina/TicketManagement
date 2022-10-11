using System;

namespace TicketManagement.TicketAPI.Exceptions
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
