using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;
using TicketManagement.EventAPI.Automapper;
using TicketManagement.EventAPI.Dto;
using TicketManagement.EventAPI.Interfaces;

namespace TicketManagement.EventAPI.Services
{
    /// <summary>
    /// Providing methods for managing venue with validation and business logic.
    /// </summary>
    internal class VenueService : AutoMapperService,  IVenueService
    {
        private readonly IEFRepository<Venue> _venueEFRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="VenueService"/> class.
        /// </summary>
        /// <param name="venueEFRepository">Object of venue ef repository.</param>
        public VenueService(IEFRepository<Venue> venueEFRepository)
        {
            _venueEFRepository = venueEFRepository;
        }

        /// <summary>
        /// Logic for get all venue.
        /// </summary>
        /// <returns>Collection of venue object.</returns>
        public async Task<IEnumerable<VenueDto>> GetAllAsync()
        {
            var venues = await _venueEFRepository.GetAllAsync();
            return venues.Select(venue => Mapper.Map<VenueDto>(venue)).AsEnumerable();
        }
    }
}
