using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.EventAPI.Dto;
using TicketManagement.EventAPI.Interfaces;
using TicketManagement.EventAPI.RoleData;

namespace TicketManagement.EventAPI.Controllers
{
    /// <summary>
    /// Controller for work with event seats.
    /// </summary>
    [Route("[controller]")]
    public class EventSeatController : Controller
    {
        private readonly IService<EventSeatDto> _eventSeatService;

        public EventSeatController(IService<EventSeatDto> eventSeatService)
        {
            _eventSeatService = eventSeatService;
        }

        /// <summary>
        /// Method for get all event seats.
        /// </summary>
        /// <returns>event seats.</returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var eventSeats = await _eventSeatService.GetAllAsync();
            return Ok(eventSeats);
        }

        /// <summary>
        /// Method for get by id.
        /// </summary>
        /// <param name="id">event seat id.</param>
        /// <returns>event seat.</returns>
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var eventSeatById = await _eventSeatService.GetByIdAsync(id);
            return Ok(eventSeatById);
        }

        /// <summary>
        /// Method for get by parent id.
        /// </summary>
        /// <param name="id">parent id.</param>
        /// <returns>event seat.</returns>
        [HttpGet("GetByParenId")]
        public async Task<IActionResult> GetByParentId(int id)
        {
            var eventSeatByParentId = await _eventSeatService.GetAsync(id);
            return Ok(eventSeatByParentId);
        }

        /// <summary>
        /// Method for delete event seat.
        /// </summary>
        /// <param name="id">event seat id.</param>
        /// <returns>truthfulness of remove.</returns>
        [HttpDelete("Delete")]
        [Authorize(Roles = Role.User + ", " + Role.Admin + ", " + Role.EventManager)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var eventSeatIsDelete = await _eventSeatService.DeleteAsync(id);
                return Ok(eventSeatIsDelete);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Method for add event seat.
        /// </summary>
        /// <param name="eventSeatDto">event seat.</param>
        /// <returns> event seat, that was added.</returns>
        [HttpPost("Add")]
        [Authorize(Roles = Role.User + ", " + Role.Admin + ", " + Role.EventManager)]
        public async Task<IActionResult> AddAsync(EventSeatDto eventSeatDto)
        {
            try
            {
                var eventSeatToAdd = await _eventSeatService.AddAsync(eventSeatDto);
                return Ok(eventSeatToAdd);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Method for update event seat.
        /// </summary>
        /// <param name="eventSeatDto"> event seat.</param>
        /// <returns> event seat that was edit.</returns>
        [HttpPut("Update")]
        [Authorize(Roles = Role.User + ", " + Role.Admin + ", " + Role.EventManager)]
        public async Task<IActionResult> EditAsync(EventSeatDto eventSeatDto)
        {
            try
            {
                var eventSeatToEdit = await _eventSeatService.EditAsync(eventSeatDto);
                return Ok(eventSeatToEdit);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
