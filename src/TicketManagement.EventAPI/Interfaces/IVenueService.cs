using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.EventAPI.Dto;

namespace TicketManagement.EventAPI.Interfaces
{
    /// <summary>
    /// Interface for work with venue service.
    /// </summary>
    public interface IVenueService
    {
        /// <summary>
        /// Logic for get all object from Venue.
        /// </summary>
        /// <returns>Collection of venue object.</returns>
        Task<IEnumerable<VenueDto>> GetAllAsync();
    }
}
