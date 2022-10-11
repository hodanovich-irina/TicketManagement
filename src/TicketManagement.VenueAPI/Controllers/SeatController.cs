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
    /// Controller for work with seats.
    /// </summary>
    [Route("[controller]")]
    public class SeatController : Controller
    {
        private readonly IService<SeatDto> _seatService;

        public SeatController(IService<SeatDto> seatService)
        {
            _seatService = seatService;
        }

        /// <summary>
        /// Method for get all seats.
        /// </summary>
        /// <returns>seats.</returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var seats = await _seatService.GetAllAsync();
            return Ok(seats);
        }

        /// <summary>
        /// Method for get seat by id.
        /// </summary>
        /// <param name="id">seat id.</param>
        /// <returns>seat, that was added.</returns>
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var seatById = await _seatService.GetByIdAsync(id);
            return Ok(seatById);
        }

        /// <summary>
        /// Meyhod for get seat by parent id.
        /// </summary>
        /// <param name="id">parent id.</param>
        /// <returns>seat.</returns>
        [HttpGet("GetByParenId")]
        public async Task<IActionResult> GetByParentId(int id)
        {
            var seatById = await _seatService.GetAsync(id);
            return Ok(seatById);
        }

        /// <summary>
        /// Method for delete seat.
        /// </summary>
        /// <param name="id">seat id.</param>
        /// <returns>truthfulness of remove.</returns>
        [HttpDelete("Delete")]
        [Authorize(Roles = Role.Admin + ", " + Role.VenueManager)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var seatIsDelete = await _seatService.DeleteAsync(id);
                return Ok(seatIsDelete);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Method for add seat.
        /// </summary>
        /// <param name="seatDto">seat.</param>
        /// <returns>seat, that was added.</returns>
        [HttpPost("Add")]
        [Authorize(Roles = Role.Admin + ", " + Role.VenueManager)]
        public async Task<IActionResult> AddAsync([FromBody] SeatDto seatDto)
        {
            try
            {
                var seatToAdd = await _seatService.AddAsync(seatDto);
                return Ok(seatToAdd);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Method for update seat.
        /// </summary>
        /// <param name="seatDto">seat.</param>
        /// <returns>seat, that was edited.</returns>
        [HttpPut("Update")]
        [Authorize(Roles = Role.Admin + ", " + Role.VenueManager)]
        public async Task<IActionResult> EditAsync([FromBody] SeatDto seatDto)
        {
            try
            {
                var seatToEdit = await _seatService.EditAsync(seatDto);
                return Ok(seatToEdit);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
