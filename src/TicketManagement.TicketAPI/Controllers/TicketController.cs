using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.TicketAPI.Dto;
using TicketManagement.TicketAPI.Interfaces;
using TicketManagement.TicketAPI.RoleData;

namespace TicketManagement.TicketAPI.Controllers
{
    /// <summary>
    /// Controller for work with ticket.
    /// </summary>.
    [Route("[controller]")]
    public class TicketController : Controller
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        /// <summary>
        /// method for get all ticket.
        /// </summary>
        /// <returns>tickets.</returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var tickets = await _ticketService.GetAllAsync();
            return Ok(tickets);
        }

        /// <summary>
        /// Method for get ticket by id.
        /// </summary>
        /// <param name="id">ticket id.</param>
        /// <returns>ticket.</returns>
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var ticketById = await _ticketService.GetByIdAsync(id);
            return Ok(ticketById);
        }

        /// <summary>
        /// method for get ticket be parent id.
        /// </summary>
        /// <param name="id">parent id.</param>
        /// <returns>ticket.</returns>
        [HttpGet("GetByParenId")]
        public async Task<IActionResult> GetByParentId(int id)
        {
            var ticketById = await _ticketService.GetAsync(id);
            return Ok(ticketById);
        }

        /// <summary>
        /// method for get ticket be parent string id.
        /// </summary>
        /// <param name="id">parent id.</param>
        /// <returns>ticket.</returns>
        [HttpGet("GetByParentStringId")]
        public async Task<IActionResult> GetByParentStringId(string id)
        {
            var ticketById = await _ticketService.GetByParentStringIdAsync(id);
            return Ok(ticketById);
        }

        /// <summary>
        /// method for delete ticket.
        /// </summary>
        /// <param name="id">ticket id.</param>
        /// <returns>truthfulness of remove.</returns>
        [HttpDelete("Delete")]
        [Authorize(Roles = Role.User)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var ticketIsDelete = await _ticketService.DeleteAsync(id);
                return Ok(ticketIsDelete);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// method for add ticket.
        /// </summary>
        /// <param name="ticketDto">ticket.</param>
        /// <returns> ticket that was added.</returns>
        [HttpPost("Add")]
        [Authorize(Roles = Role.User)]
        public async Task<IActionResult> AddAsync([FromBody] TicketDto ticketDto)
        {
            try
            {
                var ticketToAdd = await _ticketService.AddAsync(ticketDto);
                return Ok(ticketToAdd);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// method for update ticket.
        /// </summary>
        /// <param name="ticketDto">ticket to update.</param>
        /// <returns>action result.</returns>
        [HttpPut("Update")]
        [Authorize(Roles = Role.User)]
        public async Task<IActionResult> EditAsync([FromBody] TicketDto ticketDto)
        {
            try
            {
                var ticketToEdit = await _ticketService.EditAsync(ticketDto);
                return Ok(ticketToEdit);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// method for get ticket be parent string id.
        /// </summary>
        /// <param name="id">parent id.</param>
        /// <returns>ticket.</returns>
        [HttpGet("GetUserTicketInfo")]
        public async Task<IActionResult> GetUserTicketInfo(string id)
        {
            var ticketInfoById = await _ticketService.GetUserTicketInfo(id);
            return Ok(ticketInfoById);
        }
    }
}
