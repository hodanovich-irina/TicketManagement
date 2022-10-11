using TicketManagement.VenueAPI.Dto;
using TicketManagement.VenueAPI.Exceptions;
using TicketManagement.VenueAPI.Interfaces;

namespace TicketManagement.VenueAPI.Validations
{
    /// <summary>
    /// Validate properties for area.
    /// </summary>
    internal class AreaValidation : IValidator<AreaDto>
    {
        /// <summary>
        /// Method for validity check object before add and edit.
        /// </summary>
        /// <param name="entity">Object of area.</param>
        public void ValidationBeforeAddAndEdit(AreaDto entity)
        {
            IsValid(entity);
        }

        /// <summary>
        /// Method for validity check id before delete.
        /// </summary>
        /// <param name="id">Id of area.</param>
        public void ValidateId(int id)
        {
            if (id < 1)
            {
                 throw new ValidationException("Id must be more than zero");
            }
        }

        private void IsValid(AreaDto area)
        {
            if (area is null)
            {
                throw new ValidationException("Area was null");
            }

            if (area.LayoutId < 1)
            {
                throw new ValidationException("Id must be more than zero");
            }

            if (area.CoordX < 0 || area.CoordY < 0)
            {
                throw new ValidationException("Coordinates must be more than zero");
            }

            if (area.Description is null || area.Description.Length > 200 )
            {
                throw new ValidationException("Description of area must be less than 200 and must be not null");
            }
        }
    }
}