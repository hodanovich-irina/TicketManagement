using TicketManagement.BusinessLogic.Exceptions;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.BusinessLogic.Validations
{
    /// <summary>
    /// Validate properties for event seat.
    /// </summary>
    internal class EventSeatValidation : IValidator<EventSeatDto>
    {
        /// <summary>
        /// Method for validity check object before add and edit.
        /// </summary>
        /// <param name="entity">Object of event seat.</param>
        public void ValidationBeforeAddAndEdit(EventSeatDto entity)
        {
            IsValid(entity);
        }

        /// <summary>
        /// Method for validity check id before delete.
        /// </summary>
        /// <param name="id">Id of event seat.</param>
        public void ValidateId(int id)
        {
            if (id < 1)
            {
                throw new ValidationException("Id must be more than zero");
            }
        }

        private void IsValid(EventSeatDto eventSeat)
        {
            if (eventSeat is null)
            {
                throw new ValidationException("Event seat was null");
            }

            if (eventSeat.EventAreaId < 1)
            {
                throw new ValidationException("Id must be more than zero");
            }

            if (eventSeat.Number < 1 || eventSeat.Row < 1)
            {
                throw new ValidationException("Number and row of seat must be more than zero");
            }
        }
    }
}
