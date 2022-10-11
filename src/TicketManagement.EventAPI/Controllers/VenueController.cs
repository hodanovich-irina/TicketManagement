using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.EventAPI.Interfaces;

namespace TicketManagement.EventAPI.Controllers
{
    /// <summary>
    /// Controller for work with venues.
    /// </summary>
    [Route("[controller]")]
    public class VenueController : Controller
    {
        private readonly IVenueService _venueService;

        public VenueController(IVenueService venueService)
        {
            _venueService = venueService;
        }

        /// <summary>
        /// Method for get all venues.
        /// </summary>
        /// <returns>venues.</returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var venues = await _venueService.GetAllAsync();
            return Ok(venues);
        }
    }
}
