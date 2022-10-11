using TicketManagement.VenueAPI.Dto;
using TicketManagement.VenueAPI.Exceptions;
using TicketManagement.VenueAPI.Interfaces;

namespace TicketManagement.VenueAPI.Validations
{
    /// <summary>
    /// Validate properties for seat.
    /// </summary>
    internal class SeatValidation : IValidator<SeatDto>
    {
        /// <summary>
        /// Method for validity check object before add and edit.
        /// </summary>
        /// <param name="entity">Object of seat.</param>
        public void ValidationBeforeAddAndEdit(SeatDto entity)
        {
            IsValid(entity);
        }

        /// <summary>
        /// Method for validity check id before delete.
        /// </summary>
        /// <param name="id">Id of seat.</param>
        public void ValidateId(int id)
        {
            if (id < 1)
            {
                throw new ValidationException("Id must be more than zero");
            }
        }

        private void IsValid(SeatDto seat)
        {
            if (seat is null)
            {
                throw new ValidationException("Seat was null");
            }

            if (seat.AreaId < 1)
            {
                throw new ValidationException("Id must be more than zero");
            }

            if (seat.Number < 1 || seat.Row < 1)
            {
                throw new ValidationException("Number and row of seat must be more than zero");
            }
        }
    }
}
