using TicketManagement.BusinessLogic.Exceptions;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.BusinessLogic.Validations
{
    /// <summary>
    /// Validate properties for layout.
    /// </summary>
    internal class LayoutValidation : IValidator<LayoutDto>
    {
        /// <summary>
        /// Method for validity check object before add and edit.
        /// </summary>
        /// <param name="entity">Object of layout.</param>
        public void ValidationBeforeAddAndEdit(LayoutDto entity)
        {
            IsValid(entity);
        }

        /// <summary>
        /// Method for validity check id before delete.
        /// </summary>
        /// <param name="id">Id of layout.</param>
        public void ValidateId(int id)
        {
            if (id < 1)
            {
                throw new ValidationException("Id must be more than zero");
            }
        }

        private void IsValid(LayoutDto layout)
        {
            if (layout is null)
            {
                throw new ValidationException("Layout was null");
            }

            if (layout.VenueId < 1)
            {
                throw new ValidationException("Id must be more than zero");
            }

            if (layout.Name is null || layout.Name.Length > 120)
            {
                throw new ValidationException("Name of layout must be less than 120 sumbols and must be not null");
            }

            if (layout.Description is null || layout.Description.Length > 120)
            {
                throw new ValidationException("Description of layout must be less than 120 sumbols and must be not null");
            }
        }
    }
}
