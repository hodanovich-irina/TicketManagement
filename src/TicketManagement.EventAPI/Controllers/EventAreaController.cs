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
    /// Controller for work with event areas.
    /// </summary>
    [Route("[controller]")]
    public class EventAreaController : Controller
    {
        private readonly IService<EventAreaDto> _eventAreaService;

        public EventAreaController(IService<EventAreaDto> eventAreaService)
        {
            _eventAreaService = eventAreaService;
        }

        /// <summary>
        /// Method for get all event areas.
        /// </summary>
        /// <returns>event areas.</returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var eventAreas = await _eventAreaService.GetAllAsync();
            return Ok(eventAreas);
        }

        /// <summary>
        /// Method for get event area by id.
        /// </summary>
        /// <param name="id">event area id.</param>
        /// <returns>event areas.</returns>
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var eventAreaById = await _eventAreaService.GetByIdAsync(id);
            return Ok(eventAreaById);
        }

        /// <summary>
        /// Method for get event area by parent id.
        /// </summary>
        /// <param name="id">event area parent id.</param>
        /// <returns>event area.</returns>
        [HttpGet("GetByParenId")]
        public async Task<IActionResult> GetByParentId(int id)
        {
            var eventAreaById = await _eventAreaService.GetAsync(id);
            return Ok(eventAreaById);
        }

        /// <summary>
        /// Method for delete event area.
        /// </summary>
        /// <param name="id">id event area to delete.</param>
        /// <returns>truthfulness of remove.</returns>
        [HttpDelete("Delete")]
        [Authorize(Roles = Role.User + ", " + Role.Admin + ", " + Role.EventManager)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var eventAreaIsDelete = await _eventAreaService.DeleteAsync(id);
                return Ok(eventAreaIsDelete);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Method for add event area.
        /// </summary>
        /// <param name="eventAreaDto">event area to add.</param>
        /// <returns>event area.</returns>
        [HttpPost("Add")]
        [Authorize(Roles = Role.User + ", " + Role.Admin + ", " + Role.EventManager)]
        public async Task<IActionResult> AddAsync(EventAreaDto eventAreaDto)
        {
            try
            {
                var eventAreaToAdd = await _eventAreaService.AddAsync(eventAreaDto);
                return Ok(eventAreaToAdd);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Method for update event area.
        /// </summary>
        /// <param name="eventAreaDto">event area to edit.</param>
        /// <returns> event area, that was edited.</returns>
        [HttpPut("Update")]
        [Authorize(Roles = Role.User + ", " + Role.Admin + ", " + Role.EventManager)]
        public async Task<IActionResult> EditAsync(EventAreaDto eventAreaDto)
        {
            try
            {
                var eventAreaToEdit = await _eventAreaService.EditAsync(eventAreaDto);
                return Ok(eventAreaToEdit);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
