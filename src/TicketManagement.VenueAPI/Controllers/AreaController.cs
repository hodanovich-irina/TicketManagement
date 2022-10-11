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
    /// Controller for work with areas.
    /// </summary>
    [Route("[controller]")]
    public class AreaController : Controller
    {
        private readonly IService<AreaDto> _areaService;

        public AreaController(IService<AreaDto> areaService)
        {
            _areaService = areaService;
        }

        /// <summary>
        /// Method for get all areas.
        /// </summary>
        /// <returns>areas.</returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var areas = await _areaService.GetAllAsync();
            return Ok(areas);
        }

        /// <summary>
        /// Method for get area by id.
        /// </summary>
        /// <param name="id">area id.</param>
        /// <returns>area.</returns>
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var areaById = await _areaService.GetByIdAsync(id);
            return Ok(areaById);
        }

        /// <summary>
        /// Method for get area by parent id.
        /// </summary>
        /// <param name="id">parent id.</param>
        /// <returns>area.</returns>
        [HttpGet("GetByParenId")]
        public async Task<IActionResult> GetByParentId(int id)
        {
            var areaById = await _areaService.GetAsync(id);
            return Ok(areaById);
        }

        /// <summary>
        /// Method for delete area.
        /// </summary>
        /// <param name="id">area id.</param>
        /// <returns>truthfulness of remove.</returns>
        [HttpDelete("Delete")]
        [Authorize(Roles = Role.VenueManager + ", " + Role.Admin)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var areaIsDelete = await _areaService.DeleteAsync(id);
                return Ok(areaIsDelete);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Method for add area.
        /// </summary>
        /// <param name="areaDto">area.</param>
        /// <returns>area, that was added.</returns>
        [HttpPost("Add")]
        [Authorize(Roles = Role.VenueManager + ", " + Role.Admin)]
        public async Task<IActionResult> AddAsync([FromBody] AreaDto areaDto)
        {
            try
            {
                var areaToAdd = await _areaService.AddAsync(areaDto);
                return Ok(areaToAdd);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Method for update area.
        /// </summary>
        /// <param name="areaDto">area.</param>
        /// <returns>area, that was updated.</returns>
        [HttpPut("Update")]
        [Authorize(Roles = Role.VenueManager + ", " + Role.Admin)]
        public async Task<IActionResult> EditAsync([FromBody] AreaDto areaDto)
        {
            try
            {
                var areaToEdit = await _areaService.EditAsync(areaDto);
                return Ok(areaToEdit);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
