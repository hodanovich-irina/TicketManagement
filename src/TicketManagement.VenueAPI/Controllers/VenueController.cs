using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.VenueAPI.Dto;
using TicketManagement.VenueAPI.Interfaces;
using TicketManagement.VenueAPI.RoleData;

namespace TicketManagement.VenueAPI.Controllers
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
        /// Meyhod for get all venues.
        /// </summary>
        /// <returns>venues.</returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var venues = await _venueService.GetAllAsync();
            return Ok(venues);
        }

        /// <summary>
        /// Meyhod for get venue by id.
        /// </summary>
        /// <param name="id">venue id.</param>
        /// <returns>venue.</returns>
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var venueById = await _venueService.GetByIdAsync(id);
            return Ok(venueById);
        }

        /// <summary>
        /// Method for delete venue.
        /// </summary>
        /// <param name="id">id of venue to delete.</param>
        /// <returns>truthfulness of remove.</returns>
        [HttpDelete("Delete")]
        [Authorize(Roles = Role.Admin + ", " + Role.VenueManager)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var venueIsDelete = await _venueService.DeleteAsync(id);
                return Ok(venueIsDelete);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Method for add venue.
        /// </summary>
        /// <param name="venueDto">venue.</param>
        /// <returns>venue, that was added.</returns>
        [HttpPost("Add")]
        [Authorize(Roles = Role.Admin + ", " + Role.VenueManager)]
        public async Task<IActionResult> AddAsync([FromBody] VenueDto venueDto)
        {
            try
            {
                var venueToAdd = await _venueService.AddAsync(venueDto);
                return Ok(venueToAdd);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Method for add venue.
        /// </summary>
        /// <param name="venueDto">venue.</param>
        /// <returns>venue, that was edited.</returns>
        [HttpPut("Update")]
        [Authorize(Roles = Role.Admin + ", " + Role.VenueManager)]
        public async Task<IActionResult> EditAsync([FromBody] VenueDto venueDto)
        {
            try
            {
                var venueToEdit = await _venueService.EditAsync(venueDto);
                return Ok(venueToEdit);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
