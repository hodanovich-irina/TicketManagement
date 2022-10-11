using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Presentation.Client;
using TicketManagement.Presentation.Dto;
using TicketManagement.Presentation.Filters;
using TicketManagement.Presentation.Models;
using TicketManagement.Presentation.RoleData;

namespace TicketManagement.Presentation.Controllers
{
    /// <summary>
    /// Controller for work with areas.
    /// </summary>
    [Authorize(Roles = Role.VenueManager + ", " + Role.Admin)]
    public class AreaController : Controller
    {
        private readonly IVenueRestClient _venueClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="AreaController"/> class.
        /// Class constructor.
        /// </summary>
        /// <param name="venueClient"> Venue client.</param>
        public AreaController(IVenueRestClient venueClient)
        {
            _venueClient = venueClient;
        }

        /// <summary>
        /// Method for select area data.
        /// </summary>
        /// <param name="id">venue id.</param>
        /// <returns>view result.</returns>
        public async Task<IActionResult> Index(int id)
        {
            var areas = await _venueClient.GetAreaByParentIdAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);
            var layout = await _venueClient.GetLayoutByIdAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);
            var areasWithlayout = new List<AreaViewModel>();
            foreach (var area in areas)
            {
                areasWithlayout.Add(new AreaViewModel
                {
                    Id = area.Id,
                    Description = area.Description,
                    CoordX = area.CoordY,
                    CoordY = area.CoordY,
                    LayoutName = layout.Name,
                });
            }

            return View(areasWithlayout);
        }

        /// <summary>
        /// Method for create layout data.
        /// </summary>
        /// <returns>view result.</returns>
        public async Task<IActionResult> Create()
        {
            var layouts = await _venueClient.GetAllLayoutAsync(HttpContext.Request.Cookies["secret_jwt_key"]);

            return View(new AreaViewModel { Layouts = layouts.ToList() });
        }

        /// <summary>
        /// Method for create area data.
        /// </summary>
        /// <param name="area">Object of area.</param>
        /// <returns>redirect to action.</returns>
        [HttpPost]
        [ValidationExceptionFilter]
        public async Task<IActionResult> Create(AreaViewModel area)
        {
            var model = new AreaDto
            {
                Id = area.Id,
                Description = area.Description,
                CoordX = area.CoordX,
                CoordY = area.CoordY,
                LayoutId = area.LayoutId,
            };
            await _venueClient.AddAreaAsync(model, HttpContext.Request.Cookies["secret_jwt_key"]);
            return Redirect($"~/Area/Index?id={area.LayoutId}");
        }

        /// <summary>
        /// Method for edit area data.
        /// </summary>
        /// <param name="id">area id.</param>
        /// <returns>view result.</returns>
        public async Task<IActionResult> Edit(int id)
        {
            var layouts = await _venueClient.GetAllLayoutAsync(HttpContext.Request.Cookies["secret_jwt_key"]);

            var areaToEdit = await _venueClient.GetAreaByIdAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);

            var model = new AreaViewModel
            {
                Id = areaToEdit.Id,
                Description = areaToEdit.Description,
                CoordX = areaToEdit.CoordX,
                CoordY = areaToEdit.CoordY,
                LayoutId = areaToEdit.LayoutId,
                Layouts = layouts.ToList(),
            };
            return View(model);
        }

        /// <summary>
        /// Method for edit area data.
        /// </summary>
        /// <param name="areaDto">Object of area.</param>
        /// <returns>redirect to action.</returns>
        [HttpPost]
        [ValidationExceptionFilter]
        public async Task<IActionResult> Edit(AreaDto areaDto)
        {
            await _venueClient.EditAreaAsync(areaDto, HttpContext.Request.Cookies["secret_jwt_key"]);
            return Redirect($"~/Area/Index?id={areaDto.LayoutId}");
        }

        /// <summary>
        /// Method for delete area data.
        /// </summary>
        /// <param name="id">area id.</param>
        /// <returns>view result.</returns>
        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var area = await _venueClient.GetAreaByIdAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);
            var layout = await _venueClient.GetLayoutByIdAsync(area.LayoutId, HttpContext.Request.Cookies["secret_jwt_key"]);
            var areaToDelete = new AreaViewModel
            {
                Id = id,
                Description = area.Description,
                CoordX = area.CoordX,
                CoordY = area.CoordY,
                LayoutName = layout.Name,
                LayoutId = area.LayoutId,
            };
            return View(areaToDelete);
        }

        /// <summary>
        /// Method for delete area data.
        /// </summary>
        /// <param name="id">area id.</param>
        /// <returns>redirect.</returns>
        [HttpPost]
        [ValidationExceptionFilter]
        public async Task<IActionResult> Delete(int id)
        {
            var layout = await _venueClient.GetAreaByIdAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);
            await _venueClient.DeleteAreaAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);
            return Redirect($"~/Area/Index?id={layout.LayoutId}");
        }
    }
}
