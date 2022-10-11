using TicketManagement.BusinessLogic.Exceptions;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.BusinessLogic.Validations
{
    /// <summary>
    /// Validate properties for event area.
    /// </summary>
    internal class EventAreaValidation : IValidator<EventAreaDto>
    {
        /// <summary>
        /// Method for validity check object before add and edit.
        /// </summary>
        /// <param name="entity">Object of event area.</param>
        public void ValidationBeforeAddAndEdit(EventAreaDto entity)
        {
            IsValid(entity);
        }

        /// <summary>
        /// Method for validity check id before delete.
        /// </summary>
        /// <param name="id">Id of event area.</param>
        public void ValidateId(int id)
        {
            if (id < 1)
            {
                throw new ValidationException("Id must be more than zero");
            }
        }

        private void IsValid(EventAreaDto eventArea)
        {
            if (eventArea is null)
            {
                throw new ValidationException("Event area was null");
            }

            if (eventArea.EventId < 1)
            {
                throw new ValidationException("Id must be more than zero");
            }

            if (eventArea.CoordX < 0 || eventArea.CoordY < 0)
            {
                throw new ValidationException("Coordinates must be more than zero");
            }

            if (eventArea.Description is null || eventArea.Description.Length > 200)
            {
                throw new ValidationException("Description of area must be less than 200 and must be not null");
            }

            if (eventArea.Price < 0)
            {
                throw new ValidationException("Price must be more than zero");
            }
        }
    }
}