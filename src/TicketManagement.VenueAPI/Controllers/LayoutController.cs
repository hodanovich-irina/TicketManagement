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
    /// Controller for work with layouts.
    /// </summary>
    [Route("[controller]")]
    public class LayoutController : Controller
    {
        private readonly IService<LayoutDto> _layoutService;

        public LayoutController(IService<LayoutDto> layoutService)
        {
            _layoutService = layoutService;
        }

        /// <summary>
        /// Method for get all layouts.
        /// </summary>
        /// <returns>layouts.</returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var layouts = await _layoutService.GetAllAsync();
            return Ok(layouts);
        }

        /// <summary>
        /// Method for get layout by id.
        /// </summary>
        /// <param name="id">layout id.</param>
        /// <returns>layout.</returns>
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var layoutById = await _layoutService.GetByIdAsync(id);
            return Ok(layoutById);
        }

        /// <summary>
        /// Method for get layout by parent id.
        /// </summary>
        /// <param name="id">parent id.</param>
        /// <returns>layout.</returns>
        [HttpGet("GetByParenId")]
        public async Task<IActionResult> GetByParentId(int id)
        {
            var layoutById = await _layoutService.GetAsync(id);
            return Ok(layoutById);
        }

        /// <summary>
        /// Method for delete layout.
        /// </summary>
        /// <param name="id">layout id.</param>
        /// <returns>truthfulness of remove.</returns>
        [HttpDelete("Delete")]
        [Authorize(Roles = Role.User + ", " + Role.Admin + ", " + Role.VenueManager)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var layoutIsDelete = await _layoutService.DeleteAsync(id);
                return Ok(layoutIsDelete);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Method for add layout.
        /// </summary>
        /// <param name="layoutDto">layout.</param>
        /// <returns>layout, that was added.</returns>
        [HttpPost("Add")]
        [Authorize(Roles = Role.User + ", " + Role.Admin + ", " + Role.VenueManager)]
        public async Task<IActionResult> AddAsync([FromBody] LayoutDto layoutDto)
        {
            try
            {
                var layoutToAdd = await _layoutService.AddAsync(layoutDto);
                return Ok(layoutToAdd);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Method for update layout.
        /// </summary>
        /// <param name="layoutDto">layout.</param>
        /// <returns>layout, that was edited.</returns>
        [HttpPut("Update")]
        [Authorize(Roles = Role.User + ", " + Role.Admin + ", " + Role.VenueManager)]
        public async Task<IActionResult> EditAsync([FromBody] LayoutDto layoutDto)
        {
            try
            {
                var layoutToEdit = await _layoutService.EditAsync(layoutDto);
                return Ok(layoutToEdit);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
