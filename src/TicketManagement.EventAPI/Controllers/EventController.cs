using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.EventAPI.Dto;
using TicketManagement.EventAPI.Interfaces;
using TicketManagement.EventAPI.RoleData;

namespace TicketManagement.EventAPI.Controllers
{
    /// <summary>
    /// Controller for work with events.
    /// </summary>
    [Route("[controller]")]
    public class EventController : Controller
    {
        private readonly IService<EventDto> _eventService;

        public EventController(IService<EventDto> eventService)
        {
            _eventService = eventService;
        }

        /// <summary>
        /// Method for get all events.
        /// </summary>
        /// <returns>events.</returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var events = new List<EventModel>();
            var eventsGet = await _eventService.GetAllAsync();
            foreach (var eventToReturn in eventsGet)
            {
                events.Add(ReturnModel(eventToReturn));
            }

            return Ok(events);
        }

        /// <summary>
        /// Method for get event by id.
        /// </summary>
        /// <param name="id">event id.</param>
        /// <returns>event.</returns>
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var eventById = await _eventService.GetByIdAsync(id);
            return Ok(ReturnModel(eventById));
        }

        /// <summary>
        /// Method for get event by parent id.
        /// </summary>
        /// <param name="id">parent id.</param>
        /// <returns>event by id.</returns>
        [HttpGet("GetByParentId")]
        public async Task<IActionResult> GetByParentId(int id)
        {
            var events = new List<EventModel>();
            var eventsByParentId = await _eventService.GetAsync(id);
            foreach (var evntById in eventsByParentId)
            {
                events.Add(ReturnModel(evntById));
            }

            return Ok(events);
        }

        /// <summary>
        /// Method for delete event.
        /// </summary>
        /// <param name="id">event id.</param>
        /// <returns>truthfulness of remove.</returns>
        [HttpDelete("Delete")]
        [Authorize(Roles = Role.Admin + ", " + Role.EventManager)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var eventIsDelete = await _eventService.DeleteAsync(id);
                return Ok(eventIsDelete);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// method for add event.
        /// </summary>
        /// <param name="eventModel">event.</param>
        /// <returns>action result.</returns>
        [HttpPost("Add")]
        [Authorize(Roles = Role.Admin + ", " + Role.EventManager)]
        public async Task<IActionResult> AddAsync([FromBody] EventModel eventModel)
        {
            try
            {
                await _eventService.AddAsync(ReturnDto(eventModel));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        /// <summary>
        /// Method for edit event.
        /// </summary>
        /// <param name="eventModel">event.</param>
        /// <returns>event to edit.</returns>
        [HttpPut("Update")]
        [Authorize(Roles = Role.Admin + ", " + Role.EventManager)]
        public async Task<IActionResult> EditAsync([FromBody] EventModel eventModel)
        {
            try
            {
                var eventToEdit = await _eventService.EditAsync(ReturnDto(eventModel));
                return Ok(eventToEdit);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private static EventModel ReturnModel(EventDto eventDto)
        {
            var eventModel = new EventModel
            {
                BaseAreaPrice = eventDto.BaseAreaPrice,
                DateEnd = eventDto.DateEnd,
                DateStart = eventDto.DateStart,
                Description = eventDto.Description,
                Id = eventDto.Id,
                ImageURL = eventDto.ImageURL,
                LayoutId = eventDto.LayoutId,
                Name = eventDto.Name,
                Minutes = eventDto.ShowTime.Minutes,
                Seconds = eventDto.ShowTime.Seconds,
                Hours = eventDto.ShowTime.Hours,
            };

            return eventModel;
        }

        private static EventDto ReturnDto(EventModel eventModel)
        {
            var eventDto = new EventDto
            {
                BaseAreaPrice = eventModel.BaseAreaPrice,
                DateEnd = eventModel.DateEnd,
                DateStart = eventModel.DateStart,
                Description = eventModel.Description,
                Id = eventModel.Id,
                ImageURL = eventModel.ImageURL,
                LayoutId = eventModel.LayoutId,
                Name = eventModel.Name,
                ShowTime = new TimeSpan(eventModel.Hours, eventModel.Minutes, eventModel.Seconds),
            };

            return eventDto;
        }
    }
}
