using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.EventAPI.Dto;
using TicketManagement.EventAPI.Interfaces;

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
    }
}
