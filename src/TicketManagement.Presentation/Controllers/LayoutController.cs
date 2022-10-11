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
    /// Controller for work with layouts.
    /// </summary>
    [Authorize(Roles = Role.User + ", " + Role.Admin + ", " + Role.VenueManager)]
    public class LayoutController : Controller
    {
        private readonly IVenueRestClient _venueClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutController"/> class.
        /// Class constructor.
        /// </summary>
        /// <param name="venueClient"> venue client.</param>
        public LayoutController(IVenueRestClient venueClient)
        {
            _venueClient = venueClient;
        }

        /// <summary>
        /// Method for select layout data.
        /// </summary>
        /// <param name="id">venue id.</param>
        /// <returns>view result.</returns>
        public async Task<IActionResult> Index(int id)
        {
            var layouts = await _venueClient.GetLayoutByParentIdAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);
            var venue = await _venueClient.GetVenueByIdAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);
            var layoutsWithVenue = new List<LayoutViewModel>();
            foreach (var layoutModel in layouts)
            {
                layoutsWithVenue.Add(new LayoutViewModel
                {
                    Id = layoutModel.Id,
                    Description = layoutModel.Description,
                    Name = layoutModel.Name,
                    VenueName = venue.Name,
                });
            }

            return View(layoutsWithVenue);
        }

        /// <summary>
        /// Method for create layout layout.
        /// </summary>
        /// <returns>view result.</returns>
        public async Task<IActionResult> Create()
        {
            var venues = await _venueClient.GetAllVenueAsync(HttpContext.Request.Cookies["secret_jwt_key"]);
            venues.Select(venue => new VenueDto
            {
                Id = venue.Id,
                Name = venue.Name,
            });

            return View(new LayoutCreateViewModel { Venues = venues.ToList() });
        }

        /// <summary>
        /// Method for create layout.
        /// </summary>
        /// <param name="layoutToCreate">Object of layout.</param>
        /// <returns>redirect to action.</returns>
        [HttpPost]
        [ValidationExceptionFilter]
        public async Task<IActionResult> Create(LayoutCreateViewModel layoutToCreate)
        {
            var model = new LayoutDto
            {
                Id = layoutToCreate.Id,
                Description = layoutToCreate.Description,
                Name = layoutToCreate.Name,
                VenueId = layoutToCreate.VenueId,
            };
            await _venueClient.AddLayoutAsync(model, HttpContext.Request.Cookies["secret_jwt_key"]);
            return Redirect($"~/Layout/Index?id={layoutToCreate.VenueId}");
        }

        /// <summary>
        /// Method for edit layout data.
        /// </summary>
        /// <param name="id">layout id.</param>
        /// <returns>view result.</returns>
        public async Task<IActionResult> Edit(int id)
        {
            var venues = await _venueClient.GetAllVenueAsync(HttpContext.Request.Cookies["secret_jwt_key"]);
            venues.Select(venue => new VenueDto
            {
                Id = venue.Id,
                Name = venue.Name,
            });

            var layoutToEdit = await _venueClient.GetLayoutByIdAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);

            var model = new LayoutCreateViewModel
            {
                Id = layoutToEdit.Id,
                Description = layoutToEdit.Description,
                Name = layoutToEdit.Name,
                VenueId = layoutToEdit.VenueId,
                Venues = venues.ToList(),
            };
            return View(model);
        }

        /// <summary>
        /// Method for edit layout data.
        /// </summary>
        /// <param name="layoutDto">Object of layout.</param>
        /// <returns>redirect to action.</returns>
        [HttpPost]
        [ValidationExceptionFilter]
        public async Task<IActionResult> Edit(LayoutDto layoutDto)
        {
            await _venueClient.EditLayoutAsync(layoutDto, HttpContext.Request.Cookies["secret_jwt_key"]);
            return Redirect($"~/Layout/Index?id={layoutDto.VenueId}");
        }

        /// <summary>
        /// Method for delete layout data.
        /// </summary>
        /// <param name="id">layout id.</param>
        /// <returns>view result.</returns>
        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var layout = await _venueClient.GetLayoutByIdAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);
            var venue = await _venueClient.GetVenueByIdAsync(layout.VenueId, HttpContext.Request.Cookies["secret_jwt_key"]);
            var layoutToDelete = new LayoutCreateViewModel
            {
                Id = id,
                Description = layout.Description,
                Name = layout.Name,
                VenueId = layout.VenueId,
                VenueName = venue.Name,
            };
            return View(layoutToDelete);
        }

        /// <summary>
        /// Method for delete layout data.
        /// </summary>
        /// <param name="id">layout id.</param>
        /// <returns>redirect action.</returns>
        [HttpPost]
        [ValidationExceptionFilter]
        public async Task<IActionResult> Delete(int id)
        {
            var layout = await _venueClient.GetLayoutByIdAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);
            await _venueClient.DeleteLayoutAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);
            return Redirect($"~/Layout/Index?id={layout.VenueId}");
        }
    }
}
