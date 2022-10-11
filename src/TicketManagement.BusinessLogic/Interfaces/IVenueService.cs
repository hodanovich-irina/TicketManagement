using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.BusinessLogic.Interfaces
{
    /// <summary>
    /// Interface for work with venue service.
    /// </summary>
    public interface IVenueService
    {
        /// <summary>
        /// Logic for add object.
        /// </summary>
        /// <param name="entity">Object of class.</param>
        Task<VenueDto> AddAsync(VenueDto entity);

        /// <summary>
        /// Logic for edit object.
        /// </summary>
        /// <param name="entity">Object of class.</param>
        Task<bool> EditAsync(VenueDto entity);

        /// <summary>
        /// Logic for delete object.
        /// </summary>
        /// <param name="id">Id of object.</param>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Logic for get object by id.
        /// </summary>
        /// <param name="id">Id of object.</param>
        Task<VenueDto> GetByIdAsync(int id);

        /// <summary>
        /// Logic for get all object from Venue.
        /// </summary>
        /// <returns>Collection of venue object.</returns>
        Task<IEnumerable<VenueDto>> GetAllAsync();
    }
}
