using TicketManagement.BusinessLogic.Exceptions;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.BusinessLogic.Validations
{
    /// <summary>
    /// Validate properties for event.
    /// </summary>
    internal class EventValidation : IValidator<EventDto>
    {
        /// <summary>
        /// Method for validity check object before add and edit.
        /// </summary>
        /// <param name="entity">Object of event.</param>
        public void ValidationBeforeAddAndEdit(EventDto entity)
        {
            IsValid(entity);
        }

        /// <summary>
        /// Method for validity check id before delete.
        /// </summary>
        /// <param name="id">Id of event.</param>
        public void ValidateId(int id)
        {
            if (id < 1)
            {
                throw new ValidationException("Id must be more than zero");
            }
        }

        private void IsValid(EventDto eventForWork)
        {
            if (eventForWork is null)
            {
                throw new ValidationException("Event was null");
            }

            if (eventForWork.LayoutId < 1)
            {
                throw new ValidationException("Id must be more than zero");
            }

            if (eventForWork.Name is null || eventForWork.Name.Length > 120)
            {
                throw new ValidationException("Name of event must be less than 120 sumbols and must be not null");
            }

            if (eventForWork.Description is null)
            {
                throw new ValidationException("Description of event must be not null");
            }

            if (eventForWork.ImageURL is null)
            {
                throw new ValidationException("Image url of event must be not null");
            }

            if (eventForWork.ShowTime.Minutes < 0)
            {
                throw new ValidationException("Show time of event must be more than zero");
            }

            if (eventForWork.BaseAreaPrice < 0)
            {
                throw new ValidationException("Price must be more than zero");
            }
        }
    }
}
