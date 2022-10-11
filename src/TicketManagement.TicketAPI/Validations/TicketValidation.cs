using TicketManagement.TicketAPI.Dto;
using TicketManagement.TicketAPI.Exceptions;
using TicketManagement.TicketAPI.Interfaces;

namespace TicketManagement.TicketAPI.Validations
{
    /// <summary>
    /// Validate properties for ticket.
    /// </summary>
    internal class TicketValidation : IValidator<TicketDto>
    {
        /// <summary>
        /// Method for validity check id before delete.
        /// </summary>
        /// <param name="id">Id of ticket.</param>
        public void ValidateId(int id)
        {
            if (id < 1)
            {
                throw new ValidationException("Id must be more than zero");
            }
        }

        /// <summary>
        /// Method for validity check object before add and ticket.
        /// </summary>
        /// <param name="entity">Object of ticket.</param>
        public void ValidationBeforeAddAndEdit(TicketDto entity)
        {
            IsValid(entity);
        }

        private void IsValid(TicketDto ticket)
        {
            if (ticket is null)
            {
                throw new ValidationException("Ticket was null");
            }

            if (ticket.EventSeatId < 1)
            {
                throw new ValidationException("Id must be more than zero");
            }

            if (ticket.UserId is null)
            {
                throw new ValidationException("User was null");
            }

            if (ticket.Price < 0)
            {
                throw new ValidationException("Price must be more than zero");
            }
        }
    }
}
