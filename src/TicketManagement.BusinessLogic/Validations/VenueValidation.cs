using TicketManagement.BusinessLogic.Exceptions;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.BusinessLogic.Validations
{
    /// <summary>
    /// Validate properties for venue.
    /// </summary>
    internal class VenueValidation : IValidator<VenueDto>
    {
        /// <summary>
        /// Method for validity check object before add and edit.
        /// </summary>
        /// <param name="entity">Object of venue.</param>
        public void ValidationBeforeAddAndEdit(VenueDto entity)
        {
            IsValid(entity);
        }

        /// <summary>
        /// Method for validity check id before delete.
        /// </summary>
        /// <param name="id">Id of venue.</param>
        public void ValidateId(int id)
        {
            if (id < 1)
            {
                throw new ValidationException("Id must be more than zero");
            }
        }

        private void IsValid(VenueDto venue)
        {
            if (venue is null)
            {
                throw new ValidationException("Venue was null");
            }

            if (venue.Description is null || venue.Description.Length > 120)
            {
                throw new ValidationException("Description of venue must be less than 120 and must be not null");
            }

            if (venue.Name is null || venue.Name.Length > 120)
            {
                throw new ValidationException("Name of venue must be less than 120 and must be not null");
            }

            if (venue.Address is null || venue.Address.Length > 200)
            {
                throw new ValidationException("Address of venue must be less than 200 and must be not null");
            }

            if (venue.Phone.Length > 30)
            {
                throw new ValidationException("Phone of venue must be less than 30");
            }
        }
    }
}
